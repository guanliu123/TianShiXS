using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Skills;

public class SkillManager : BaseManager<SkillManager>
{
    public static Dictionary<int, SkillData> skillDatas = new Dictionary<int, SkillData>();

    public Dictionary<int,int> skillPool = new Dictionary<int, int>();//目前可以抽取的所有技能所在的池子
    public Dictionary<int, int> occurredSkill = new Dictionary<int, int>();//目前已经出现过的技能池子
    
    public Dictionary<int, ISkill> skillEvent = new Dictionary<int, ISkill>();

    public Dictionary<int,Sprite> nowSkillIcons = new Dictionary<int, Sprite>();
    static SkillManager()
    {
        skillDatas =SkillDataTool.ReadSkillData();
    }
    public SkillManager(){
        //skillSO = ResourceManager.GetInstance().LoadByPath<SkillSO>("ScriptableObject/SkillSO");

        InitSkill();
        InitSkillDic();
    }

    private void InitSkill()
    {
        foreach (var item in skillDatas)
        {
            //if(!skillDatas.ContainsKey(item.id)) skillDatas.Add(item.id, item);
            if (item.Value.num > 0 && item.Value.beforeSkills.Count <= 0)
            {
                skillPool.Add(item.Value.id, item.Value.num);
                occurredSkill.Add(item.Value.id, 1);
            }            
        }
    }

    public override void Reset()
    {
        base.Reset();
        skillPool.Clear();
        occurredSkill.Clear();
        nowSkillIcons.Clear();
        InitSkill();
    }

    public void UpdateSkillPool(int usedId)
    {
        if (!nowSkillIcons.ContainsKey(usedId)) nowSkillIcons.Add(usedId, skillDatas[usedId].icon);
        int n = skillPool[usedId] - 1;       
        if (n <= 0) skillPool.Remove(usedId);
        else skillPool[usedId]--;
            
        if (!occurredSkill.ContainsKey(usedId))
        {
            occurredSkill.Add(usedId, 1);
        }
        else occurredSkill[usedId]++;

        foreach (var item in skillDatas)
        {
            if (item.Value.beforeSkills.Contains(usedId) && !occurredSkill.ContainsKey(item.Value.id))
            {
                skillPool.Add(item.Value.id, item.Value.num);
            }
        }
    }

    public List<SkillUpgrade> RandomSkill()
    {        
        List<SkillUpgrade> skillUpgrades = new List<SkillUpgrade>();
        HashSet<int> indices = new HashSet<int>();

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
                } while (indices.Contains(t));
                
                indices.Add(t);
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

            SkillData data = skillDatas[skillPool.ElementAt(t).Key];
            int iconNum = 0;
            /*if (occurredSkill.ContainsKey(skillPool.ElementAt(t).Key))
            {
                iconNum = Mathf.Min(occurredSkill[skillPool.ElementAt(t).Key], data.icon);
            }*/

            temp.icon = data.icon;
            temp.name = data.name;
            temp.describe = data.describe;
            temp.skill = skillEvent[data.id];

            skillUpgrades.Add(temp);
        }

        return skillUpgrades;
    }

    private void InitSkillDic()
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
