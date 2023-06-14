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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);

        Recovery();
    }

    /*protected override void AttackCheck()
    {
        float radius = 0.5f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, ignoreObj);
        if (hits.Length > 0)
        {
            IAttack targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            if (targetIAttck == null) return;

            foreach (var item in nowBuffs)
            {
                targetIAttck.TakeBuff(attacker, gameObject, item.Key, item.Value);
            }
            if (isCrit)
            {
                targetIAttck.ChangeHealth(attacker, -bulletData.ATK *
                    (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                isCrit = false;
            }
            else { targetIAttck.ChangeHealth(attacker, -bulletData.ATK); }

            Recovery();
        }
    }*/
}