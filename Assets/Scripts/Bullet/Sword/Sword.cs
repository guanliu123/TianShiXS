using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : BulletBase
{
    /*private void Awake()
    {
        bulletType = BulletType.Sword;
    }*/

    List<GameObject> checkPoints;

    private void Awake()
    {
        bulletType = BulletType.Sword;
        checkPoints = FindChilds();
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
                targetIAttck.TakeDamage(increaseATK);

                RecoveryInstant();
            }
        }
    }

}
