using StarkSDKSpace.SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using LitJson;

public class DataManager : BaseManager<DataManager>
{
    public CharacterSO characterSO;
    private BuffSO buffSO;

    public Dictionary<CharacterType, CharacterTag> characterTagDic = new Dictionary<CharacterType, CharacterTag>();
    public Dictionary<CharacterType, CharacterMsg> characterMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    private Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<BuffType, BuffData> buffDataDic = new Dictionary<BuffType, BuffData>();

    public DataManager()
    {
        characterSO = ResourceManager.GetInstance().LoadByPath<CharacterSO>("ScriptableObject/CharacterSO");
        buffSO = ResourceManager.GetInstance().LoadByPath<BuffSO>("ScriptableObject/BuffSo");

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

#region 玩家角色数据的读写工具
public class RoleData
{
    public CharacterType characterType;
    public float MaxHP;
    public List<BulletType> bulletTypes;
    public float Aggressivity;
    public float ATKSpeed;
    public float avoidance;
    public float energy;
    public int money;
}

public static class RolesData
{
    
    static string filePath = Application.dataPath + "/Resources/Data/Json/Test.json";
    static JsonData jsonData;
    //private static Dictionary<CharacterType, CharacterData> playerDataDic = new Dictionary<CharacterType, CharacterData>();

    static RolesData()
    {
        string jsonString = File.ReadAllText(filePath);
        jsonData = JsonMapper.ToObject(jsonString);
    }

    public static Dictionary<CharacterType, CharacterData> ReadRoleData()
    {
        string jsonString = File.ReadAllText(filePath);
        Dictionary<CharacterType, CharacterData> playerDataDic = new Dictionary<CharacterType, CharacterData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData characterDataJson in jsonData)
        {
            CharacterData characterData = new CharacterData();
            CharacterType characterType = (CharacterType)System.Enum.Parse(typeof(CharacterType), characterDataJson["characterType"].ToString());
            characterData.levelNum = 0;
            characterData.MaxHP = float.Parse(characterDataJson["MaxHP"].ToString());
            characterData.bulletTypes = new List<BulletType>();
            foreach (JsonData bulletTypeJson in characterDataJson["bulletTypes"])
            {
                characterData.bulletTypes.Add((BulletType)System.Enum.Parse(typeof(BulletType), bulletTypeJson.ToString()));
            }
            characterData.Aggressivity = float.Parse(characterDataJson["Aggressivity"].ToString());
            characterData.ATKSpeed = float.Parse(characterDataJson["ATKSpeed"].ToString());
            characterData.avoidance = float.Parse(characterDataJson["avoidance"].ToString());
            characterData.energy = float.Parse(characterDataJson["energy"].ToString());
            characterData.money = int.Parse(characterDataJson["money"].ToString());

            playerDataDic.Add(characterType, characterData);
        }
        return playerDataDic;
    }

    /// <summary>
    /// 返回是否写入成功
    /// </summary>
    /// <param name="characterType"></param>
    /// <param name="_hp"></param>
    /// <param name="_bullet"></param>
    /// <param name="_aggressivity"></param>
    /// <param name="_atkspeed"></param>
    /// <param name="_avoidance"></param>
    /// <param name="_energy"></param>
    /// <param name="_money"></param>
    /// <returns></returns>
    public static bool WriteRoleData(
        CharacterType characterType,
        float _hp = -1,
        List<BulletType> _bullet = null,
        float _aggressivity = -1,
        float _atkspeed = -1,
        float _avoidance = -1,
        float _energy=-1,
        int _money=-1)
    {
        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData characterDataJson = jsonData[i];
            if (characterDataJson["characterType"].ToString() == characterType.ToString())
            {
                if(_hp>0) characterDataJson["MaxHP"] = float.Parse(characterDataJson["MaxHP"].ToString()) + _hp;
                if (_bullet != null)
                {
                    //赋值回去
                    foreach(var item in _bullet)
                    {
                        characterDataJson["bulletTypes"].Add(item.ToString());
                    }
                }
                if(_aggressivity>0) characterDataJson["Aggressivity"] = float.Parse(characterDataJson["Aggressivity"].ToString()) + _aggressivity;
                if(_atkspeed>0) characterDataJson["ATKSpeed"] = float.Parse(characterDataJson["ATKSpeed"].ToString()) + _atkspeed;
                if(_avoidance>0) characterDataJson["avoidance"] = float.Parse(characterDataJson["avoidance"].ToString()) + _avoidance;
                if(_energy>0) characterDataJson["energy"] = float.Parse(characterDataJson["energy"].ToString()) + _energy;
                if(_money>0) characterDataJson["money"] = int.Parse(characterDataJson["money"].ToString())+ 1000;

                string newJsonString = JsonMapper.ToJson(jsonData);
                File.WriteAllText(filePath, newJsonString);
                return true;
            }
        }

        return false;
    }
}
#endregion