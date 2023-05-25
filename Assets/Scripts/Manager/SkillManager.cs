using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Skills;

public class SkillManager : BaseManager<SkillManager>
{
    SkillSO skillSO;

    public Dictionary<int,int> skillPool = new Dictionary<int, int>();//目前可以抽取的所有技能所在的池子
    public Dictionary<int, int> occurredSkill = new Dictionary<int, int>();//目前已经出现过的技能池子

    public Dictionary<int, SkillDatas> skillDatas=new Dictionary<int, SkillDatas>();
    public Dictionary<int, ISkill> skillEvent = new Dictionary<int, ISkill>();

    public SkillManager(){
        skillSO = ResourceManager.GetInstance().LoadByPath<SkillSO>("ScriptableObject/SkillSO");

        foreach(var item in skillSO.skilldatas)
        {
            if (item.num > 0 && item.beforeSkills.Count <= 0)
            {
                skillPool.Add(item.id, item.num);
                occurredSkill.Add(item.id, 1);
                skillDatas.Add(item.id, item);
                Debug.Log("将技能" + item.describe + "加入技能池");
            }
            
        }
        InitSkill();
    }

    public void UpdateSkillPool(int usedId)
    {
        int n = skillPool[usedId] - 1;
        if (n <= 0)
        {
            skillPool.Remove(usedId);
            if (!occurredSkill.ContainsKey(usedId)) 
                occurredSkill.Add(usedId, 1);
        }
        else skillPool[usedId]--;

        foreach (var item in skillSO.skilldatas)
        {
            if (item.beforeSkills.Contains(usedId) && !occurredSkill.ContainsKey(item.id))
            {
                skillPool.Add(item.id, item.num);
                Debug.Log("将技能" + item.describe + "加入技能池");
            }
        }
    }

    public List<SkillUpgrade> RandomSkill()
    {        
        List<SkillUpgrade> skillUpgrades = new List<SkillUpgrade>();
        List<int> selectedList = new List<int>();
        int t;

        for (int i = 0; i < 3; i++)
        {
            SkillUpgrade temp;
            if (skillPool.Count <= 0) t = -1;
            else if (skillPool.Count < 3)
            {
                if (i >= skillPool.Count)
                {
                    t = skillPool.Count - 1;
                }
                else t = i;
            }
            else
            {
                do
                {
                    t = Random.Range(0, skillPool.Count);
                } while (selectedList.Contains(t));
            }
            if (t == -1)
            {
                temp.icon = null;
                temp.name = "无";
                temp.describe = "现在没有可用技能！";
                temp.skill = null;

                skillUpgrades.Add(temp);
                continue;
            }
            SkillDatas data = skillDatas[skillPool.ElementAt(t).Key];

            temp.icon = data.icon;
            temp.name = data.name;
            temp.describe = data.describe;
            temp.skill = skillEvent[data.id];

            skillUpgrades.Add(temp);
        }

        return skillUpgrades;

        /*List<(BulletType, BuffType)> choices = new List<(BulletType, BuffType)>();
        
        BulletType t1 = BulletType.NULL;
        BuffType t2 = BuffType.NULL;


        for (int i = 0; i < 3; i++)
        {
            int randomNum = 0;//防止一直抽不到不同的buff死循环
            do
            {
                if (Player._instance.nowBullet.Count <= 0) break;
                int t = Random.Range(0, Player._instance.nowBullet.Count);
                t1 = Player._instance.nowBullet.ElementAt(t).Key;

                randomNum++;

                if (BulletManager.GetInstance().BulletDic[t1].evolvableList.Count <= 0) continue;
                t = Random.Range(0,
                    BulletManager.GetInstance().BulletDic[t1].evolvableList.Count);
                t2 = BulletManager.GetInstance().BulletDic[t1].evolvableList[t];
            } while (choices.Contains((t1, t2)) && randomNum < 3);

            choices.Add((t1, t2));
            SkillUpgrade item;

            item.icon = null;
            item.bulletType = choices[i].Item1;
            item.buffType = choices[i].Item2;

            if (t1 == BulletType.NULL || t2 == BuffType.NULL)
            {
                item.describe = "当前无可进化技能！";
            }
            else
            {
                item.describe = "为" + item.bulletType.ToString() + "弹幕添加" + item.buffType.ToString() + "效果";
            }

            skillUpgrades.Add(item);
        }*/
    }

    private void InitSkill()
    {
        skillEvent.Add(0, new AddCritProbability());
        skillEvent.Add(1, new AddCritRate());
        skillEvent.Add(2, new AddPosionWeapon());
        skillEvent.Add(3, new AddPosionPile());
        skillEvent.Add(4, new AddShootPosionProbability());
        skillEvent.Add(5, new AddForst());
        skillEvent.Add(6, new AddForstProbability());
        skillEvent.Add(7, new AddForstTime());
        skillEvent.Add(8, new AddForstDamage());
        skillEvent.Add(9, new AddBulletDamage());
        skillEvent.Add(10, new AddFanshaped());
        skillEvent.Add(11, new IncreaseFanshapedAttack());
        skillEvent.Add(12, new AddLaserBullet());
        skillEvent.Add(13, new IncreaseLaserTime());
        skillEvent.Add(14, new AddPerpetualBullet());
        skillEvent.Add(15, new IncreasePerpetualTrack());
        skillEvent.Add(16, new IncreasePerpetualNum());
        skillEvent.Add(17, new AddFireBall());
        skillEvent.Add(18, new IncreaseFireSize());
        skillEvent.Add(19, new IncreaseFireTrack());
        skillEvent.Add(20, new AddRotateBullet());
        skillEvent.Add(21, new AddRotateBulletDivision());
        skillEvent.Add(22, new AddBounceBullet());
        skillEvent.Add(23, new EvolveBounceBullet());
        skillEvent.Add(24, new AddBulletShoot());
        skillEvent.Add(25, new AddVampirism());
        skillEvent.Add(26, new AddMaxHP());
        skillEvent.Add(27, new AddMoney());
        skillEvent.Add(28, new AddBulletTrack());
        skillEvent.Add(29, new AddReflect());
        skillEvent.Add(30, new AddEnergy());
        skillEvent.Add(31, new AddShield());
        skillEvent.Add(32, new IncreaseSpeed());
    }
}
