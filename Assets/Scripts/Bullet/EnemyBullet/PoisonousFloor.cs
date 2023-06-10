using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonousFloor : BulletBase
{
    float attackTimer;
    private void Awake()
    {
        bulletType = BulletType.PoisonousFloor;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
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
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, layerMask);
        if (hits.Length > 0)
        {
            IAttack targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            if (targetIAttck == null) return;
           
            foreach (var item in nowBuffs)
            {
                targetIAttck.TakeBuff(attacker,gameObject, item.Key, item.Value);
            }
            targetIAttck.ChangeHealth(attacker,-bulletData.ATK);

            attackTimer = bulletData.damageInterval;
        }
    }
}
