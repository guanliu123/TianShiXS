using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BulletManager : BaseManager<BulletManager>
{
    //存储每种子弹数据
    public Dictionary<BulletType, BulletData> BulletDic = new Dictionary<BulletType, BulletData>();
    public Dictionary<BulletType, List<BuffType>> evolvableBuffDic = new Dictionary<BulletType, List<BuffType>>();
    //存每种子弹先挂载效果的层数
    public Dictionary<BulletType, Dictionary<BuffType, int>> BulletBuffs = new Dictionary<BulletType, Dictionary<BuffType, int>>();

    //子弹进化数据
    public Dictionary<BulletType, Dictionary<BuffType, int>> increaseBuffs = new Dictionary<BulletType, Dictionary<BuffType, int>>();
    public Dictionary<BulletType, float> increaseProbability = new Dictionary<BulletType, float>();
    public Dictionary<BulletType, float> increaseATK = new Dictionary<BulletType, float>();
    public Dictionary<BulletType, float> increaseShootTimer = new Dictionary<BulletType, float>();
    public Dictionary<BulletType, float> increaseTime = new Dictionary<BulletType, float>();
    public Dictionary<BulletType, bool> haveSpecialEvolved = new Dictionary<BulletType, bool>();

    private List<GameObject> bulletList = new List<GameObject>();

    public BulletManager()
    {
        BulletDic = DataManager.GetInstance().bulletDatasDic;
        Init();
    }
    private void Init()
    {
        foreach (var item in BulletDic)
        {
            if (!BulletBuffs.ContainsKey(item.Key))
            {
                BulletBuffs.Add(item.Key, new Dictionary<BuffType, int>());
                foreach (var t in item.Value.buffList)
                {
                    BulletBuffs[item.Key].Add(t, 1);
                }
            }
            if (!evolvableBuffDic.ContainsKey(item.Key)) evolvableBuffDic.Add(item.Key, item.Value.evolvableList);
            increaseBuffs.Add(item.Key, new Dictionary<BuffType, int>());
            increaseProbability.Add(item.Key, 0);
            increaseATK.Add(item.Key, 0);
            increaseShootTimer.Add(item.Key, 0);
            increaseTime.Add(item.Key, 0);
            haveSpecialEvolved.Add(item.Key, false);           
        }
    }
    public override void Reset()
    {
        increaseBuffs.Clear();
        increaseProbability.Clear();
        increaseTime.Clear();
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

    public void BulletLauncher(Transform shooter, BulletType bulletType, float aggressivity, GameObject attacker)
    {
        if (BulletDic[bulletType].isRandomShoot)
        {
            RandomLauncher(shooter, bulletType, aggressivity, attacker);
        }
        else
        {
            StraightLauncher(shooter, bulletType, aggressivity, attacker);
        }
    }
    private void RandomLauncher(Transform shooter, BulletType bulletType, float aggressivity, GameObject attacker)
    {
        Dictionary<BuffType, int> initBuffs = new Dictionary<BuffType, int>(BulletBuffs[bulletType]);
        BulletData initData = BulletDic[bulletType];
        CharacterTag attackerTag = CharacterTag.Null;

        //针对玩家发出的弹幕进行特化
        var character = attacker.GetComponent<CharacterBase>();
        if (character)
        {
            attackerTag = character.characterTag;
            if (attackerTag == CharacterTag.Player)
            {
                foreach (var item in increaseBuffs[bulletType])
                {
                    if (initBuffs.ContainsKey(item.Key)) initBuffs[item.Key] += item.Value;
                    else initBuffs.Add(item.Key, item.Value);
                }

                initData.shootProbability += increaseProbability[bulletType];
                initData.ATK += increaseATK[bulletType];
                initData.existTime += increaseTime[bulletType];
            }
        }
        initData.ATK += aggressivity;

        if (UnityEngine.Random.Range(0, 100) > initData.shootProbability * 100) return;//概率发射

        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, 1f, shooter.transform.position.z);

        int n;
        if (!initBuffs.ContainsKey(BuffType.Multiply)) n = 1;
        else
        {
            n = initBuffs[BuffType.Multiply] + 1;
            n = n < 2 ? 2 : n;
        }
        for (int i = 0; i < n; i++)
        {
            GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());
            t.GetComponent<BulletBase>().InitBullet(attacker, attackerTag, initData, initBuffs);
            bulletList.Add(t);
            //GameObject t = PoolManager.GetInstance().GetBullet(bulletType.ToString(), attacker, attackerTag, initData, initBuffs);

            t.transform.position = instantPos;
            t.transform.rotation = Quaternion.Euler(shooter.transform.rotation.eulerAngles +
                 new Vector3(0, UnityEngine.Random.Range(-60, 60), 0));
            if (BulletDic[bulletType].isFollowShooter) t.transform.parent = shooter;
        }
    }
    private void StraightLauncher(Transform shooter, BulletType bulletType, float aggressivity, GameObject attacker)
    {
        Dictionary<BuffType, int> initBuffs = new Dictionary<BuffType, int>(BulletBuffs[bulletType]);
        BulletData initData = BulletDic[bulletType];
        CharacterTag attackerTag = CharacterTag.Null;

        //针对玩家发出的弹幕进行特化
        var character = attacker.GetComponent<CharacterBase>();
        if (character)
        {
            attackerTag = character.characterTag;
            if (attackerTag == CharacterTag.Player)
            {
                foreach (var item in increaseBuffs[bulletType])
                {
                    if (initBuffs.ContainsKey(item.Key)) initBuffs[item.Key] += item.Value;
                    else initBuffs.Add(item.Key, item.Value);
                }

                initData.shootProbability += increaseProbability[bulletType];
                initData.ATK += increaseATK[bulletType];
                initData.existTime += increaseTime[bulletType];
            }
        }
        initData.ATK += aggressivity;

        if (UnityEngine.Random.Range(0, 100) > initData.shootProbability * 100) return;//概率发射

        Vector3 instantPos = new Vector3(
                    shooter.transform.position.x, 1f, shooter.transform.position.z);

        int n;
        if (!initBuffs.ContainsKey(BuffType.Multiply)) n = 1;
        else
        {
            n = initBuffs[BuffType.Multiply] + 1;
            n = n < 2 ? 2 : n;
        }
        for (int i = 0; i < n; i++)
        {
            GameObject t = PoolManager.GetInstance().GetObj(bulletType.ToString());
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
            if (BulletDic[bulletType].isFollowShooter) t.transform.parent = shooter;
        }
    }

    public void BulletEvolute(BuffType evolutionType, BulletType bulletType)
    {
        if (!evolvableBuffDic.ContainsKey(bulletType) || !evolvableBuffDic[bulletType].Contains(evolutionType)) return;
        if (!increaseBuffs[bulletType].ContainsKey(evolutionType)) increaseBuffs[bulletType].Add(evolutionType, 1);
        else increaseBuffs[bulletType][evolutionType]++;
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
}