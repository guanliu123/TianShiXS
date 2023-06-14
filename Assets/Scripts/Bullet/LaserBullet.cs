using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;
using DG.Tweening;
using static UnityEngine.UI.Image;
using System.Drawing;
using System.Linq;

public class LaserBullet : BulletBase
{
    private Dictionary<GameObject, float> unAttachable = new Dictionary<GameObject, float>();
    GameObject targetObj;
    float attackTimer;

    private void Awake()
    {
        bulletType = BulletType.LaserBullet;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);

        bulletAction += Rotat;
        bulletAction += AttckInterval;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (targetTag == CharacterTag.Player.ToString())
        {
            targetObj = Player._instance.gameObject;
        }
        else if (targetTag == CharacterTag.Enemy.ToString())
        {
            if (GameManager.GetInstance().enemyList.Count <= 0) return;
            targetObj = GameManager.GetInstance().enemyList[0];
        }
    }

    public override void OnEnter()
    {
        attackTimer = 0f;
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

    public override void Rotat()
    {
        if (targetObj == null) return;
        Vector3 direction = targetObj.transform.position - transform.position;
        direction.y = 0; // 只在水平面上旋转

        // 计算旋转角度
        Quaternion rotation = Quaternion.LookRotation(direction);

        // 旋转物体
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * bulletData.rotateSpeed);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != targetTag||unAttachable.ContainsKey(other.gameObject)) return;
        AttackCheck(other.gameObject);
        unAttachable.Add(other.gameObject, bulletData.damageInterval);
    }

    /*protected override void AttackCheck()
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

            if (Physics.Raycast(ray, out hit, distance, ignoreObj))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
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

                attackTimer = bulletData.damageInterval;
            }
        }
    }*/
}
