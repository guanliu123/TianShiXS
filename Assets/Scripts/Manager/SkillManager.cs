using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Skills;
using System;
using System.Reflection;

public class SkillManager : BaseManager<SkillManager>
{
    public static Dictionary<int, SkillData> SkillDic = new Dictionary<int, SkillData>();

    public Dictionary<int,int> skillPool = new Dictionary<int, int>();//目前可以抽取的所有技能所在的池子
    public Dictionary<int, int> occurredSkill = new Dictionary<int, int>();//目前已经出现过的技能池子
    private float randomValue;

    //public Dictionary<int, ISkill> skillEvent = new Dictionary<int, ISkill>();
    public Dictionary<int, SkillBase> skillEvent = new Dictionary<int, SkillBase>();


    public Dictionary<int,Sprite> nowSkillIcons = new Dictionary<int, Sprite>();
    static SkillManager()
    {
        SkillDic =SkillDataTool.ReadSkillData();
    }
    public SkillManager(){
        //skillSO = ResourceManager.GetInstance().LoadByPath<SkillSO>("ScriptableObject/SkillSO");

        InitSkillPool();
        InitSkillEvent();
    }

    private void InitSkillPool()
    {
        foreach (var item in SkillDic)
        {
            //if(!skillDatas.ContainsKey(item.id)) skillDatas.Add(item.id, item);
            if (item.Value.num > 0 && item.Value.beforeSkills.Count <= 0)
            {
                skillPool.Add(item.Value.id, item.Value.num);
                occurredSkill.Add(item.Value.id, 1);
                randomValue += item.Value.probability;
            }            
        }
    }

    public override void Reset()
    {
        base.Reset();
        skillPool.Clear();
        occurredSkill.Clear();
        nowSkillIcons.Clear();
        InitSkillPool();
    }

    public void UpdateSkillPool(int usedId)
    {
        if (!nowSkillIcons.ContainsKey(usedId)) nowSkillIcons.Add(usedId, SkillDic[usedId].icon);
        int n = skillPool[usedId] - 1;       
        if (n <= 0)
        {
            skillPool.Remove(usedId);
            randomValue -= SkillDic[usedId].probability;
        }
        else skillPool[usedId]--;

        if (!occurredSkill.ContainsKey(usedId))
        {
            occurredSkill.Add(usedId, 1);
        }
        else occurredSkill[usedId]++;

        foreach (var item in SkillDic)
        {
            if (item.Value.beforeSkills.Contains(usedId) && !occurredSkill.ContainsKey(item.Value.id))
            {
                //skillPool.Add(item.Value.id, item.Value.num);
                TryUnlockSkill(item.Value.id);
            }
        }
    }

    private void TryUnlockSkill(int id)
    {
        foreach (var item in SkillDic[id].beforeSkills)
        {
            if (!occurredSkill.ContainsKey(item)) return;
        }
        skillPool.Add(id, SkillDic[id].num);
        randomValue += SkillDic[id].probability;
    }

    public List<SkillUpgrade> RandomSkill()
    {        
        List<SkillUpgrade> skillUpgrades = new List<SkillUpgrade>();
        HashSet<int> indices = new HashSet<int>();

        //float t_random=-1;
        int t_id = 0;

        for (int i = 0; i < 3; i++)
        {
            SkillUpgrade temp;
            if (skillPool.Count <= 0) t_id = -1;
            else if (skillPool.Count < 3)
            {
                if (i >= skillPool.Count)
                {
                    t_id = skillPool.ElementAt(skillPool.Count - 1).Key;
                }
                else t_id = skillPool.ElementAt(i).Key;
            }
            else
            {
                do
                {
                    t_id = ExtractSkill(indices);
                } while (t_id < 0);

                indices.Add(t_id);
            }
            if (t_id < 0)
            {
                temp.icon = null;
                temp.name = "无";
                temp.describe = "现在没有可用技能！";
                temp.quality = null;
                temp.skill = null;

                skillUpgrades.Add(temp);
                break;
            }

            SkillData data = SkillDic[t_id];
            //int iconNum = 0;
            /*if (occurredSkill.ContainsKey(skillPool.ElementAt(t).Key))
            {
                iconNum = Mathf.Min(occurredSkill[skillPool.ElementAt(t).Key], data.icon);
            }*/

            temp.icon = data.icon;
            temp.name = data.name;
            temp.describe = data.describe;
            temp.quality = data.quality;
            temp.skill = skillEvent[data.id];

            skillUpgrades.Add(temp);
        }

        return skillUpgrades;
    }

    private int ExtractSkill(HashSet<int> indices)
    {
        int id = -1;       

        float t_random = UnityEngine.Random.Range(0, randomValue * 100);
        foreach (var item in skillPool)
        {
            if (t_random < SkillDic[item.Key].probability * 100)
            {
                if (!indices.Contains(item.Key))
                {
                    id = item.Key;
                    break;
                }
            }
            t_random -= SkillDic[item.Key].probability * 100;
        }
        
        return id;
    }

    private void InitSkillEvent()
    {
        int skillNum = SkillDic.Count;
        int n = 0;

        Type baseType = typeof(SkillBase);
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.IsSubclassOf(baseType))
            {
                SkillBase skill = (SkillBase)Activator.CreateInstance(type);
                // 在这里可以对skill进行进一步的操作
                skillEvent.Add(skill.id, skill);
                n++;
            }
            if (n >= skillNum) break;
        }
    }
}
