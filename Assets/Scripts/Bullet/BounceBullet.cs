using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BounceBullet : BulletBase
{
    private Dictionary<IAttack, float> unAttachable = new Dictionary<IAttack, float>();

    int bounceNum;
    Vector3 moveDir;

    private void Awake()
    {
        //bulletID = BulletType.BounceBullet;
        bulletData = BulletManager.GetInstance().BulletDic[bulletID];


        bulletAction += Move;
        bulletAction += this.AttckInterval;
    }

    public override void OnEnter()
    {
        bounceNum = 3;
        moveDir = transform.forward;

        base.OnEnter();
    }
    private void AttckInterval()
    {
        for (int i = 0; i < unAttachable.Count; i++)
        {
            float t = unAttachable.ElementAt(i).Value - Time.deltaTime;

            if (t <= 0)
            {
                unAttachable.Remove(unAttachable.ElementAt(i).Key);
                continue;
            }

            unAttachable[unAttachable.ElementAt(i).Key] = t;
        }
    }

    public override void Move()
    {
        gameObject.transform.Translate(moveDir * bulletData.moveSpeed * Time.deltaTime);
    }

    protected override void SpecialEvolution()
    {
        base.SpecialEvolution();
        if (!BulletManager.GetInstance().haveSpecialEvolved[bulletID]) return;

        bounceNum += 2;
        bulletData.ATK += 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bounceNum <= 0) Recovery();
        if (other.tag != ignoreTag)
        {
            moveDir = Vector3.Reflect(Vector3.forward, other.transform.position.normalized);
            moveDir = moveDir.normalized;
            if (other.tag == targetTag)
            {
                AttackCheck(other.gameObject);
            }
            bounceNum--;
        }
    }

    /*protected override void AttackCheck()
    {
        float radius = 1f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, ignoreObj);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                IAttack targetIAttck = hits[i].gameObject.GetComponentInParent<IAttack>();
                if (targetIAttck == null || unAttachable.ContainsKey(targetIAttck)) return;

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

                unAttachable.Add(targetIAttck, bulletData.damageInterval);
            }

        }
    }*/
}
