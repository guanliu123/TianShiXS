using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletBase : MonoBehaviour
{
    public BulletData bulletData;
    public BulletType bulletType;

    public event UnityAction bulletAction;
    IDamage targetIDamage;

    private float baseATK;

    private float exitTimer = 0;
    private Vector3 moveDir;
    private float moveTimer=0f;
    private float rotateTimer = 0f;

    protected void Start()
    {
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
        if (bulletData.isMovable)
        {
            bulletAction += BaseMove;
            moveDir = gameObject.transform.forward;
        }
        if (bulletData.isRotatable) bulletAction += BaseRotate;
        bulletAction += Retrieve;
    }
    protected void OnEnable()
    {
        //Invoke(nameof(Retrieve), bulletData.existTime);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (bulletAction != null) bulletAction();
    }

    public void SetInfo(float ATK)
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
        PoolManager.GetInstance().PushObj("Bullet", this.gameObject);
    }
}
