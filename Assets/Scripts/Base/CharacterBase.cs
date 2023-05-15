using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;

public interface IAttack
{
    void TakeDamage(float damage);
    //void ShowDamage(float damage);
    void TakeBuff(BuffType buffType);
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


    public Dictionary<BuffType, float> buffDic=new Dictionary<BuffType, float>();//里面存放的是每个buff和其对应的持续时间
    public Dictionary<BulletType, float> nowBullet = new Dictionary<BulletType, float>();//存放角色的攻击方式（弹幕类型,值是每种子弹的攻击间隔）
    public Dictionary<BulletType, List<BuffType>> bulletBuff = new Dictionary<BulletType, List<BuffType>>();
    public Dictionary<BulletType, float> bulletTimer = new Dictionary<BulletType, float>();

    public Animator animator;
    public HPSlider hpSlider;

    protected event UnityAction characterEvent;

    protected void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
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
            bulletBuff.Add(item, DataManager.GetInstance().AskBulletData(item).buffList);
            bulletTimer.Add(item, nowBullet[item]);
        }
    }

    protected void Update()
    {
        if (currentState != null) currentState.OnUpdate();
        if(characterEvent!=null) characterEvent();
    }

    public void TakeDamage(float damage)
    {
        nowHP -= damage;
        if (hpSlider) hpSlider.UpdateHPSlider(maxHP, nowHP);
        //ShowDamage(damage);
        if (nowHP <= 0) DiedEvent();
    }
    public void ShowDamage(float damage)
    {
        GameObject obj = PoolManager.GetInstance().GetObj("DamageText");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        //obj.transform.position = point.position;
        obj.transform.position = screenPos;
        obj.GetComponent<TextMesh>().text = damage + "";
        
        /*Vector3 cameraPoint = new Vector3(Screen.width / 2, 0, Screen.height / 2);
        obj.transform.LookAt(Camera.main.ScreenToWorldPoint(cameraPoint));*/

        float posY = obj.transform.position.y + 1f;
        obj.transform.DOMoveY(posY, 1f).OnComplete(() => { PoolManager.GetInstance().PushObj("DamageText", obj); });
    }

    public void TakeBuff(BuffType buffType)
    {
        //BuffManager.GetInstance().Buffs[buffType].OnAdd(gameObject);
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

        PoolManager.GetInstance().PushObj(characterType.ToString(), this.gameObject);    
    }
    public void Recovery()
    {
        if (!gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
        {
            PoolManager.GetInstance().PushObj(characterType.ToString(), this.gameObject);
        }
    }

    private void OnDisable()
    {
        if(characterTag != "Player") GameManager.GetInstance().EnemyDecrease(this.gameObject);
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
    public void Evolut(BuffType bulletEvolutionType)
    {
        foreach(var item in bulletBuff)
        {
            
        }
    }
}
