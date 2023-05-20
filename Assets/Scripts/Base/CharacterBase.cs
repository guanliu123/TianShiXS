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
    void TakeDamage(float damage,DamageType damageType=DamageType.Default);
    //void ShowDamage(float damage);
    void TakeMove();
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

    protected event UnityAction characterEvent;

    protected void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
        characterEvent += BuffCheck;
    }

    /*protected void Start()
    {
        characterData = DataManager.GetInstance().AskCharacterData(characterType);
        InitData();
    }*/

    protected void OnEnable()//从对象池取出的时候会置为初始状态
    {
        nowHP = maxHP;
        if (characterTag!="Player") GameManager.GetInstance().EnemyIncrease(this.gameObject);
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
        if (currentState != null) currentState.OnUpdate();
        if(characterEvent!=null) characterEvent();
    }

    public void AddHP(float add)
    {
        nowHP = Mathf.Min(maxHP, nowHP + add);
    }

    public void TakeDamage(float damage,DamageType damageType=DamageType.Default)
    {
        nowHP=nowHP - damage>0?nowHP-damage:0;
        if (hpSlider) hpSlider.UpdateHPSlider(maxHP, nowHP);
        GameManager.GetInstance().ShowDamage(this.transform,damage);
        if (nowHP <= 0) DiedEvent();
    }
    
    public void TakeMove()
    {

    }
    public void TakeBuff(GameObject attacker,GameObject bullet, BuffType buffType,int piles=1)
    {
        if (!buffDic.ContainsKey(buffType))
        {
            buffDic.Add(buffType, (piles,DataManager.GetInstance().AskBuffDate(buffType).duration));
            BuffManager.GetInstance().Buffs[buffType].OnAdd(attacker,bullet, this.gameObject);
        }
        else BuffManager.GetInstance().Buffs[buffType].OnSuperpose(attacker,this.gameObject,piles);
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
        Recovery(); 
    }

    public void VisibleCheck()
    {
        if (!gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible) Recovery();
    }

    public void Recovery()
    {
        if (characterTag != "Player") GameManager.GetInstance().EnemyDecrease(this.gameObject);
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

    public void AddCharacterEvent(UnityAction unityAction)
    {
        characterEvent += unityAction;
    }
    public void RemoveCharacterEvent(UnityAction unityAction)
    {

        characterEvent -= unityAction;
    }
}
