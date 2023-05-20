using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : BaseManager<DataManager>
{
    private ResourcepathSO resourcepathSO;
    private CharacterSO characterSO;
    private BulletSO bulletSO;
    private LevelSO levelSO;
    private BuffSO buffSO;

    private Dictionary<ResourceType, Dictionary<string, string>> objPathDic = new Dictionary<ResourceType, Dictionary<string, string>>();
    private Dictionary<CharacterType, CharacterData> characterDatasDic=new Dictionary<CharacterType, CharacterData>();
    public Dictionary<BulletType, BulletData> bulletDatasDic = new Dictionary<BulletType, BulletData>();
    private Dictionary<int, LevelData> levelDatasDic = new Dictionary<int, LevelData>();
    private Dictionary<BuffType, BuffData> buffDataDic = new Dictionary<BuffType, BuffData>();

    public DataManager()
    {
        resourcepathSO = ResourceManager.GetInstance().LoadByPath<ResourcepathSO>("ScriptableObject/ResourcepathSO");
        characterSO = ResourceManager.GetInstance().LoadByPath<CharacterSO>("ScriptableObject/CharacterSO");
        bulletSO = ResourceManager.GetInstance().LoadByPath<BulletSO>("ScriptableObject/BulletSO");
        levelSO = ResourceManager.GetInstance().LoadByPath<LevelSO>("ScriptableObject/LevelSO");
        buffSO = ResourceManager.GetInstance().LoadByPath<BuffSO>("ScriptableObject/BuffSo");

        List <ResourceDatas> t = resourcepathSO.resourcePaths;
        foreach(var item in t)
        {
            objPathDic.Add(item.resourceType, new Dictionary<string, string>());
            foreach(var item1 in item.resourceNPs)
            {
                objPathDic[item.resourceType].Add(item1.name, item1.path);
            }
        }
        foreach(var item in characterSO.characterdatas)
        {
            characterDatasDic.Add(item.characterType,item.characterData);
        }
        foreach(var item in bulletSO.bulletdatas)
        {
            bulletDatasDic.Add(item.bulletType, item.bulletData);
        }
        foreach(var item in levelSO.leveldatas)
        {
            levelDatasDic.Add(item.id, item.levelData);
        }
        foreach(var item in buffSO.buffdatas)
        {
            buffDataDic.Add(item.buffType, item.buffData);
        }
    }

    /// <summary>
    /// 查询名称为objName的资源路径，主要给ResourceManager用的
    /// </summary>
    /// <param name="objName">目标物体名称</param>
    /// <returns></returns>
    public string AskAPath(string objName)
    {
        
        foreach (var item in objPathDic)
        {
            if (item.Value.ContainsKey(objName))
            {
                return item.Value[objName];
            }
        }
        
        return null;
    }

    /// <summary>
    /// 查询某一个资源类型中所有的数据路径
    /// </summary>
    /// <param name="resourceType">目标资源类型</param>
    /// <returns></returns>
    public List<string> AskPaths(ResourceType resourceType)
    {
        if (objPathDic.ContainsKey(resourceType)) return objPathDic[resourceType].Values.ToList<string>();

        return null;
    }

    public CharacterData AskCharacterData(CharacterType characterType)
    {
        CharacterData t = new CharacterData();
        t.MaxHP = -1;//初始化的角色获取数据时如果获取到的血量为-1意为没获取到数据

        if (characterDatasDic.ContainsKey(characterType)) t = characterDatasDic[characterType];

        return t;
    }

    public BulletData AskBulletData(BulletType bulletType)
    {
        BulletData t=new BulletData();

        if (bulletDatasDic.ContainsKey(bulletType)) t = bulletDatasDic[bulletType];

        return t;
    }
    public LevelData AskLevelData(int levelNum)
    {
        LevelData t = new LevelData();
        if (levelDatasDic.ContainsKey(levelNum)) t = levelDatasDic[levelNum];

        return t;
    }
    public BuffData AskBuffDate(BuffType buffType)
    {
        BuffData t = new BuffData();

        if (buffDataDic.ContainsKey(buffType)) t = buffDataDic[buffType];

        return t;
    }
}
