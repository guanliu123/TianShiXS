using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : BaseManager<BulletManager>
{
    public void ShootBullet(Transform parent,BulletType bulletType,float ATK)
    {
        switch (bulletType)
        {
            case BulletType.BaseSword:ShootBaseSword(parent, bulletType, ATK); break;
            case BulletType.AttackSword:ShootAttackSword(parent, bulletType, ATK);break;
            case BulletType.MoreSword:ShootMoreSword(parent, bulletType, ATK);break;
            case BulletType.NormalBullet:ShootNormalBullet(parent, bulletType, ATK);break;
        }
    }

    #region 剑气的发射
    private void ShootBaseSword(Transform parent,BulletType bulletType,float atk)
    {
        GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());
        t.transform.position = new Vector3(parent.position.x, parent.position.y + 1.2f, parent.position.z);

        IInitBullet initBullet = t.GetComponent<IInitBullet>();
        initBullet.InitInfo(atk);
    }

    private void ShootAttackSword(Transform parent, BulletType bulletType, float atk)
    {
        GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());
        t.transform.position = new Vector3(parent.position.x, parent.position.y + 1.2f, parent.position.z);

        IInitBullet initBullet = t.GetComponent<IInitBullet>();
        initBullet.InitInfo(atk);
    }

    private void ShootMoreSword(Transform parent,BulletType bulletType,float atk)
    {
        int n = 2;

        for(int i = 0; i < n; i++)
        {
            GameObject t= PoolManager.GetInstance().GetObj(bulletType.ToString());
            t.transform.position= new Vector3(parent.position.x+(i+1.5f-n), parent.position.y + 1.2f, parent.position.z);
            
        }
    }

    #endregion
    private void ShootNormalBullet(Transform parent, BulletType bulletType, float atk)
    {
        GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());
        t.transform.position = parent.transform.position;
        t.transform.rotation = parent.transform.rotation;

        IInitBullet initBullet = t.GetComponent<IInitBullet>();
        initBullet.InitInfo(atk);
    }
}
