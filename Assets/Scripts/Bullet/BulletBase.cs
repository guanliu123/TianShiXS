using Bat;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IBulletEvent
{
    void InitBullet(float ATK,GameObject _shooter);
    //void BulletEvolute(BulletEvolutionType evolutionType,List<BuffType> bulletTypes);
}

public class BulletBase : MonoBehaviour, IBulletEvent
{
    public BulletData bulletData;
    public BulletType bulletType;
    public GameObject shooter;

    //public List<BuffType> nowBuffs=new List<BuffType>();

    public UnityAction bulletAction;

    protected LayerMask playerBulletMask;
    protected LayerMask enemyBulletMask;

    protected float bulletATK;
    private float existTimer = 0;

    public bool isCrit;

    protected void Start()
    {
        //bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        playerBulletMask = LayerMask.GetMask("Enemy");//玩家发射出去的子弹的layermask
        enemyBulletMask = LayerMask.GetMask("Player");//敌人发射出去的子弹的layermask

        bulletAction += Recovery;
        bulletAction += AttackCheck;
    }

    public virtual void InitBullet(float ATK,GameObject _shooter)
    {
        bulletATK = bulletData.baseATK + ATK + BulletManager.GetInstance().increaseATK[bulletType];
        shooter = _shooter;
    }

    protected void OnEnable()
    {
        OnEnter();
    }
    protected void Update()
    {
        if (bulletAction != null) bulletAction();
    }

    public virtual void OnEnter()
    {
        if (GameManager.GetInstance().enemyList.Count <= 0) RecoveryInstant();
        SpecialEvolution();
    }
    public virtual void OnExit()
    {

    }

    public virtual void Move()
    {
        gameObject.transform.Translate(transform.forward * bulletData.moveSpeed * Time.deltaTime);
    }
    public virtual void Rotat()
    {

    }

    //如果有类似Sword这样需要获取子物体所有点位以便发射射线进行检测的子弹类型，调用这个方法
    protected List<GameObject> FindChilds(GameObject obj)
    {
        List<GameObject> t = new List<GameObject>();
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            t.Add(obj.transform.GetChild(i).gameObject);
        }

        return t;
    }

    protected virtual void AttackCheck()
    {

    }
    protected virtual void SpecialEvolution()
    {
        
    }

    protected void Recovery()
    {
        existTimer += Time.deltaTime;
        if (existTimer < bulletData.existTime + BulletManager.GetInstance().increaseTime[bulletType]) return;

        existTimer = 0;
        OnExit();
        PoolManager.GetInstance().PushObj(bulletType.ToString(), gameObject);
    }
    protected void RecoveryInstant()
    {
        existTimer = 0;
        OnExit();
        PoolManager.GetInstance().PushObj(bulletType.ToString(), this.gameObject);
    }
}