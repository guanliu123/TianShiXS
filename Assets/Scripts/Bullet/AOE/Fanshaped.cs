using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Fanshaped : BulletBase
{
    private Dictionary<IAttack, float> unAttachable = new Dictionary<IAttack, float>();

    public float radius = 7f; // 射线检测半径
    public float angle = 120f; // 射线检测角度
    public int rayCount = 50; // 射线数量

    public void Awake()
    {
        bulletType = BulletType.Fanshaped;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
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
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawLine(transform.position, transform.position + leftRayDirection * radius);
        Gizmos.DrawLine(transform.position, transform.position + rightRayDirection * radius);
        for (int i = 0; i < rayCount; i++)
        {
            float t = (float)i / (float)(rayCount - 1);
            Vector3 rayDirection = Vector3.Lerp(leftRayDirection, rightRayDirection, t);
            Gizmos.DrawLine(transform.position, transform.position + rayDirection * radius);
        }
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
                if (targetIAttck == null) return;

                foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
                {
                    targetIAttck.TakeBuff(Player._instance.gameObject, gameObject, item.Key, item.Value);
                }
                if (isCrit)
                {
                    targetIAttck.TakeDamage(increaseATK * (1 + (float)bulletData.critRate / 100));
                    isCrit = false;
                }
                else { targetIAttck.TakeDamage(increaseATK); }
            }
        }
    }
}





















































