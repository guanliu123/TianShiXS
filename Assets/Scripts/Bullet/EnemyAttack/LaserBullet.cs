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
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
        checkPoints = FindChilds(transform.GetChild(0).gameObject);

        bulletAction += Rotate;
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

    public override void Rotate()
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
            Ray ray = new Ray(checkPoints[i].transform.position, checkPoints[i + 1].transform.position);
            float distence = (checkPoints[i + 1].transform.position - checkPoints[i].transform.position).magnitude;

            RaycastHit hit;
     
            if (Physics.Raycast(ray, out hit, distence))
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
                else {targetIAttck.TakeDamage(increaseATK); }

                attackTimer = bulletData.damageInterval;
            }
        }
    }
}
