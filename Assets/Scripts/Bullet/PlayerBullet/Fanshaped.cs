using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Fanshaped : BulletBase
{
    private Dictionary<IAttack, float> unAttachable = new Dictionary<IAttack, float>();

    private float radius = 6f; // 射线检测半径
    private float angle = 100f; // 射线检测角度
    private int rayCount = 50; // 射线数量

    public void Awake()
    {
        bulletType = BulletType.Fanshaped;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += AttckInterval;
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

    public override void OnExit()
    {
        unAttachable.Clear();
    }

    protected override void AttackCheck()
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
            if (Physics.Raycast(transform.position, rayDirection, out hit, radius, playerBulletMask))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
                if (targetIAttck == null|| unAttachable.ContainsKey(targetIAttck)) continue;

                foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
                {
                    targetIAttck.TakeBuff(shooter, gameObject, item.Key, item.Value);
                }
                if (isCrit)
                {
                    targetIAttck.ChangeHealth(shooter, -bulletATK * 
                        (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                    isCrit = false;
                }
                else { targetIAttck.ChangeHealth(shooter, -bulletATK); }

                unAttachable.Add(targetIAttck,bulletData.damageInterval);
            }
        }
    }

    void OnDrawGizmos()
    {
        // 绘制扇形区域形状
        Vector3 direction = transform.forward;
        float halfAngle = angle / 2f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfAngle, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfAngle, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * direction;
        Vector3 rightRayDirection = rightRayRotation * direction;
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * radius);
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (float)(rayCount - 1);
            Vector3 rayDirection = Vector3.Lerp(leftRayDirection, rightRayDirection, t);
            Gizmos.DrawLine(transform.position, transform.position + rayDirection * radius);
        }
    }
}





















































