using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : BaseManager<BulletManager>
{
    //存储每种子弹目前拥有的附加效果类型和进化次数
    public Dictionary<BulletType, BulletData> BulletDic = new Dictionary<BulletType, BulletData>();
    //存每种子弹先挂载效果的层数
    public Dictionary<BulletType, Dictionary<BuffType,int>> BulletBuffs = new Dictionary<BulletType, Dictionary<BuffType, int>>();
    /*public Dictionary<BulletType, Dictionary<BuffType, int>> EvolvableBuffs = new Dictionary<BulletType, Dictionary<BuffType, int>>();*/


    public BulletManager()
    {
        BulletDic = DataManager.GetInstance().bulletDatasDic;
        
        foreach(var item in BulletDic)
        {
            BulletBuffs.Add(item.Key, new Dictionary<BuffType, int>());
            foreach(var t in item.Value.buffList)
            {
                BulletBuffs[item.Key].Add(t, 1);
            }
        }
    }

    public void BulletLauncher(Transform shooter,BulletType bulletType, float aggressivity)
    {
        

        if (BulletDic[bulletType].isRandomShoot)
        {
            RandomLauncher(shooter, bulletType, aggressivity);
        }
        else
        {
            StraightLauncher(shooter,bulletType, aggressivity);
        }

             
    }
    private void RandomLauncher(Transform shooter, BulletType bulletType, float aggressivity)
    {
        int n;
        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, shooter.transform.position.y + 1f, shooter.transform.position.z);
        if (!BulletBuffs[bulletType].ContainsKey(BuffType.Multiply)) n = 2;
        else
        {
            n = BulletBuffs[bulletType][BuffType.Multiply];
            n = n < 1 ? 1 : n;
        }
        for (int i = 0; i < n; i++)
        {
            
            GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());

            t.transform.position = instantPos;
            t.transform.rotation = Quaternion.Euler(shooter.transform.rotation.eulerAngles +
                 new Vector3(0, UnityEngine.Random.Range(-60, 60), 0));
            
            t.GetComponent<IBulletEvent>().InitATK(aggressivity);
        }
    }
    private void StraightLauncher(Transform shooter, BulletType bulletType, float aggressivity)
    {
        int n;
        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, shooter.transform.position.y + 1f, shooter.transform.position.z);
        if (!BulletBuffs[bulletType].ContainsKey(BuffType.Multiply)) n = 1;
        else
        {
            n = BulletBuffs[bulletType][BuffType.Multiply];
            n = n < 1 ? 1 : n;
        }
        GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());

        t.transform.position = instantPos;
        t.GetComponent<IBulletEvent>().InitATK(aggressivity);
    }

    public void BulletEvolute(BuffType evolutionType, BulletType bulletType)
    {
        if (!BulletDic[bulletType].evolvableList.Contains(evolutionType)) return;

        /*if (BulletBuffs[bulletType].ContainsKey(evolutionType))
        {
            BulletBuffs[bulletType][evolutionType]++;
        }
        else
        {
            BulletBuffs[bulletType].Add(evolutionType, 1);
        }*/
    }
}
