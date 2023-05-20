using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using DG.Tweening;

public class LaserBullet : BulletBase
{
    List<GameObject> checkPoints;
    float attackTimer;

    private void Awake()
    {
        bulletType = BulletType.LaserBullet;
        checkPoints = FindChilds(transform.GetChild(0).gameObject);

        bulletAction += Rotate;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void Rotate()
    {
        /*float dot = Vector3.Dot(transform.forward, Player._instance.transform.forward);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        transform.Rotate(transform.up * angle * bulletData.rotateSpeed * Time.deltaTime);*/

        /*Vector3 vec = ( transform.position- Player._instance.transform.position);
        Quaternion rotate = Quaternion.LookRotation(vec);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, rotate, bulletData.rotateSpeed*Time.deltaTime);*/

        Vector3 direction = Player._instance.gameObject.transform.position - transform.position;
        direction.y = 0; // 只在水平面上旋转

        // 计算旋转角度
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 旋转物体
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * bulletData.rotateSpeed);
    }

    protected override void AttackCheck()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }
        for (int i = 0; i < checkPoints.Count - 1; i++)
        {
            Ray ray = new Ray(checkPoints[i].transform.position, checkPoints[i + 1].transform.position);
            float distence = (checkPoints[i + 1].transform.position - checkPoints[i].transform.position).magnitude;

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distence, enemyBulletMask))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
                if (targetIAttck == null) return;

                foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
                {
                    targetIAttck.TakeBuff(this.gameObject, gameObject,item.Key,item.Value);
                }

                if (isCrit)
                {
                    targetIAttck.TakeDamage(increaseATK * (1 + (float)bulletData.critRate / 100));
                    isCrit = false;
                }
                attackTimer = bulletData.damageInterval;
            }
        }
    }
}
