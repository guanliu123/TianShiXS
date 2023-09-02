using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//using Boo.Lang;

#region 资源数据的读写工具
public class ResourceDataTool
{
    //static string filePath = Application.dataPath + "/Resources/Data/Resource.json";
    static string filePath = Application.streamingAssetsPath + "/Data/Resource.json";

    public static Dictionary<ResourceType, (string, string)> ReadResourceData()
    {
        //string jsonString = File.ReadAllText(filePath);
        string jsonString = Resources.Load<TextAsset>("Data/Resource").text;
        //string jsonString="";
        //var handle = Addressables.LoadAssetAsync<TextAsset>("Data/Resource");
        /*if (!handle.IsDone) return null;

        if (handle.Status == AsyncOperationStatus.Succeeded) jsonString = handle.Result.text;*/

        Dictionary<ResourceType, (string, string)> resourceDataDic = new Dictionary<ResourceType, (string, string)>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData resourceDataJson in jsonData)
        {
            ResourceType resourceType = (ResourceType)System.Enum.Parse(typeof(ResourceType), resourceDataJson["ResourceType"].ToString());
            string resourcePath = resourceDataJson["ResourcePath"].ToString();
            string resourceSuffix = resourceDataJson["ResourceSuffix"].ToString();

            resourceDataDic.Add(resourceType, (resourcePath, resourceSuffix));
        }
        return resourceDataDic;
    }
}

#endregion

#region 玩家角色数据的读写工具
public static class RoleDataTool
{
    static string filePath = Application.streamingAssetsPath + "/Data/Role.json";
    static JsonData jsonData;
    public static Dictionary<int, CharacterMsg> roleMsgDic;
    public static Dictionary<int, CharacterData> roleDataDic;

    public static void ReadRoleData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Role").text;

        roleMsgDic = new Dictionary<int, CharacterMsg>();
        roleDataDic = new Dictionary<int,CharacterData>();
        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData characterDataJson in jsonData)
        {
            CharacterData characterData = new CharacterData();
            CharacterMsg characterMsg = new CharacterMsg();

            int characterID = (int)characterDataJson["RoleID"];
            //CharacterType characterType = (CharacterType)System.Enum.Parse(typeof(CharacterType), characterDataJson["CharacterType"].ToString());
            roleDataDic.Add(characterID, new CharacterData());

            characterMsg.name = characterDataJson["Name"].ToString();
            //characterMsg.image= ResourceManager.Instance.LoadByName<GameObject>("CharacterImage/"+characterID + "Image",ResourceType.UI);
            //ResourceManager.GetInstance().LoadByName<GameObject>("CharacterImage/" + characterID + "Image",result=> { characterMsg.image = result; }, ResourceType.UI);
            characterMsg.imagePath = "CharacterImage/" + characterID + "Image";
            characterMsg.describe= characterDataJson["Describe"].ToString(); 

            characterData.MaxHP = float.Parse(characterDataJson["MaxHP"].ToString());
            characterData.bulletList = new List<int>();
            foreach (JsonData bulletIDJson in characterDataJson["Bullets"])
            {
                characterData.bulletList.Add((int)bulletIDJson);
            }
            characterData.ATK = float.Parse(characterDataJson["ATK"].ToString());
            characterData.ATKSpeed = float.Parse(characterDataJson["ATKSpeed"].ToString());
            characterData.energy = 0;
            characterData.money = 0;

            roleDataDic[characterID]=characterData;
            roleMsgDic.Add(characterID, characterMsg);
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
        int characterID,
        //CharacterType characterType,
        float _hp = -1,
        List<int> _bullet = null,
        float _aggressivity = -1,
        float _atkspeed = -1
        )
    {
        for (int i = 0; i < jsonData.Count; i++)
        {
            JsonData characterDataJson = jsonData[i];
            if ((int)characterDataJson["RoleID"] == characterID)
            {
                if (_hp > 0) characterDataJson["MaxHP"] = float.Parse(characterDataJson["MaxHP"].ToString()) + _hp;
                if (_bullet != null)
                {
                    //赋值回去
                    foreach (var item in _bullet)
                    {
                        characterDataJson["bulletTypes"].Add(item.ToString());
                    }
                }
                if (_aggressivity > 0) characterDataJson["Aggressivity"] = float.Parse(characterDataJson["Aggressivity"].ToString()) + _aggressivity;
                if (_atkspeed > 0) characterDataJson["ATKSpeed"] = float.Parse(characterDataJson["ATKSpeed"].ToString()) + _atkspeed;
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
    static string filePath = Application.streamingAssetsPath + "/Data/Enemy.json";
    public static Dictionary<int, CharacterMsg> enemyMsgDic;
    public static Dictionary<int,  CharacterData> enemyDataDic;


    public static void ReadEnemyData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Enemy").text;


        enemyMsgDic = new Dictionary<int, CharacterMsg>();
        enemyDataDic = new Dictionary<int, CharacterData>();
        JsonData jsonData = JsonMapper.ToObject(jsonString);
        int characterID = -1;
        foreach (JsonData enemyDataJson in jsonData)
        {
            CharacterData enemyData = new CharacterData();
            CharacterMsg enemyMsg = new CharacterMsg();

            //int t = int.Parse(enemyDataJson["EnemyID"].ToString());
            /*if (t != 0)
            {
                characterID = t;
                enemyDataDic.Add(characterID, new Dictionary<int, CharacterData>());

                enemyMsg.name = enemyDataJson["Name"].ToString();
                enemyMsg.imagePath = "CharacterImage/" + characterID + "Image";

                enemyMsg.describe = enemyDataJson["Describe"].ToString();
                enemyMsgDic.Add(characterID, enemyMsg);
            }*/

            //int levelNum = int.Parse(enemyDataJson["LevelNum"].ToString());
            characterID = int.Parse(enemyDataJson["EnemyID"].ToString());
            enemyDataDic.Add(characterID,new CharacterData());

            enemyMsg.name = enemyDataJson["Name"].ToString();
            enemyMsg.imagePath = "CharacterImage/" + characterID + "Image";

            enemyMsg.describe = enemyDataJson["Describe"].ToString();
            enemyMsgDic.Add(characterID, enemyMsg);
            enemyData.MaxHP = float.Parse(enemyDataJson["MaxHP"].ToString());
            enemyData.bulletList = new List<int>();
            foreach (JsonData bulletIDJson in enemyDataJson["Bullets"])
            {
                enemyData.bulletList.Add((int)bulletIDJson);
            }
            enemyData.ATK = float.Parse(enemyDataJson["ATK"].ToString());
            enemyData.ATKSpeed = float.Parse(enemyDataJson["ATKSpeed"].ToString());
            //characterData.avoidance = float.Parse(characterDataJson["avoidance"].ToString());
            enemyData.energy = float.Parse(enemyDataJson["Energy"].ToString());
            enemyData.money = int.Parse(enemyDataJson["Money"].ToString());

            enemyDataDic[characterID] = enemyData;
        }
    }
}

#endregion

#region 子弹数据的读取工具

public static class BulletDataTool
{
    static string filePath = Application.streamingAssetsPath + "/Data/Bullet.json";

    public static Dictionary<int, BulletData> ReadBulletData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Bullet").text;

        Dictionary<int, BulletData> bulletDataDic = new Dictionary<int, BulletData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData item in jsonData)
        {
            //BulletType bulletType = (BulletType)System.Enum.Parse(typeof(BulletType), item["BulletType"].ToString());
            int bulletID = (int)item["BulletID"];
            BulletData bulletData = new BulletData();

            bulletData.transmissionFrequency = float.Parse(item["TransmissionFrequency"].ToString());
            bulletData.isRandomShoot = (bool)item["IsRandomShoot"];
            bulletData.existTime = float.Parse(item["ExistTime"].ToString());
            bulletData.moveSpeed = float.Parse(item["MoveSpeed"].ToString());
            bulletData.rotateSpeed = float.Parse(item["RotateSpeed"].ToString());
            bulletData.buffList = new List<int>();
            foreach (JsonData buff in item["BuffList"])
            {
                //Debug.Log((BuffType)System.Enum.Parse(typeof(BuffType), buffType.ToString()));
                //bulletData.buffList.Add((BuffType)System.Enum.Parse(typeof(BuffType), buff.ToString()));
                bulletData.buffList.Add((int)buff);
            }
            bulletData.evolvableList = new List<int>();
            foreach (JsonData buff in item["EvolvableList"])
            {
                //bulletData.evolvableList.Add((BuffType)System.Enum.Parse(typeof(BuffType), buffType.ToString()));
                bulletData.evolvableList.Add((int)buff);
            }
            bulletData.crit = float.Parse(item["Crit"].ToString());
            bulletData.critRate = float.Parse(item["CritRate"].ToString());
            bulletData.ATK = float.Parse(item["ATK"].ToString());
            bulletData.damageInterval = float.Parse(item["DamageInterval"].ToString());
            bulletData.isFollowShooter = (bool)item["IsFollowShooter"];
            bulletData.shootProbability = float.Parse(item["ShootProbability"].ToString());

            //bulletData.audio = ResourceManager.Instance.LoadByName<AudioClip>(bulletID+ "Audio",ResourceType.Audio);
            //ResourceManager.Instance.LoadRes<AudioClip>(bulletID + "Audio", result => { bulletData.audioPath = result; }, ResourceType.Audio);
            bulletData.audioPath = bulletID + "Audio";

            //bulletData.effect = ResourceManager.Instance.LoadByName<GameObject>(bulletID + "Effect", ResourceType.Effect);
            //ResourceManager.Instance.LoadRes<GameObject>(bulletID + "Effect", result => { bulletData.effectPath = result; }, ResourceType.Effect);
            bulletData.effectPath = bulletID + "Effect";

            bulletDataDic.Add(bulletID, bulletData);
        }
        return bulletDataDic;
    }
}

#endregion

#region 关卡数据读取工具

public static class LevelDataTool
{
    static string filePath = Application.streamingAssetsPath + "/Data/Level.json";

    public static Dictionary<int, LevelData> ReadLevelData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Level").text;
        Dictionary<int, LevelData> levelDataDic = new Dictionary<int, LevelData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        int levelID = -1;
        LevelData levelData = new LevelData();
        levelData.StageDatas = new List<StageData>();
        //List<StageData> stageDatas = new List<StageData>();
        foreach (JsonData item in jsonData)
        {
            int t = int.Parse(item["ID"].ToString());
            if (t != 0)
            {
                //将上一个数据存入
                if (levelID != -1&& !levelDataDic.ContainsKey(levelID))
                {
                    levelDataDic.Add(levelID, levelData);
                    levelData = new LevelData();
                }

                levelID = t;
                levelData.energy = (int)item["Energy"];

                levelData.skyboxName = "Skybox" + levelID;

                levelData.normalPlanes = new List<string>();
                for (int i = 0; i < (int)item["NormalPlaneNum"]; i++)
                {
                    levelData.normalPlanes.Add(levelID + "NormalGround" + (i + 1));
                }
                levelData.normalSize = new float[2] { 20, 10 };
                levelData.widthPlanes = new List<string>();
                for (int i = 0; i < (int)item["WidthPlaneNum"]; i++)
                {
                    levelData.widthPlanes.Add(levelID + "WidthGround" + (i + 1));
                }
                levelData.widthSize = new float[2] { 20, 40 };

                levelData.StageDatas = new List<StageData>();
            }

            StageData temp = new StageData();
            temp.StageID = (int)item["StageID"];
            temp.isSpecial = (bool)item["IsSpecial"];
            temp.BOSSList = new List<int>();
            foreach (JsonData boss in item["BossList"])
            {
                temp.BOSSList.Add((int)boss);
            }
            temp.WaveEnemyNum = new List<int>();
            foreach (JsonData num in item["WaveEnemyNum"])
            {
                temp.WaveEnemyNum.Add((int)num);
            }
            temp.WaveEnemyList = new List<int>();
            foreach (JsonData enemy in item["WaveEnemyList"])
            {                
                temp.WaveEnemyList.Add((int)enemy);
            }
            levelData.StageDatas.Add(temp);
        }
        if(!levelDataDic.ContainsKey(levelID)) levelDataDic.Add(levelID, levelData);

        return levelDataDic;
    }
}

#endregion

#region buff数据的读取工具

public static class BuffDataTool
{
    static string filePath = Application.streamingAssetsPath + "/Data/Buff.json";

    public static Dictionary<int, BuffData> ReadBuffData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Buff").text;


        Dictionary<int, BuffData> buffDataDic = new Dictionary<int, BuffData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData item in jsonData)
        {
            int buffID = (int)item["BuffID"];
            BuffData buffData = new BuffData();

            //buffData.audio = ResourceManager.Instance.LoadByName<AudioClip>(buffID+"Audio", ResourceType.Audio);
            //ResourceManager.Instance.LoadRes<AudioClip>(buffID + "Audio", result => { buffData.audioPath = result; }, ResourceType.Audio);
            buffData.audioPath = buffID + "Audio";

            //buffData.effect = ResourceManager.Instance.LoadByName<GameObject>(buffID + "Effect", ResourceType.Effect);
            //ResourceManager.Instance.LoadRes<GameObject>(buffID + "Effect", result => { buffData.effectPath = result; }, ResourceType.Effect);
            buffData.effectPath = buffID + "Effect";

            buffData.duration = float.Parse(item["Duration"].ToString());
            buffData.probability = float.Parse(item["Probability"].ToString());

            buffDataDic.Add(buffID, buffData);
        }
        return buffDataDic;
    }
}

#endregion

#region 技能数据的读取工具

public static class SkillDataTool
{
    static string filePath = Application.streamingAssetsPath + "/Data/Skill.json";

    public static Dictionary<int, SkillData> ReadSkillData()
    {
        string jsonString = Resources.Load<TextAsset>("Data/Skill").text;


        Dictionary<int, SkillData> skillDataDic = new Dictionary<int, SkillData>();

        JsonData jsonData = JsonMapper.ToObject(jsonString);
        foreach (JsonData item in jsonData)
        {
            SkillData skillData = new SkillData();

            skillData.id = (int)item["SkillID"];
            skillData.name = item["Name"].ToString();
            //skillData.icon = ResourceManager.Instance.LoadByName<Sprite>(skillData.name+"Icon",ResourceType.UI);

            skillData.iconPath = "Arts/UI/Icon/SkillIcon/" + skillData.id + "Icon";
            skillData.describe = item["Describe"].ToString();
            skillData.probability = float.Parse(item["Probability"].ToString());
            skillData.quality = item["Quality"].ToString() + "Quality";
            skillData.num = (int)item["Num"];
            skillData.beforeSkills = new List<int>();
            foreach (JsonData id in item["BeforeSkills"])
            {
                skillData.beforeSkills.Add((int)id);
            }

            skillDataDic.Add(skillData.id, skillData);
        }
        return skillDataDic;
    }
}

#endregion