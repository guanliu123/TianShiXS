using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaiChiDart : BulletBase
{
    List<GameObject> checkPoints;

    private void Awake()
    {
        bulletType = BulletType.TaiChiDart;
        bulletData = DataManager.GetInstance().AskBulletData(bulletType);
        checkPoints = FindChilds(this.gameObject);

        bulletAction += Move;
    }
    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override void AttackCheck()
    {
        for (int i = 0; i < checkPoints.Count - 1; i++)
        {
            Ray ray = new Ray(checkPoints[i].transform.position, checkPoints[i + 1].transform.position);
            float distence = (checkPoints[i + 1].transform.position - checkPoints[i].transform.position).magnitude;

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distence, playerBulletMask))
            {
                IAttack targetIAttck = hit.collider.gameObject.GetComponent<IAttack>();
                if (targetIAttck==null) return;
              
                foreach (var item in BulletManager.GetInstance().BulletBuffs[bulletType])
                {
                    targetIAttck.TakeBuff(Player._instance.gameObject,gameObject, item.Key, item.Value);
                }
                if (isCrit)
                {
                    targetIAttck.ChangeHealth(-bulletATK*
                        (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
                    isCrit = false;
                }
                else {targetIAttck.ChangeHealth(-bulletATK); }

                RecoveryInstant();
            }
        }
    }

}
