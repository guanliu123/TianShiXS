using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class Fanshaped : BulletBase
{
    private Dictionary<GameObject, float> unAttachable = new Dictionary<GameObject, float>();

    public void Awake()
    {
        //bulletID = BulletType.Fanshaped;
        bulletData = BulletManager.GetInstance().BulletDic[bulletID];


        bulletAction += AttckInterval;
        bulletAction += Move;
    }

    private void AttckInterval()
    {
        for(int i = 0; i < unAttachable.Count; i++)
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
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != targetTag || unAttachable.ContainsKey(other.gameObject)) return;
        AttackCheck(other.gameObject);
        unAttachable.Add(other.gameObject, bulletData.damageInterval);
    }

    public override void OnExit()
    {
        unAttachable.Clear();
    }

    /*protected override void AttackCheck()
    {
        Vector3 direction = transform.forward;
        float halfAngle = angle / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfAngle, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * direction;
        Vector3 rightRayDirection = rightRayRotation * direction;

        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (float)(rayCount - 1);
            Vector3 rayDirection = Vector3.Lerp(leftRayDirection, rightRayDirection, t);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, radius, ignoreObj))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
                if (targetIAttck == null || unAttachable.ContainsKey(targetIAttck)) continue;

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





















































