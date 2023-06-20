using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Events;
//using static UnityEditor.PlayerSettings;

public interface IAttack
{
    void ChangeHealth(GameObject applicator, float damage,HPType damageType=HPType.Default);
    //void ShowDamage(float damage);
    void TakeMove(float speed, float time, Vector3 direction=default);
    void TakeBuff(GameObject attacker,GameObject bullet,BuffType buffType, int piles = 1);
}

public class CharacterBase : MonoBehaviour, IAttack
{
    public CharacterData characterData;

    public CharacterType characterType;
    public CharacterTag characterTag;

    public ICharacterState currentState;
    public Dictionary<CharacterStateType, ICharacterState> statesDic = new Dictionary<CharacterStateType, ICharacterState>();

    public float maxHP;
    public float nowHP;
    public float aggressivity;//�������ӳ�
    public float ATKSpeed;//���ټӳ�
    public float _avoidance;//������

    public Dictionary<BuffType, (int,float)> buffDic=new Dictionary<BuffType, (int, float)>();//�����ŵ���ÿ��buff�����Ӧ�Ĳ��������ʱ��
    public Dictionary<BulletType, float> nowBullet = new Dictionary<BulletType, float>();//��Ž�ɫ�Ĺ�����ʽ����Ļ����,ֵ��ÿ���ӵ��Ĺ��������
    //public Dictionary<BulletType, List<BuffType>> bulletBuff = new Dictionary<BulletType, List<BuffType>>();
    public Dictionary<BulletType, float> bulletTimer = new Dictionary<BulletType, float>();

    public Animator animator;
    public HPSlider hpSlider;

    public bool canActive;
    protected event UnityAction characterActiveEvent;//��ɫ���������е���Ϊ�����繥�����ƶ�
    protected event UnityAction characterPassiveEvent;//��ɫ����ִ�е���Ϊ�������ϵ�buff����ʱ���Ƿ񱻻���
    public event UnityAction<GameObject,GameObject,float,HPType> attackedEvent;//�ܻ��¼�

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

    protected void OnEnable()//�Ӷ����ȡ����ʱ�����Ϊ��ʼ״̬
    {
        nowHP = maxHP;
        canActive = true;
        gameObject.tag = characterTag.ToString();

        if (characterTag==CharacterTag.Enemy) GameManager.GetInstance().EnemyIncrease(this.gameObject);
        TransitionState(CharacterStateType.Idle);
    }

    protected void BuffCheck()
    {
        for(int i = 0; i < buffDic.Count; i++)
        {
            var item = buffDic.ElementAt(i);
            if (item.Value.Item2 > 999) continue;//����ʱ�����999��buff��Ϊ������buff
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

    public void AddBullet(BulletType bulletType)
    {
        if (nowBullet.ContainsKey(bulletType))
        {
            BulletManager.GetInstance().BulletEvolute(BuffType.Multiply,bulletType);
            return;
        }
        nowBullet.Add(bulletType, BulletManager.GetInstance().BulletDic[BulletType.RotateBullet].damageInterval);
        bulletTimer.Add(bulletType, BulletManager.GetInstance().BulletDic[BulletType.RotateBullet].damageInterval);
    }

    protected void InitData(bool isPlayer=false)
    {
        int askNum = isPlayer ? 0 : LevelManager.GetInstance().nowLevelNum;
        (CharacterTag, CharacterData) t = DataManager.GetInstance().AskCharacterData(characterType,askNum);
        characterTag = t.Item1;
        if (characterTag == CharacterTag.Null) Recovery();
        characterData = t.Item2;
        maxHP = characterData.MaxHP;
        nowHP = maxHP;
        aggressivity = characterData.Aggressivity;
        ATKSpeed = characterData.ATKSpeed;       

        if (hpSlider)
        {
            hpSlider.UpdateHPSlider(maxHP, nowHP);
        }

        foreach (var item in characterData.bulletTypes)
        {
            //Debug.Log(item);
            if (!nowBullet.ContainsKey(item)) nowBullet.Add(item, BulletManager.GetInstance().BulletDic[item].transmissionFrequency);
            
            //bulletBuff.Add(item, DataManager.GetInstance().AskBulletData(item).buffList);
            if(!bulletTimer.ContainsKey(item)) bulletTimer.Add(item, nowBullet[item]);
        }
    }

    protected void Update()
    {
        if (characterPassiveEvent != null) characterPassiveEvent();

        if (!canActive) return;
        if (currentState != null) currentState.OnUpdate();
        if(characterActiveEvent!=null) characterActiveEvent();
    }

    public void ChangeHealth(GameObject applicator,float variation,HPType hpType=HPType.Default)
    {
        if(attackedEvent!=null) attackedEvent(applicator, gameObject,variation, hpType);
        if (variation > 0) nowHP = Mathf.Min(maxHP, nowHP + variation);
        else nowHP= nowHP + variation > 0 ? nowHP + variation : 0;

        if (hpSlider) hpSlider.UpdateHPSlider(maxHP, nowHP);
        GameManager.GetInstance().ShowDamage(this.transform,variation, hpType);
        if (nowHP <= 0) DiedEvent();
    }
    
    /// <summary>
    /// ������Ӱ���ɫ�ƶ���buff���߹���ʹ��
    /// </summary>
    /// <param name="speed">�ƶ��ٶȣ�С��0���ƶ�ֻ���٣�С��-99�����ж�</param>
    /// <param name="time">��Ӱ��ʱ��</param>
    /// <param name="direction">�ƶ�����Ĭ��Ϊ�������</param>
    public void TakeMove(float speed,float time,Vector3 direction=default)
    {
        if (speed<=-99)
        {
            animator.speed = 0;
            if (this.transform.parent != null)//��ǰ��ɫ�и����壨����ذ��ƶ�����ֻ�������ж�
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
            //(int, float) t = (piles, 0f);
            buffDic.Add(buffType, BuffManager.GetInstance().Buffs[buffType].Init());
            if(buffDic[buffType].Item1!=0) BuffManager.GetInstance().Buffs[buffType].OnAdd(attacker, bullet, this.gameObject);           
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
        foreach (var item in nowBullet)
        {
            float atkSpeed = ATKSpeed;
            if (characterTag == CharacterTag.Player) atkSpeed += BulletManager.GetInstance().increaseShootTimer[item.Key];

            if (item.Key == BulletType.Combat)//��ս����
            {
                if ((transform.position - Player._instance.transform.position).magnitude < 2f && bulletTimer[item.Key] <= 0)
                {
                    TransitionState(CharacterStateType.Attack);
                    Player._instance.ChangeHealth(gameObject,
                        -(BulletManager.GetInstance().BulletDic[item.Key].ATK + aggressivity));
                    bulletTimer[item.Key] = item.Value;
                }
                else
                {
                    bulletTimer[item.Key] -= (1 + atkSpeed) * Time.deltaTime;
                }
                continue;
            }
            bulletTimer[item.Key] -= (1+ atkSpeed) *Time.deltaTime;
            if (bulletTimer[item.Key] <= 0)
            {
                //GameObject t = PoolManager.GetInstance().GetObj(item.Key.ToString());
                BulletManager.GetInstance().BulletLauncher(transform, item.Key, aggressivity,gameObject);
                bulletTimer[item.Key] = item.Value;
            }
        }
    }

    public virtual void DiedEvent()
    {
        if (characterTag != CharacterTag.Player)
        {
            GameManager.GetInstance().ChangeEnergy(characterData.energy);
            GameManager.GetInstance().FallMoney(transform, characterData.money);
        }
        else
        {
            PanelManager.Instance.Push(new FailPanel());
            //GameManager.GetInstance().QuitGame();
        }
        Recovery(); 
    }

    public void CheckDistance()
    {
        if ((Player._instance.transform.position - transform.position).z > 1.5f) Recovery();
    }

    public void Recovery()
    {
        if (characterTag == CharacterTag.Enemy) GameManager.GetInstance().EnemyDecrease(this.gameObject);
        foreach (var item in buffDic)
        {
            BuffManager.GetInstance().Buffs[item.Key].OnEnd(this.gameObject);
        }
        buffDic.Clear();
        if(characterTag!= CharacterTag.Player) PoolManager.GetInstance().PushObj(characterType.ToString(), this.gameObject);
    }
}
