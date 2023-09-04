using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public struct ShootData
{
    public Transform shooter;
    public GameObject attacker;
    public CharacterTag attackerTag;
    public string objName;
    public int n;
    public BulletData initData;
    public Dictionary<int, int> initBuffs;
    public bool isFollowShooter;
}

public class BulletManager : SingletonBase<BulletManager>
{
    //存储每种子弹数据
    public Dictionary<int, BulletData> BulletDic = new Dictionary<int, BulletData>();
    public Dictionary<int, List<int>> evolvableBuffList = new Dictionary<int, List<int>>();
    //存每种子弹先挂载效果的层数
    public Dictionary<int, Dictionary<int, int>> BulletBuffs = new Dictionary<int, Dictionary<int, int>>();

    //子弹进化数据
    public Dictionary<int, Dictionary<int, int>> increaseBuffs = new Dictionary<int, Dictionary<int, int>>();
    public Dictionary<int, float> increaseProbability = new Dictionary<int, float>();
    public Dictionary<int, float> increaseATK = new Dictionary<int, float>();
    public Dictionary<int, float> increaseShootTimer = new Dictionary<int, float>();
    public Dictionary<int, float> increaseExistTime = new Dictionary<int, float>();
    public Dictionary<int, bool> haveSpecialEvolved = new Dictionary<int, bool>();

    private List<GameObject> bulletList = new List<GameObject>();

    public BulletManager()
    {
        Debug.Log("Begin BulletManager Constructor!");

        BulletDic = BulletDataTool.ReadBulletData();

        Init();

        Debug.Log("End BulletManager Constructor!");
    }
    private void Init()
    {
        foreach (var item in BulletDic)
        {
            if (!BulletBuffs.ContainsKey(item.Key))
            {
                BulletBuffs.Add(item.Key, new Dictionary<int, int>());
                foreach (var t in item.Value.buffList)
                {
                    BulletBuffs[item.Key].Add(t, 1);
                }
            }
            if (!evolvableBuffList.ContainsKey(item.Key)) evolvableBuffList.Add(item.Key, item.Value.evolvableList);
            increaseBuffs.Add(item.Key, new Dictionary<int, int>());
            increaseProbability.Add(item.Key, 0);
            increaseATK.Add(item.Key, 0);
            increaseShootTimer.Add(item.Key, 0);
            increaseExistTime.Add(item.Key, 0);
            haveSpecialEvolved.Add(item.Key, false);
        }
    }
    public override void Reset()
    {
        increaseBuffs.Clear();
        increaseProbability.Clear();
        increaseExistTime.Clear();
        increaseATK.Clear();
        increaseShootTimer.Clear();
        haveSpecialEvolved.Clear();

        ClearExistBullet();

        Init();
    }

    public void ClearExistBullet()
    {
        for (int i = 0; i < bulletList.Count; i++)
        {
            if (bulletList[i] != null) GameObject.Destroy(bulletList[i]);
        }
        bulletList.Clear();
    }

    //b是用于一些载体子弹（比如连击弹幕）用的
    public void BulletLauncher(Transform shooter, int bulletID, float aggressivity, GameObject attacker, int variantCode = -1)
    {
        ShootData shootData = new ShootData();
        shootData.initBuffs = new Dictionary<int, int>();
        shootData.initData = BulletDic[bulletID];

        shootData.shooter = shooter;
        shootData.attacker = attacker;

        //针对玩家发出的弹幕进行特化
        var character = attacker.GetComponent<CharacterBase>();
        if (character)
        {
            shootData.attackerTag = character.characterTag;
            if (shootData.attackerTag == CharacterTag.Player)
            {
                foreach (var item in increaseBuffs[bulletID])
                {
                    if (shootData.initBuffs.ContainsKey(item.Key)) shootData.initBuffs[item.Key] += item.Value;
                    else shootData.initBuffs.Add(item.Key, item.Value);
                }

                shootData.initData.shootProbability += increaseProbability[bulletID];
                shootData.initData.ATK += increaseATK[bulletID];
                shootData.initData.existTime += increaseExistTime[bulletID];
            }
        }

        shootData.initData.ATK += aggressivity;
        shootData.isFollowShooter = shootData.initData.isFollowShooter;

        if (!shootData.initBuffs.ContainsKey(4005)) shootData.n = 1;
        else
        {
            shootData.n = shootData.initBuffs[4005] + 1;
            shootData.n = shootData.n < 2 ? 2 : shootData.n;
        }

        //变式子弹
        shootData.objName = bulletID.ToString();
        if (variantCode > 0)
        {
            shootData.objName = bulletID + "_" + variantCode;
        }

        if (UnityEngine.Random.Range(0, 100) > shootData.initData.shootProbability * 100) return;//概率发射
        if (BulletDic[bulletID].isRandomShoot)
        {
            RandomLauncher(shootData);
        }
        else
        {
            StraightLauncher(shootData);
        }
    }
    private void RandomLauncher(ShootData sd)
    {
        Vector3 instantPos = new Vector3(
                    sd.shooter.position.x, 1f, sd.shooter.position.z);

        int n = 1;

        for (int i = 0; i < n; i++)
        {
            PoolManager.GetInstance().GetObj(sd.objName, t =>
            {
                t.GetComponent<BulletBase>().InitBullet(sd.attacker, sd.attackerTag, sd.initData, sd.initBuffs);
                bulletList.Add(t);

                t.transform.position = instantPos;
                t.transform.rotation = Quaternion.Euler(sd.shooter.rotation.eulerAngles +
                     new Vector3(0, UnityEngine.Random.Range(-60, 60), 0));
                if (sd.isFollowShooter) t.transform.parent = sd.shooter;
            }, ResourceType.Bullet);
        }
    }
    private void StraightLauncher(ShootData sd)
    {
        Debug.Log("StraightLauncher begin!");

        Vector3 instantPos = new Vector3(
                    sd.shooter.position.x, 1f, sd.shooter.position.z);

        for (int i = 0; i < sd.n; i++)
        {
            PoolManager.GetInstance().GetObj(sd.objName, t =>
            {
                Debug.Log("StraightLauncher GetObj begin!");

                t.GetComponent<BulletBase>().InitBullet(sd.attacker, sd.attackerTag, sd.initData, sd.initBuffs);
                bulletList.Add(t);

                Debug.Log("StraightLauncher GetObj bulletList.Add(t)!");
                //GameObject t = PoolManager.GetInstance().GetBullet(bulletType.ToString(), attacker, attackerTag, initData, initBuffs);

                if (sd.n % 2 != 0)//整除2不等于0，中间需要单独放弹幕
                {
                    Vector3 point = new Vector3(instantPos.x + (i - sd.n / 2) * 1f, instantPos.y, instantPos.z);
                    t.transform.position = point;
                }
                else
                {
                    if (i < sd.n / 2)
                    {
                        Vector3 point = new Vector3(instantPos.x + (i - sd.n / 2) * 1f, instantPos.y, instantPos.z);
                        t.transform.position = point;
                    }
                    else
                    {
                        Vector3 point = new Vector3(instantPos.x + (i - sd.n / 2 + 1) * 1f, instantPos.y, instantPos.z);
                        t.transform.position = point;
                    }
                }

                t.transform.rotation = sd.shooter.rotation;
                if (sd.isFollowShooter) t.transform.parent = sd.shooter;
            }, ResourceType.Bullet);
        }

        Debug.Log("StraightLauncher end!");
    }

    public void BulletEvolute(int evolutionID, int bulletType)
    {
        if (!evolvableBuffList.ContainsKey(bulletType) || !evolvableBuffList[bulletType].Contains(evolutionID)) return;
        if (!increaseBuffs[bulletType].ContainsKey(evolutionID)) increaseBuffs[bulletType].Add(evolutionID, 1);
        else increaseBuffs[bulletType][evolutionID]++;
        //if (!BulletDic[bulletType].evolvableList.Contains(evolutionType)) return;

        /*if (BulletBuffs[bulletType].ContainsKey(evolutionType))
        {
            BulletBuffs[bulletType][evolutionType]++;
        }
        else
        {
            BulletBuffs[bulletType].Add(evolutionType, 1);
        }*/
    }
    public void ChangeBullet(int originBullet, int changedBullet)
    {
        foreach (var item in Player._instance.nowBullet)
        {
            if (item.Key == originBullet)
            {
                Player._instance.nowBullet.Remove(item.Key);
                Player._instance.bulletTimer.Remove(item.Key);
                Player._instance.nowBullet.Add(changedBullet, BulletDic[changedBullet].transmissionFrequency);
                Player._instance.bulletTimer.Add(changedBullet, BulletDic[changedBullet].transmissionFrequency);
                return;
            }
        }
    }
}