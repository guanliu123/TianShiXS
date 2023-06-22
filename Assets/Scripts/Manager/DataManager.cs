using StarkSDKSpace.SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LitJson;

public class DataManager : BaseManager<DataManager>
{
    private ResourcepathSO resourcepathSO;
    public CharacterSO characterSO;
    private BuffSO buffSO;

    private Dictionary<ResourceType, Dictionary<string, string>> objPathDic = new Dictionary<ResourceType, Dictionary<string, string>>();
    public Dictionary<CharacterType, CharacterTag> characterTagDic = new Dictionary<CharacterType, CharacterTag>();
    public Dictionary<CharacterType, CharacterMsg> characterMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    private Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<BulletType, BulletData> bulletDatasDic = new Dictionary<BulletType, BulletData>();
    public Dictionary<int, LevelData> levelDatasDic = new Dictionary<int, LevelData>();
    public Dictionary<BuffType, BuffData> buffDataDic = new Dictionary<BuffType, BuffData>();

    public DataManager()
    {
        resourcepathSO = ResourceManager.GetInstance().LoadByPath<ResourcepathSO>("ScriptableObject/ResourcepathSO");
        characterSO = ResourceManager.GetInstance().LoadByPath<CharacterSO>("ScriptableObject/CharacterSO");
        buffSO = ResourceManager.GetInstance().LoadByPath<BuffSO>("ScriptableObject/BuffSo");

        List<ResourceDatas> t = resourcepathSO.resourcePaths;
        foreach (var item in t)
        {
            objPathDic.Add(item.resourceType, new Dictionary<string, string>());
            foreach (var item1 in item.resourceNPs)
            {
                objPathDic[item.resourceType].Add(item1.name, item1.path);
            }
        }

        foreach (var item in characterSO.characterdatas)
        {
            characterTagDic.Add(item.characterType, item.characterTag);
            characterMsgDic.Add(item.characterType, item.characterMsg);
            characterDatasDic.Add(item.characterType, new Dictionary<int, CharacterData>());
            foreach (var item1 in item.characterData)
            {
                if (!characterDatasDic[item.characterType].ContainsKey(item1.levelNum)) characterDatasDic[item.characterType].Add(item1.levelNum, item1);
            }
        }
        foreach (var item in buffSO.buffdatas)
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

    public (CharacterTag, CharacterData) AskCharacterData(CharacterType characterType, int levelNum)
    {
        CharacterData _data = new CharacterData();
        CharacterTag _tag = CharacterTag.Null;

        _data.MaxHP = -1;//初始化的角色获取数据时如果获取到的血量为-1意为没获取到数据

        if (characterDatasDic.ContainsKey(characterType) && characterDatasDic[characterType].ContainsKey(levelNum))
        {
            _data = characterDatasDic[characterType][levelNum];
            _tag = characterTagDic[characterType];
        }

        return (_tag, _data);
    }

    public BuffData AskBuffDate(BuffType buffType)
    {
        BuffData t = new BuffData();

        if (buffDataDic.ContainsKey(buffType)) t = buffDataDic[buffType];

        return t;
    }
}