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
    private BulletSO bulletSO;
    private LevelSO levelSO;
    private BuffSO buffSO;
    public SkillSO skillSO;

    private Dictionary<ResourceType, Dictionary<string, string>> objPathDic = new Dictionary<ResourceType, Dictionary<string, string>>();
    public Dictionary<CharacterType, CharacterTag> characterTagDic = new Dictionary<CharacterType, CharacterTag>();
    public Dictionary<CharacterType, CharacterMsg> characterMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    private Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic=new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<BulletType, BulletData> bulletDatasDic = new Dictionary<BulletType, BulletData>();
    public Dictionary<int, LevelData> levelDatasDic = new Dictionary<int, LevelData>();
    private Dictionary<BuffType, BuffData> buffDataDic = new Dictionary<BuffType, BuffData>();

    public DataManager()
    {
        resourcepathSO = ResourceManager.GetInstance().LoadByPath<ResourcepathSO>("ScriptableObject/ResourcepathSO");
        characterSO = ResourceManager.GetInstance().LoadByPath<CharacterSO>("ScriptableObject/CharacterSO");
        bulletSO = ResourceManager.GetInstance().LoadByPath<BulletSO>("ScriptableObject/BulletSO");
        levelSO = ResourceManager.GetInstance().LoadByPath<LevelSO>("ScriptableObject/LevelSO");
        buffSO = ResourceManager.GetInstance().LoadByPath<BuffSO>("ScriptableObject/BuffSo");
        skillSO= ResourceManager.GetInstance().LoadByPath<SkillSO>("ScriptableObject/SkillSO");

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
            characterTagDic.Add(item.characterType, item.characterTag);
            characterMsgDic.Add(item.characterType,item.characterMsg);
            characterDatasDic.Add(item.characterType, new Dictionary<int, CharacterData>());
            foreach(var item1 in item.characterData)
            {
                if(!characterDatasDic[item.characterType].ContainsKey(item1.levelNum)) characterDatasDic[item.characterType].Add(item1.levelNum, item1);
            }
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
    /// ��ѯ����ΪobjName����Դ·������Ҫ��ResourceManager�õ�
    /// </summary>
    /// <param name="objName">Ŀ����������</param>
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
    /// ��ѯĳһ����Դ���������е�����·��
    /// </summary>
    /// <param name="resourceType">Ŀ����Դ����</param>
    /// <returns></returns>
    public List<string> AskPaths(ResourceType resourceType)
    {
        if (objPathDic.ContainsKey(resourceType)) return objPathDic[resourceType].Values.ToList<string>();

        return null;
    }

    public (CharacterTag,CharacterData) AskCharacterData(CharacterType characterType,int levelNum)
    {
        CharacterData _data = new CharacterData();
        CharacterTag _tag=CharacterTag.Null;

        _data.MaxHP = -1;//��ʼ���Ľ�ɫ��ȡ����ʱ�����ȡ����Ѫ��Ϊ-1��Ϊû��ȡ������

        if (characterDatasDic.ContainsKey(characterType) && characterDatasDic[characterType].ContainsKey(levelNum))
        {
            _data = characterDatasDic[characterType][levelNum];
            _tag = characterTagDic[characterType];
        }

        return (_tag, _data);
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
