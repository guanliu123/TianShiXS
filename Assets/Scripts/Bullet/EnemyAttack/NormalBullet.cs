using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletBase
{
    private void Awake()
    {
        bulletType = BulletType.NormalBullet;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void AttackCheck()
    {
        float radius = 0.5f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, enemyBulletMask);
        if (hits.Length > 0)
        {
            targetIDamage = hits[0].gameObject.GetComponent<IDamage>();
            targetIDamage.TakeDamage(baseATK, hits[0].transform);

            RetrieveInstant();
        }
    }
}
