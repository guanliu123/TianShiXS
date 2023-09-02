using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : BulletBase
{
    private void Awake()
    {
        //bulletID = BulletType.TrackingBullet;
        bulletID = 3005;
        bulletData = BulletManager.GetInstance().BulletDic[bulletID];

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);

        Recovery();
    }
}
