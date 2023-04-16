using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreSword : BulletBase
{
    private float AttackDecline = 0.2f;
    List<GameObject> checkPoints;

    private void Awake()
    {
        bulletType = BulletType.MoreSword;
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
                targetIDamage = hit.collider.gameObject.GetComponent<IDamage>();
                targetIDamage.TakeDamage(baseATK * (1 - AttackDecline),hit.transform);

                RetrieveInstant();
            }
        }
    }
}
