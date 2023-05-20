using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousFloor : BulletBase
{
    float attackTimer;
    private void Awake()
    {
        bulletType = BulletType.PoisonousFloor;

        bulletAction += Move;
    }
    protected override void AttackCheck()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }
        float radius = 2f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, enemyBulletMask);
        if (hits.Length > 0)
        {
            IAttack targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            if (targetIAttck == null) return;
           
            foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
            {
                targetIAttck.TakeBuff(this.gameObject,gameObject, item.Key, item.Value);
            }
            targetIAttck.TakeDamage(increaseATK);

            attackTimer = bulletData.damageInterval;
        }
    }
}
