using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrackingBullet : BulletBase
{
    private void Awake()
    {
        bulletType = BulletType.TrackingBullet;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += Move;
    }

    public override void Move()
    {
        if (GameManager.GetInstance().enemyList.Count <= 0)
        {
            base.Move();
        }
        else
        {
            Vector3 target = GameManager.GetInstance().enemyList[0].transform.position;
            transform.forward = Vector3.Slerp(transform.forward, target - transform.position, 0.5f / Vector3.Distance(transform.position, target));
            transform.position += transform.forward * bulletData.moveSpeed * Time.deltaTime;
        }
    }

    protected override void AttackCheck()
    {
        float radius = 0.5f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, playerBulletMask);
        if (hits.Length > 0)
        {
            IAttack targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            if (targetIAttck == null) return;

            foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
            {
                targetIAttck.TakeBuff(shooter, gameObject, item.Key, item.Value);
            }
            if (isCrit)
            {
                targetIAttck.ChangeHealth(shooter,-bulletATK *
                    (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                isCrit = false;
            }
            else { targetIAttck.ChangeHealth(shooter,-bulletATK); }

            RecoveryInstant();
        }
    }
}
/*    public BulletData bulletData;
    public BulletType bulletType;

    public event UnityAction bulletAction;

    protected LayerMask playerBulletMask;
    protected LayerMask enemyBulletMask;

    protected IDamage targetIDamage;

    protected float baseATK;
    private float exitTimer = 0;
    private Vector3 moveDir;
    private float moveTimer=0f;
    private float rotateTimer = 0f;

    protected void Start()
    {
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        playerBulletMask = LayerMask.GetMask("Enemy");//玩家发射出去的子弹的layermask
        enemyBulletMask = LayerMask.GetMask("Player");//敌人发射出去的子弹的layermask

        if (bulletData.isMovable) bulletAction += BaseMove;
        if (bulletData.isRotatable) bulletAction += BaseRotate;

        bulletAction += AttackCheck;
        bulletAction += Retrieve;
    }

    protected void OnEnable()
    {
        
        moveDir = Vector3.forward;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (bulletAction != null) bulletAction();
    }

    public void InitInfo(float ATK)
    {
        baseATK = ATK;
    }

    void BaseMove()
    {
        if (bulletData.stopTime > 0)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > bulletData.moveTime + bulletData.stopTime) moveTimer = 0;
            else if (moveTimer > bulletData.moveTime) return;
        }
        gameObject.transform.Translate(moveDir * bulletData.moveSpeed * Time.deltaTime);
    }
    void BaseRotate()
    {
        rotateTimer += Time.deltaTime;
        if (rotateTimer > bulletData.moveTime)
        {
            gameObject.transform.Rotate(Vector3.up * bulletData.rotateSpeed * Time.deltaTime);
            rotateTimer = 0;
        }
    }

    protected virtual void Retrieve()
    {
        exitTimer += Time.deltaTime;
        if (exitTimer < bulletData.existTime) return;

        exitTimer = 0;
        PoolManager.GetInstance().PushObj(bulletData.bulletType.ToString(), this.gameObject);
    }
    protected virtual void RetrieveInstant()
    {
        exitTimer = 0;
        PoolManager.GetInstance().PushObj(bulletData.bulletType.ToString(), this.gameObject);
    }

    protected virtual void AttackCheck()
    {

    }

    public virtual void BulletEvolution(EvolutionType evolutionType)
    {

    }

    //如果有类似Sword这样需要获取子物体所有点位以便发射射线进行检测的子弹类型，调用这个方法
    protected List<GameObject> FindChilds()
    {
        List<GameObject> t=new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            t.Add(gameObject.transform.GetChild(i).gameObject);
        }

        return t;
    }*/