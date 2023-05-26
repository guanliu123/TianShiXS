using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBullet : BulletBase
{
    float divisionTimer;
    private void Awake()
    {
        bulletType = BulletType.TaiChiDart;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += Move;
        bulletAction += Division;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public override void OnEnter()
    {
        divisionTimer = 9999f;
        base.OnEnter();
    }
    protected override void SpecialEvolution()
    {
        if (!BulletManager.GetInstance().haveSpecialEvolved[bulletType]) return;
        divisionTimer = 0.3f;
    }

    public void Division()
    {
        if (divisionTimer < 999)
        {
            divisionTimer -= Time.deltaTime;
            if (divisionTimer <= 0)
            {
                BulletManager.GetInstance().BulletLauncher(transform, BulletType.TrackingBullet, 0);
                divisionTimer = 0.3f;
            }
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
                targetIAttck.TakeBuff(this.gameObject, gameObject, item.Key, item.Value);
            }
            if (isCrit)
            {
                targetIAttck.ChangeHealth(-bulletATK *
                    (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                isCrit = false;
            }
            else { targetIAttck.ChangeHealth(-bulletATK); }

            RecoveryInstant();
        }
    }
}
