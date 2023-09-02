using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BulletBase
{
    public void Awake()
    {
        bulletID = 3008;
        bulletData = BulletManager.GetInstance().BulletDic[bulletID];

        bulletAction += Move;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        AttackCheck(other.gameObject);
        Recovery();
    }

    /*protected override void AttackCheck()
    {
        float radius = 1f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, ignoreObj);
        if (hits.Length > 0)
        {
            IAttack targetIAttck = hits[0].gameObject.GetComponentInParent<IAttack>();
            if (targetIAttck == null) return;

            foreach (var item in nowBuffs)
            {
                targetIAttck.TakeBuff(attacker, gameObject, item.Key, item.Value);
            }
            if (isCrit)
            {
                targetIAttck.ChangeHealth(attacker, -bulletData.ATK *
                    (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                isCrit = false;
            }
            else { targetIAttck.ChangeHealth(attacker, -bulletData.ATK); }

            Recovery();
        }
    }*/
}
