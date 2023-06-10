using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using DG.Tweening;
using static UnityEngine.UI.Image;
using System.Drawing;

public class LaserBullet : BulletBase
{
    List<GameObject> checkPoints;
    float attackTimer;

    private void Awake()
    {
        bulletType = BulletType.LaserBullet;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
        checkPoints = FindChilds(transform.GetChild(0).gameObject);

        bulletAction += Rotat;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void OnEnter()
    {
        attackTimer = 0f;
    }

    public override void Rotat()
    {
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
            Ray ray = new Ray(checkPoints[i].transform.position, checkPoints[i + 1].transform.position - checkPoints[i].transform.position);
            float distance = (checkPoints[i + 1].transform.position - checkPoints[i].transform.position).magnitude;
            RaycastHit hit;
            Debug.DrawRay(checkPoints[i].transform.position, checkPoints[i + 1].transform.position - checkPoints[i].transform.position * distance);

            if (Physics.Raycast(ray, out hit, distance, layerMask))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
                if (targetIAttck == null) return;
                
                foreach (var item in nowBuffs)
                {
                    targetIAttck.TakeBuff(attacker, gameObject,item.Key,item.Value);
                }

                if (isCrit)
                {
                    targetIAttck.ChangeHealth(attacker, -bulletData.ATK *
                        (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                    isCrit = false;
                }
                else {targetIAttck.ChangeHealth(attacker, -bulletData.ATK); }

                attackTimer = bulletData.damageInterval;
            }
        }
    }
}
