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
        bulletType = BulletType.TaiChiDart;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += Move;
        bulletAction += this.AttckInterval;
    }

    public override void OnEnter()
    {
        bounceNum = 2;
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
        if (!BulletManager.GetInstance().haveSpecialEvolved[bulletType]) return;

        bounceNum += 3;
        bulletData.ATK += 5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            moveDir = Vector3.Reflect(Vector3.forward, other.transform.position.normalized);
            moveDir = moveDir.normalized;
        }
    }

    protected override void AttackCheck()
    {
        float radius = 1f;
        Collider[] hits = Physics.OverlapSphere(this.transform.position, radius, layerMask);
        if (hits.Length > 0)
        {
            for(int i = 0; i < hits.Length; i++)
            {
                IAttack targetIAttck = hits[i].gameObject.GetComponentInParent<IAttack>();
                if (targetIAttck == null || unAttachable.ContainsKey(targetIAttck)) return;

                foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
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
    }
}
