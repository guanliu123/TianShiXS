using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerpetualBullet : BulletBase
{
    int num;//连击次数

    private void Awake()
    {
        //bulletType = BulletType.PerpetualBullet;

        bulletData = BulletManager.GetInstance().BulletDic[bulletID];

    }

    public override void OnEnter()
    {
        num = 3;

        base.OnEnter();
        MonoManager.GetInstance().StartCoroutine(shoot());
    }

    public IEnumerator shoot()
    {
        if (attacker == null)
        {
            yield return null;
        }
        for (int i = 0; i < num; i++)
        {
            BulletManager.GetInstance().BulletLauncher(transform, -1, 0,attacker);
            yield return new WaitForSeconds(0.2f);
        }
    }

    protected override void SpecialEvolution()
    {
        base.SpecialEvolution();
        if (!BulletManager.GetInstance().haveSpecialEvolved[bulletID]) return;
        num += 2;
    }
}
