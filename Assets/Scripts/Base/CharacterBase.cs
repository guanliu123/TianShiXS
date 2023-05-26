using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.Events;
//using static UnityEditor.PlayerSettings;

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
    public float aggressivity;//�������ӳ�
    public float ATKSpeed;//���ټӳ�


    public Dictionary<BuffType, (int,float)> buffDic=new Dictionary<BuffType, (int, float)>();//�����ŵ���ÿ��buff�����Ӧ�Ĳ��������ʱ��
    public Dictionary<BulletType, float> nowBullet = new Dictionary<BulletType, float>();//��Ž�ɫ�Ĺ�����ʽ����Ļ����,ֵ��ÿ���ӵ��Ĺ��������
    //public Dictionary<BulletType, List<BuffType>> bulletBuff = new Dictionary<BulletType, List<BuffType>>();
    public Dictionary<BulletType, float> bulletTimer = new Dictionary<BulletType, float>();

    public Animator animator;
    public HPSlider hpSlider;

    public bool canActive;
    protected event UnityAction characterActiveEvent;//��ɫ���������е���Ϊ�����繥�����ƶ�
    protected event UnityAction characterPassiveEvent;//��ɫ����ִ�е���Ϊ�������ϵ�buff����ʱ���Ƿ񱻻���

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

    public void AddBullet(BulletType bulletType)
    {
        if (nowBullet.ContainsKey(bulletType)) return;
        nowBullet.Add(bulletType, BulletManager.GetInstance().BulletDic[BulletType.RotateBullet].damageInterval);
        bulletTimer.Add(bulletType, BulletManager.GetInstance().BulletDic[BulletType.RotateBullet].damageInterval);
    }

    protected void InitData()
    {
        characterData = DataManager.GetInstance().AskCharacterData(characterType);
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
            (int, float) t = (piles, 0f);
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
        foreach(var item in nowBullet)
        {
            if (item.Key == BulletType.Combat)//��ս����
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
                    bulletTimer[item.Key] -= (1 + ATKSpeed + BulletManager.GetInstance().increaseShoot[item.Key]) * Time.deltaTime;
                }
                continue;
            }
            bulletTimer[item.Key] -= (1+ATKSpeed + BulletManager.GetInstance().increaseShoot[item.Key]) *Time.deltaTime;
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
            GameManager.GetInstance().FallMoney(transform, characterData.money);
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
        if(characterTag!="Player") PoolManager.GetInstance().PushObj(characterType.ToString(), this.gameObject);
    } 
}
