using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class NormalBullet : BulletBase
{
    GameObject targetObj;
    float rotateTimer;

    private void Awake()
    {
        bulletType = BulletType.NormalBullet;

        bulletAction += Move;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        targetObj = null;
    }

    private void Aim()
    {
        if (targetTag == CharacterTag.Player.ToString())
        {
            targetObj = Player._instance.gameObject;
        }
        else if (targetTag == CharacterTag.Enemy.ToString())
        {
            if (GameManager.GetInstance().enemyList.Count <= 0) return;
            targetObj = GameManager.GetInstance().enemyList[0];
        }
        transform.LookAt(targetObj.transform.position);
    }

    public override void Move()
    {
        if (!targetObj) Aim();
        gameObject.transform.Translate(Vector3.forward * bulletData.moveSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);

        Recovery();
    }
}
