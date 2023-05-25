using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public interface IAttack
{
    void ChangeHealth(float damage,HPType damageType=HPType.Default);
    //void ShowDamage(float damage);
    void TakeMove(float speed, float time, Vector3 direction=default);
    void TakeBuff(GameObject attacker,GameObject bullet,BuffType buffType, int piles = 1);
}

public class CharacterBase : MonoBehaviour, IAttack
{
    public CharacterData characterData;

    public CharacterType characterType;
    public string characterTag;

    public ICharacterState currentState;
    public Dictionary<CharacterStateType, ICharacterState> statesDic = new Dictionary<CharacterStateType, ICharacterState>();

    public float maxHP;
    public float nowHP;
    public float aggressivity;//攻击力加成
    public float ATKSpeed;//攻速加成


    public Dictionary<BuffType, (int,float)> buffDic=new Dictionary<BuffType, (int, float)>();//里面存放的是每个buff和其对应的层数与持续时间
    public Dictionary<BulletType, float> nowBullet = new Dictionary<BulletType, float>();//存放角色的攻击方式（弹幕类型,值是每种子弹的攻击间隔）
    //public Dictionary<BulletType, List<BuffType>> bulletBuff = new Dictionary<BulletType, List<BuffType>>();
    public Dictionary<BulletType, float> bulletTimer = new Dictionary<BulletType, float>();

    public Animator animator;
    public HPSlider hpSlider;

    public bool canActive;
    protected event UnityAction characterActiveEvent;//角色会主动进行的行为，比如攻击，移动
    protected event UnityAction characterPassiveEvent;//角色被动执行的行为，如身上的buff倒计时，是否被回收

    protected void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
        characterPassiveEvent += BuffCheck;
    }

    /*protected void Start()
    {
        characterData = DataManager.GetInstance().AskCharacterData(characterType);
        InitData();
    }*/

    protected void OnEnable()//从对象池取出的时候会置为初始状态
    {
        nowHP = maxHP;
        canActive = true;

        if (characterTag=="Enemy") GameManager.GetInstance().EnemyIncrease(this.gameObject);
        TransitionState(CharacterStateType.Idle);
    }

    protected void BuffCheck()
    {
        for(int i = 0; i < buffDic.Count; i++)
        {
            var item = buffDic.ElementAt(i);
            float t = item.Value.Item2 - Time.deltaTime;
            if (t<= 0.0001)
            {
                buffDic[item.Key]= BuffManager.GetInstance().Buffs[item.Key].OnZero(this.gameObject, item.Value.Item1);
                if (buffDic[item.Key].Item1 <= 0)
                {
                    BuffManager.GetInstance().Buffs[item.Key].OnEnd(this.gameObject);
                    buffDic.Remove(item.Key);
                }
            }
            else buffDic[item.Key] = (item.Value.Item1, t);
        }
    }

    protected void InitData()
    {
        characterData = DataManager.GetInstance().AskCharacterData(characterType);
        maxHP = characterData.MaxHP;
        nowHP = maxHP;
        aggressivity = characterData.Aggressivity;
        ATKSpeed = characterData.ATKSpeed;

        foreach(var item in characterData.bulletTypes)
        {
            nowBullet.Add(item, DataManager.GetInstance().AskBulletData(item).transmissionFrequency);
            //bulletBuff.Add(item, DataManager.GetInstance().AskBulletData(item).buffList);
            bulletTimer.Add(item, nowBullet[item]);
        }
    }

    protected void Update()
    {
        if (characterPassiveEvent != null) characterPassiveEvent();

        if (!canActive) return;
        if (currentState != null) currentState.OnUpdate();
        if(characterActiveEvent!=null) characterActiveEvent();
    }

    public void ChangeHealth(float variation,HPType hpType=HPType.Default)
    {
        if (variation > 0) nowHP = Mathf.Min(maxHP, nowHP + variation);
        else nowHP= nowHP + variation > 0 ? nowHP + variation : 0;

        if (hpSlider) hpSlider.UpdateHPSlider(maxHP, nowHP);
        GameManager.GetInstance().ShowDamage(this.transform,variation, hpType);
        if (nowHP <= 0) DiedEvent();
    }
    
    /// <summary>
    /// 给可能影响角色移动的buff或者攻击使用
    /// </summary>
    /// <param name="speed">移动速度，小于0不移动只减速，小于-99冻结行动</param>
    /// <param name="time">受影响时间</param>
    /// <param name="direction">移动方向，默认为随机方向</param>
    public void TakeMove(float speed,float time,Vector3 direction=default)
    {
        if (speed<=-99)
        {
            Debug.Log("冻结敌人");
            animator.speed = 0;
            if (this.transform.parent != null)//当前角色有父物体（跟随地板移动）则只限制其行动
            {
                canActive = false;
            }
            return;
        }
        if(direction==default)
        {
            MonoManager.GetInstance().StartCoroutine(RandomMove(speed,time));
        }
        gameObject.transform.DOMove(transform.position + speed * time * direction, time);
    }
    public IEnumerator RandomMove(float speed, float time)
    {
        yield return null;
    }

    public void TakeBuff(GameObject attacker,GameObject bullet, BuffType buffType,int piles=1)
    {
        if (!buffDic.ContainsKey(buffType))
        {
            buffDic.Add(buffType, BuffManager.GetInstance().Buffs[buffType].OnAdd(attacker, bullet, this.gameObject));           
        }
        else
        {
            (int, float) t =
                (buffDic[buffType].Item1 + BuffManager.GetInstance().Buffs[buffType].OnSuperpose(attacker, this.gameObject, piles).Item1
                , buffDic[buffType].Item2 + BuffManager.GetInstance().Buffs[buffType].OnSuperpose(attacker, this.gameObject, piles).Item2);
            buffDic[buffType] = t;
        }
    }

    public void TransitionState(CharacterStateType characterStateType)
    {
        if (!statesDic.ContainsKey(characterStateType)) return;
        if (currentState != null)
            currentState.OnExit();
        currentState = statesDic[characterStateType];
        currentState.OnEnter();
    } 

    public virtual void Attack()
    {
        foreach(var item in nowBullet)
        {
            if (item.Key == BulletType.Combat)//近战攻击
            {
                if ((transform.position - Player._instance.transform.position).magnitude < 2f && bulletTimer[item.Key] <= 0)
                {
                    TransitionState(CharacterStateType.Attack);
                    Player._instance.ChangeHealth(
                        -BulletManager.GetInstance().BulletDic[item.Key].baseATK + aggressivity);
                    bulletTimer[item.Key] = item.Value;
                }
                else
                {
                    bulletTimer[item.Key] -= (1 + ATKSpeed) * Time.deltaTime;
                }
                continue;
            }
            bulletTimer[item.Key] -= (1+ATKSpeed)*Time.deltaTime;
            if (bulletTimer[item.Key] <= 0)
            {
                BulletManager.GetInstance().BulletLauncher(gameObject.transform, item.Key, aggressivity);
                bulletTimer[item.Key] = item.Value;
            }
        }
    }

    public virtual void DiedEvent()
    {
        if (characterTag != "Player")
        {
            GameManager.GetInstance().ChangeEnergy(characterData.energy);            
            FallMoney();
        }
        else
        {
            GameManager.GetInstance().QuitGame();
        }
        Recovery(); 
    }

    public void CheckDistance()
    {
        if ((Player._instance.transform.position - transform.position).z > 1.5f) Recovery();
    }

    public void Recovery()
    {
        if (characterTag == "Enemy") GameManager.GetInstance().EnemyDecrease(this.gameObject);
        foreach (var item in buffDic)
        {
            BuffManager.GetInstance().Buffs[item.Key].OnEnd(this.gameObject);
        }
        buffDic.Clear();
        PoolManager.GetInstance().PushObj(characterType.ToString(), this.gameObject);
    }

    public void FallMoney()
    {
        for(int i = 0; i < characterData.money; i++)
        {
            GameObject t = PoolManager.GetInstance().GetObj(PropType.Money.ToString());
            
            t.transform.position = gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
        }
    }
}
