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

    //public Dictionary<CharacterType, CharacterTag> characterTagDic = new Dictionary<CharacterType, CharacterTag>();
    public Dictionary<CharacterType, CharacterMsg> characterMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    private Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<BuffType, BuffData> buffDataDic = new Dictionary<BuffType, BuffData>();

    public DataManager()
    {
        characterSO = ResourceManager.GetInstance().LoadByPath<CharacterSO>("ScriptableObject/CharacterSO");
        buffSO = ResourceManager.GetInstance().LoadByPath<BuffSO>("ScriptableObject/BuffSo");

        foreach (var item in characterSO.characterdatas)
        {
            //characterTagDic.Add(item.characterType, item.characterTag);
            characterMsgDic.Add(item.characterType, item.characterMsg);
            characterDatasDic.Add(item.characterType, new Dictionary<int, CharacterData>());
            foreach (var item1 in item.characterData)
            {
                //if (!characterDatasDic[item.characterType].ContainsKey(item1.levelNum)) characterDatasDic[item.characterType].Add(item1.levelNum, item1);
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
            //_tag = characterTagDic[characterType];
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

#region 资源数据的读写工具
public class ResourceDataTool
{
    static string filePath = Application.dataPath + "/Resources/Data/Json/Resource.json";

    public static Dictionary<ResourceType, string> ReadResourceData()
    {
        string jsonString = File.ReadAllText(filePath);
        Dictionary<ResourceType, string> resourceDataDic = new Dictionary<ResourceType, string>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData resourceDataJson in jsonData)
        {
            ResourceType resourceType = (ResourceType)System.Enum.Parse(typeof(ResourceType), resourceDataJson["ResourceType"].ToString());
            string resourcePath = resourceDataJson["ResourcePath"].ToString();

            resourceDataDic.Add(resourceType, resourcePath);
        }
        return resourceDataDic;
    }
}

#endregion

#region 玩家角色数据的读写工具
public static class RoleDataTool
{   
    static string filePath = Application.dataPath + "/Resources/Data/Json/Role.json";
    static JsonData jsonData;
    public static Dictionary<CharacterType, CharacterMsg> roleMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    public static Dictionary<CharacterType, Dictionary<int, CharacterData>> roleDataDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();

    static RoleDataTool()
    {
        string jsonString = File.ReadAllText(filePath);
        jsonData = JsonMapper.ToObject(jsonString);
    }

    public static void ReadRoleData()
    {
        string jsonString = File.ReadAllText(filePath);
        //Dictionary<CharacterType, Dictionary<int, CharacterData>> playerDataDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData characterDataJson in jsonData)
        {
            CharacterData characterData = new CharacterData();
            CharacterMsg characterMsg = new CharacterMsg();

            CharacterType characterType = (CharacterType)System.Enum.Parse(typeof(CharacterType), characterDataJson["CharacterType"].ToString());
            roleDataDic.Add(characterType, new Dictionary<int, CharacterData>());

            characterMsg.name = characterDataJson["Name"].ToString();
            characterMsg.image= characterType+"Image";
            characterMsg.describe= characterDataJson["Describe"].ToString();
            roleMsgDic.Add(characterType, characterMsg);

            characterData.MaxHP = float.Parse(characterDataJson["MaxHP"].ToString());
            characterData.bulletTypes = new List<BulletType>();
            foreach (JsonData bulletTypeJson in characterDataJson["Bullets"])
            {
                characterData.bulletTypes.Add((BulletType)System.Enum.Parse(typeof(BulletType), bulletTypeJson.ToString()));
            }
            characterData.ATK = float.Parse(characterDataJson["ATK"].ToString());
            characterData.ATKSpeed = float.Parse(characterDataJson["ATKSpeed"].ToString());           
            characterData.energy = 0;
            characterData.money = 0;

            roleDataDic[characterType].Add(0, characterData);
        }
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
        float _atkspeed = -1
        )
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
                /*if(_avoidance>0) characterDataJson["avoidance"] = float.Parse(characterDataJson["avoidance"].ToString()) + _avoidance;
                if(_energy>0) characterDataJson["energy"] = float.Parse(characterDataJson["energy"].ToString()) + _energy;
                if(_money>0) characterDataJson["money"] = int.Parse(characterDataJson["money"].ToString())+ _money;*/

                string newJsonString = JsonMapper.ToJson(jsonData);
                File.WriteAllText(filePath, newJsonString);
                return true;
            }
        }

        return false;
    }
}
#endregion

#region 怪物数据的读取工具

public static class EnemyDataTool
{
    static string filePath = Application.dataPath + "/Resources/Data/Json/Enemy.json";
    public static Dictionary<CharacterType, CharacterMsg> enemyMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    public static Dictionary<CharacterType, Dictionary<int, CharacterData>> enemyDataDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();


    public static void ReadEnemyData()
    {
        string jsonString = File.ReadAllText(filePath);
        //Dictionary<CharacterType, Dictionary<int, CharacterData>> playerDataDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData enemyDataJson in jsonData)
        {
            CharacterData enemyData = new CharacterData();
            CharacterMsg enemyMsg = new CharacterMsg();

            CharacterType characterType = (CharacterType)System.Enum.Parse(typeof(CharacterType), enemyDataJson["CharacterType"].ToString());
            enemyDataDic.Add(characterType, new Dictionary<int, CharacterData>());

            enemyMsg.name = enemyDataJson["Name"].ToString();
            enemyMsg.image = characterType + "Image";
            enemyMsg.describe = enemyDataJson["Describe"].ToString();
            enemyMsgDic.Add(characterType, enemyMsg);

            foreach (JsonData item in enemyDataJson["LevelDatas"])
            {
                int levelNum = int.Parse(item["LevelNum"].ToString());
                enemyData.MaxHP = float.Parse(item["MaxHP"].ToString());
                enemyData.bulletTypes = new List<BulletType>();
                foreach (JsonData bulletTypeJson in item["Bullets"])
                {
                    enemyData.bulletTypes.Add((BulletType)System.Enum.Parse(typeof(BulletType), bulletTypeJson.ToString()));
                }
                enemyData.ATK = float.Parse(item["ATK"].ToString());
                enemyData.ATKSpeed = float.Parse(item["ATKSpeed"].ToString());
                //characterData.avoidance = float.Parse(characterDataJson["avoidance"].ToString());
                enemyData.energy = float.Parse(item["Energy"].ToString());
                enemyData.money = int.Parse(item["Money"].ToString());

                enemyDataDic[characterType].Add(levelNum, enemyData);
            }
        }
    }
}

#endregion

#region 子弹数据的读取工具

public static class BulletDataTool
{
    static string filePath = Application.dataPath + "/Resources/Data/Json/Bullet.json";

    public static Dictionary<BulletType, BulletData> ReadBulletData()
    {
        string jsonString = File.ReadAllText(filePath);
        Dictionary<BulletType, BulletData> bulletDataDic = new Dictionary<BulletType, BulletData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData item in jsonData)
        {
            BulletType bulletType = (BulletType)System.Enum.Parse(typeof(BulletType), item["BulletType"].ToString());
            BulletData bulletData = new BulletData();

            bulletData.transmissionFrequency = float.Parse(item["TransmissionFrequency"].ToString());
            bulletData.isRandomShoot = (bool)item["IsRandomShoot"];
            bulletData.existTime= float.Parse(item["ExistTime"].ToString());
            bulletData.moveSpeed= float.Parse(item["MoveSpeed"].ToString());
            bulletData.rotateSpeed= float.Parse(item["RotateSpeed"].ToString());
            bulletData.buffList = new List<BuffType>();
            foreach(JsonData buffType in item["BuffList"])
            {
                //Debug.Log((BuffType)System.Enum.Parse(typeof(BuffType), buffType.ToString()));
                bulletData.buffList.Add((BuffType)System.Enum.Parse(typeof(BuffType), buffType.ToString()));
            }
            bulletData.evolvableList = new List<BuffType>();
            foreach (JsonData buffType in item["EvolvableList"])
            {
                bulletData.evolvableList.Add((BuffType)System.Enum.Parse(typeof(BuffType), buffType.ToString()));
            }
            bulletData.crit= float.Parse(item["Crit"].ToString());
            bulletData.critRate= float.Parse(item["CritRate"].ToString());
            bulletData.ATK= float.Parse(item["ATK"].ToString());
            bulletData.damageInterval= float.Parse(item["DamageInterval"].ToString());
            bulletData.isFollowShooter = (bool)item["IsFollowShooter"];
            bulletData.shootProbability= float.Parse(item["ShootProbability"].ToString());

            bulletData.audioName = bulletType.ToString() + "Audio";
            bulletData.effectName = bulletType.ToString() + "Effect";

            bulletDataDic.Add(bulletType, bulletData);
        }
        return bulletDataDic;
    }
}

#endregion

#region 关卡数据读取工具

public static class LevelDataTool
{
    static string filePath = Application.dataPath + "/Resources/Data/Json/Level.json";

    public static Dictionary<int, LevelData> ReadLevelData()
    {
        string jsonString = File.ReadAllText(filePath);
        Dictionary<int, LevelData> levelDataDic = new Dictionary<int, LevelData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData item in jsonData)
        {
            int levelID = int.Parse(item["ID"].ToString());
            LevelData levelData = new LevelData();

            levelData.skybox = "Skybox"+ levelID;
            levelData.normalPlanes = new List<string>();
            for(int i = 0; i < (int)item["NormalPlaneNum"]; i++)
            {
                levelData.normalPlanes.Add(levelID+"NormalGround"+(i+1));
            }
            levelData.normalSize =new float[2];
            int j = 0;
            foreach(JsonData t in item["NormalSize"])
            {
                levelData.normalSize[j++] =float.Parse(t.ToString());
            }            
            levelData.widthPlanes = new List<string>();
            for (int i = 0; i < (int)item["WidthPlaneNum"]; i++)
            {
                levelData.widthPlanes.Add(levelID + "WidthGround" + (i + 1));
            }
            levelData.widthSize = new float[2];
            j = 0;
            foreach (JsonData t in item["WidthSize"])
            {
                levelData.widthSize[j++] = float.Parse(t.ToString());
            }

            levelData.StageDatas = new List<StageData>();
            foreach(JsonData stageData in item["StageDatas"])
            {
                StageData t = new StageData();
                t.StageID = (int)stageData["StageID"];
                t.isSpecial = (bool)stageData["IsSpecial"];
                t.BOSSType = new List<CharacterType>();
                foreach(JsonData bossType in stageData["BossType"])
                {
                    t.BOSSType.Add((CharacterType)System.Enum.Parse(typeof(CharacterType), bossType.ToString()));
                }
                t.WaveEnemyNum = new List<int>();
                foreach(JsonData num in stageData["WaveEnemyNum"])
                {
                    t.WaveEnemyNum.Add((int)num);
                }
                t.WaveEnemyType = new List<CharacterType>();
                foreach(JsonData enemyType in stageData["WaveEnemyType"])
                {
                    t.WaveEnemyType.Add((CharacterType)System.Enum.Parse(typeof(CharacterType), enemyType.ToString()));
                }

                levelData.StageDatas.Add(t);
            }

            levelDataDic.Add(levelID, levelData);
        }
        return levelDataDic;
    }
}

#endregion