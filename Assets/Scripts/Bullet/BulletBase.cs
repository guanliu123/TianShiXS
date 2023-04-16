using Bat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInitBullet
{
    void InitInfo(float ATK);
}

public class BulletBase : MonoBehaviour, IInitBullet
{
    public BulletData bulletData;
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

        playerBulletMask = LayerMask.GetMask("Enemy");//��ҷ����ȥ���ӵ���layermask
        enemyBulletMask = LayerMask.GetMask("Player");//���˷����ȥ���ӵ���layermask

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
        PoolManager.GetInstance().PushObj(bulletData.bulletName, this.gameObject);
    }
    protected virtual void RetrieveInstant()
    {
        exitTimer = 0;
        PoolManager.GetInstance().PushObj(bulletData.bulletName, this.gameObject);
    }

    protected virtual void AttackCheck()
    {

    }

    public virtual void BulletEvolution(EvolutionType evolutionType)
    {

    }

    //���������Sword������Ҫ��ȡ���������е�λ�Ա㷢�����߽��м����ӵ����ͣ������������
    protected List<GameObject> FindChilds()
    {
        List<GameObject> t=new List<GameObject>();
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            t.Add(gameObject.transform.GetChild(i).gameObject);
        }

        return t;
    }
    protected virtual void HitEvent()
    {

    }
}
