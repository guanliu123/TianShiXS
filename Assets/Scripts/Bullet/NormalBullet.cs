using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBullet : BulletBase
{
    private void Awake()
    {
        bulletType = BulletType.NormalBullet;

        bulletAction += Move;
    }

    public override void Move()
    {
        gameObject.transform.Translate(-transform.forward * bulletData.moveSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);

        Recovery();
    }
    /*private void Awake()
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
            targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            targetIAttck.TakeDamage(increaseATK);

            //RetrieveInstant();
        }
    }*/
}
