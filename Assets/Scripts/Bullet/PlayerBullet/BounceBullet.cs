using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : BulletBase
{
    int bounceNum;
    Vector3 moveDir;

    private void Awake()
    {
        bulletType = BulletType.TaiChiDart;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += Move;
    }

    public override void OnEnter()
    {
        bounceNum = 1;
        moveDir = transform.forward;

        base.OnEnter();
    }

    public override void Move()
    {
        gameObject.transform.Translate(moveDir * bulletData.moveSpeed * Time.deltaTime);
    }

    protected override void SpecialEvolution()
    {
        base.SpecialEvolution();
        bounceNum += 2;
        bulletATK += 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        moveDir = transform.forward + (other.transform.position -transform.position);
    }
}
