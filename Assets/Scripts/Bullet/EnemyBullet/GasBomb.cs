using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasBomb : BulletBase
{
    public void Awake()
    {
        bulletType = BulletType.GasBomb;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
        bulletAction += Move;

    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    public override void OnEnter()
    {
        gameObject.transform.LookAt(Player._instance.gameObject.transform);
    }

    public override void Move()
    {
        //gameObject.transform.LookAt(Player._instance.gameObject.transform);
        gameObject.transform.Translate(-transform.forward * bulletData.moveSpeed * Time.deltaTime);
        gameObject.transform.Translate(-transform.up * 0.9f * Time.deltaTime);
        if (transform.position.y <= 0)
        {
            BulletManager.GetInstance().BulletLauncher(gameObject.transform, BulletType.PoisonousFloor, 0,attacker);
            RecoveryInstant();
        }
    }
}
