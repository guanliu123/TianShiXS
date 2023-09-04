using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBullet : BulletBase
{
    GameObject targetObj;

    private void Awake()
    {
        //bulletID = BulletType.NormalBullet;
        bulletAction += Move;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        targetObj = null;
    }

    public override void Move()
    {
        gameObject.transform.Translate(Vector3.forward * bulletData.moveSpeed * Time.deltaTime);
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);

        Recovery();
    }
}
