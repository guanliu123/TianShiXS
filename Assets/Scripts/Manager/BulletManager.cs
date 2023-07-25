using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
        BulletDic = BulletDataTool.ReadBulletData();

        Init();
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

    public void BulletLauncher(Transform shooter, int bulletID, float aggressivity, GameObject attacker)
    {

        if (bulletID < 0) return;
        if (BulletDic[bulletID].isRandomShoot)
        {
            RandomLauncher(shooter, bulletID, aggressivity, attacker);
        }
        else
        {
            StraightLauncher(shooter, bulletID, aggressivity, attacker);
        }
    }
    private void RandomLauncher(Transform shooter, int bulletID, float aggressivity, GameObject attacker)
    {
        Dictionary<int, int> initBuffs = new Dictionary<int, int>(BulletBuffs[bulletID]);
        BulletData initData = BulletDic[bulletID];
        CharacterTag attackerTag = CharacterTag.Null;

        //针对玩家发出的弹幕进行特化
        var character = attacker.GetComponent<CharacterBase>();
        if (character)
        {
            attackerTag = character.characterTag;
            if (attackerTag == CharacterTag.Player)
            {
                foreach (var item in increaseBuffs[bulletID])
                {
                    if (initBuffs.ContainsKey(item.Key)) initBuffs[item.Key] += item.Value;
                    else initBuffs.Add(item.Key, item.Value);
                }

                initData.shootProbability += increaseProbability[bulletID];
                initData.ATK += increaseATK[bulletID];
                initData.existTime += increaseExistTime[bulletID];
            }
        }
        initData.ATK += aggressivity;

        if (UnityEngine.Random.Range(0, 100) > initData.shootProbability * 100) return;//概率发射

        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, 1f, shooter.transform.position.z);

        int n = 1;
        //确定好倍增buff的id后修改这里

        /*if (!initBuffs.ContainsKey(BuffType.Multiply)) n = 1;
        else
        {
            n = initBuffs[BuffType.Multiply] + 1;
            n = n < 2 ? 2 : n;
        }*/
        for (int i = 0; i < n; i++)
        {
            PoolManager.GetInstance().GetObj(bulletID.ToString(), t =>
            {
                t.GetComponent<BulletBase>().InitBullet(attacker, attackerTag, initData, initBuffs);
                bulletList.Add(t);

                t.transform.position = instantPos;
                t.transform.rotation = Quaternion.Euler(shooter.transform.rotation.eulerAngles +
                     new Vector3(0, UnityEngine.Random.Range(-60, 60), 0));
                if (BulletDic[bulletID].isFollowShooter) t.transform.parent = shooter;
            }, ResourceType.Bullet);
        }
    }
    private void StraightLauncher(Transform shooter, int bulletID, float aggressivity, GameObject attacker)
    {
        Dictionary<int, int> initBuffs = new Dictionary<int, int>(BulletBuffs[bulletID]);
        BulletData initData = BulletDic[bulletID];
        CharacterTag attackerTag = CharacterTag.Null;

        //针对玩家发出的弹幕进行特化
        var character = attacker.GetComponent<CharacterBase>();
        if (character)
        {
            attackerTag = character.characterTag;
            if (attackerTag == CharacterTag.Player)
            {
                foreach (var item in increaseBuffs[bulletID])
                {
                    if (initBuffs.ContainsKey(item.Key)) initBuffs[item.Key] += item.Value;
                    else initBuffs.Add(item.Key, item.Value);
                }

                initData.shootProbability += increaseProbability[bulletID];
                initData.ATK += increaseATK[bulletID];
                initData.existTime += increaseExistTime[bulletID];
            }
        }
        initData.ATK += aggressivity;

        if (UnityEngine.Random.Range(0, 100) > initData.shootProbability * 100) return;//概率发射

        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, 1f, shooter.transform.position.z);

        int n = 1;
        //确定好倍增buff的id后修改这里
        /*if (!initBuffs.ContainsKey(BuffType.Multiply)) n = 1;
        else
        {
            n = initBuffs[BuffType.Multiply] + 1;
            n = n < 2 ? 2 : n;
        }*/
        for (int i = 0; i < n; i++)
        {
            PoolManager.GetInstance().GetObj(bulletID.ToString(), t =>
            {
                t.GetComponent<BulletBase>().InitBullet(attacker, attackerTag, initData, initBuffs);
                bulletList.Add(t);
                //GameObject t = PoolManager.GetInstance().GetBullet(bulletType.ToString(), attacker, attackerTag, initData, initBuffs);

                if (n % 2 != 0)//整除2不等于0，中间需要单独放弹幕
                {
                    Vector3 point = new Vector3(instantPos.x + (i - n / 2) * 1f, instantPos.y, instantPos.z);
                    t.transform.position = point;
                }
                else
                {
                    if (i < n / 2)
                    {
                        Vector3 point = new Vector3(instantPos.x + (i - n / 2) * 1f, instantPos.y, instantPos.z);
                        t.transform.position = point;
                    }
                    else
                    {
                        Vector3 point = new Vector3(instantPos.x + (i - n / 2 + 1) * 1f, instantPos.y, instantPos.z);
                        t.transform.position = point;
                    }
                }

                t.transform.rotation = shooter.transform.rotation;
                if (BulletDic[bulletID].isFollowShooter) t.transform.parent = shooter;
            }, ResourceType.Bullet);
        }
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