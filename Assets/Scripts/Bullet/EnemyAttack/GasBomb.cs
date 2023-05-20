using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : BulletBase
{
    private void Awake()
    {
        bulletType = BulletType.GasBomb;

        bulletAction += Move;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();

    }

    public override void Move()
    {
        gameObject.transform.LookAt(Player._instance.gameObject.transform);
        gameObject.transform.Translate(-transform.forward * bulletData.moveSpeed * Time.deltaTime);
        gameObject.transform.Translate(-transform.up * 1f * Time.deltaTime);
        if (transform.position.y <= 0)
        {
            BulletManager.GetInstance().BulletLauncher(gameObject.transform, BulletType.PoisonousFloor, 0);
            RecoveryInstant();
        }
    }
}
