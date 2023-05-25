using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualBullet : BulletBase
{
    int num;//连击次数

    private void Awake()
    {
        bulletType = BulletType.PerpetualBullet;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);     
    }

    public override void OnEnter()
    {
        num = 3;

        base.OnEnter();
        MonoManager.GetInstance().StartCoroutine(shoot());
    }

    public IEnumerator shoot()
    {
        for (int i = 0; i < num; i++)
        {
            BulletManager.GetInstance().BulletLauncher(transform, BulletType.TaiChiDart, 0);
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected override void SpecialEvolution()
    {
        if (!BulletManager.GetInstance().haveSpecialEvolved[bulletType]) return;
        num += 2;
    }
}
