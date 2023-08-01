using Abelkhan;
using DG.Tweening;
using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Numerics;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonBase<GameManager>
{
    //public static GameManager Instance { get; private set; }
    public GameObject mainCanvas;

    private UserData userData = new UserData();

    public int nowLevel;

    public float playerEnergy { get; private set; }
    public int playerLevel;

    public int levelMoney {get; private set;}

    public int nowPlayerID { get; private set; }
    public int playerCount;
    public int enemyCount;
    public int skillCount;

    public List<GameObject> enemyList = new List<GameObject>();
    public List<GameObject> floatDamageList = new List<GameObject>();
    public List<GameObject> bulletList = new List<GameObject>();

    public int existBOSS { get; private set; }
    public UserData _UserData { get => userData; set => userData = value; }

    private int evolutionNum = 0;

    //=====================所有会因为技能变化的公共数据==========================//
    public float critProbability;
    public float critRate;//子弹暴击倍率
    public int increaseMoney;
    public float increaseEnergy;

    public GameManager(){
        ChangeRole();
        MonoManager.GetInstance().AddUpdateListener(() => {if(mainCanvas==null) mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas"); });
        nowPlayerID = 1001;
        
    }

    public Dictionary<int,CharacterMsg> GetPlayerRole()
    {
        Dictionary<int, CharacterMsg> players = new Dictionary<int, CharacterMsg>(CharacterManager.GetInstance().roleMsgDic);

        /*foreach(var item in DataManager.GetInstance().characterTagDic)
        {
            if (item.Value == CharacterTag.Player) players.Add(item.Key,DataManager.GetInstance().characterMsgDic[item.Key]);
        }*/
        
        playerCount = players.Count;
        return players;
    }
    public List<CharacterMsg> GetEnemyRole()
    {
        List<CharacterMsg> enemys = new List<CharacterMsg>();
        /*foreach(var item in DataManager.GetInstance().characterTagDic)
        {
            if (item.Value == CharacterTag.Enemy) enemys.Add(DataManager.GetInstance().characterMsgDic[item.Key]);
        }*/
        enemys = CharacterManager.GetInstance().enemyMagDic.Values.ToList<CharacterMsg>();

        enemyCount = enemys.Count;
        return enemys;
    }
    public List<SkillData> GetSkills()
    {
        List<SkillData> skills = new List<SkillData>();
        foreach(var item in SkillManager.SkillDic)
        {
            skills.Add(item.Value);
        }

        skillCount = skills.Count;
        return skills;
    }

    public void ChangeRole(int playerID=0)
    {
        nowPlayerID = playerID;
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevel = levelNum;
        LevelManager.GetInstance().ChangeLevel(levelNum);
    }

    public async void StartLoad()
    {
        //AsyncOperation sceneAsync= SceneSystem.Instance.SetScene(new LevelScene());
        await LevelManager.GetInstance().LoadLevelRes();
        //SceneSystem.GetInstance().SetScene(new LevelScene());       
    }



    public void StartGame()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        PoolManager.GetInstance().GetObj(nowPlayerID.ToString(), res => {
            res.transform.parent = player.transform;
            res.transform.position = new Vector3(0, 0, -1);
        }, ResourceType.Player);

        player.AddComponent<Player>().InitPlayer();
        player.AddComponent<PlayerController>();
        //player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.position = Vector3.zero + new Vector3(0, 1, -1f);

        LevelManager.GetInstance().Start();
        CameraMove(CameraPointType.MainPoint, 1f);
        CameraManager.GetInstance().StartCameraEvent();

        //RequestCenter.CostStrengthReq(GameClient.Instance, 5, (data) =>
        //{
        //    _UserData = data;
        //});

        Init();
    }

    public void Init()
    {
        critProbability=0;
        critRate=0;
        increaseMoney=0;
        increaseEnergy = 0;

        evolutionNum = 0;
        playerEnergy = 0;
        levelMoney = 0;
        playerLevel = 1;

    }
    public void QuitGame()
    {
        GameObject player = Player._instance.gameObject;
        //player.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 0; player != null && i < player.transform.childCount; i++)
        {
            var child = player.transform.GetChild(i).gameObject;
            GameObject.Destroy(child);
        }

        for(int i = 0; i < enemyList.Count; i++)
        {
            GameObject.Destroy(enemyList[i]);
        }
        enemyList.Clear();
        
        BulletManager.GetInstance().Reset();
        PoolManager.GetInstance().Reset();
        LevelManager.GetInstance().Reset();
        MonoManager.GetInstance().Reset();
        SkillManager.GetInstance().Reset();
        Player._instance.ResetRole();
        GameObject.Destroy(player.GetComponent<Player>());
        GameObject.Destroy(player.GetComponent<PlayerController>());
        CameraMove(CameraPointType.OrginPoint, 1f);
        WriteData();
        //DataCenter.Money += levelMoney;

        //RequestCenter.AddCoinReq(GameClient.Instance, levelMoney, (data) =>
        //{
        //    userData = data;
        //});
    }

    public void WriteData()
    {

    }

    public void ClearFloatDamage()
    {
        for(int i = 0; i < floatDamageList.Count; i++)
        {
            if (floatDamageList[i] != null) PoolManager.GetInstance().PushObj("FloatDamage", floatDamageList[i]);
        }
        floatDamageList.Clear();
    }
    public void PlayerReset()
    {
        Player._instance.transform.DOMove(Vector3.zero+Vector3.up+Vector3.back, 1f);
    }
    public void LockMove()
    {
        PlayerController t = Player._instance.gameObject.GetComponent<PlayerController>();
        t.canMove = false;
        //MonoManager.GetInstance().AddUpdateListener(ControllerLock);
    }
    public void UnlockMove()
    {
        //MonoManager.GetInstance().RemoveUpdeteListener(ControllerLock);
        PlayerController t = Player._instance.gameObject.GetComponent<PlayerController>();
        t.canMove = true;
    }

    private void ControllerLock()
    {
        PlayerController t = Player._instance.gameObject.GetComponent<PlayerController>();
        t.canMove = false;
    }

    public void SwitchMode()
    {
        PlayerController t = Player._instance.gameObject.GetComponent<PlayerController>();
        t.SwitchMode();
    }

    public void CameraMove(CameraPointType cameraPointType,float moveTime)
    {
        CameraManager.GetInstance().CameraMove(cameraPointType, moveTime);
    }

    public void ChangeEnergy(float num)
    {
        playerEnergy += (num+increaseEnergy);
        if (playerEnergy < 0) playerEnergy = 0;
    }

    public void FallMoney(Transform point, int num)
    {
        for (int i = 0; i < num + increaseMoney; i++)
        {
            PoolManager.GetInstance().GetObj(PropType.Money.ToString(), res =>
            {
                res.transform.position = point.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            },ResourceType.Prop);
        }
    }
    public void ChangeMoney(int money)
    {
        levelMoney += money;
    }

    public void CallSkillPanel()
    {
        PanelManager.Instance.Push(new SkillPanel());
        Time.timeScale = 0f;
        ChangeEnergy(-playerEnergy);
    }

    public void EnemyIncrease(GameObject obj)
    {
        enemyList.Add(obj);
    }
    public void EnemyDecrease(GameObject obj)
    {
        enemyList.Remove(obj);
        if (enemyList.Count <= 0)
        {
            LevelManager.GetInstance().WaveIncrease();
        }
    }

    public void ShowDamage(Transform point,float damage,HPType  hpType)
    {
        PoolManager.GetInstance().GetObj("FloatDamage", obj =>
        {
            obj.transform.parent = mainCanvas.transform;

            obj.GetComponent<FloatDamage>().Init(point, damage, floatDamageList, hpType);
        },ResourceType.UI);        
    }
    public void GenerateEffect(Transform insTransform, string effectName, bool withTra = false, float existTime = 0f)
    {
        PoolManager.GetInstance().GetObj(effectName, t =>
        {
            if (!t) return;

            ObjTimer timer = t.GetComponent<ObjTimer>();
            if (!timer) timer = t.AddComponent<ObjTimer>();

            timer.Init(effectName, existTime);
            t.transform.position = insTransform.position;
            t.transform.rotation = insTransform.rotation;
            if (withTra) t.transform.SetParent(insTransform);
        }, ResourceType.Effect);
    }
    /*public void GenerateEffect(Transform insTransform,GameObject effect,bool withTra=false,float existTime = 0f)
    {
        if (!effect) return;
        string effectName = effect.name;
        GameObject t = PoolManager.GetInstance().GetObj(effectName, effect);

        if (!t) return;

        ObjTimer timer = t.GetComponent<ObjTimer>();
        if (!timer) timer = t.AddComponent<ObjTimer>();

        timer.Init(effectName, existTime);
        t.transform.position = insTransform.position;
        t.transform.rotation = insTransform.rotation;
        if (withTra) t.transform.SetParent(insTransform);
    }*/
    /*public void GenerateEffect(Transform insTransform, string effectName, float existTime = 0f)
    {
        PoolManager.GetInstance().GetObj(effectName,t=>
        {
            if (!t)
            {
                ObjTimer timer = t.GetComponent<ObjTimer>();
                if (!timer) timer = t.AddComponent<ObjTimer>();

                timer.Init(effectName, existTime);
                t.transform.position = insTransform.position;
                t.transform.rotation = insTransform.rotation;
            }
        },ResourceType.Effect);       
    }*/

    public void PlayerEvolution()
    {
        switch (evolutionNum)
        {
            case 0:AddEvolutionProp(PropType.HuLu);break;
            case 1:AddEvolutionProp(PropType.DaoQi);break;
            case 2:AddEvolutionProp(PropType.Bell);break;
            case 3:AddEvolutionProp(PropType.Banners);break;
            case 4:AddEvolutionProp(PropType.Sword);break;

        }
        evolutionNum++;
    }

    public void AddEvolutionProp(PropType propType)
    {
        PoolManager.GetInstance().GetObj(propType.ToString(), t =>
        {
            t.transform.position = new Vector3(Player._instance.transform.position.x
            , Player._instance.transform.position.y - 1
            , Player._instance.transform.position.z);
            t.transform.parent = Player._instance.transform;
        },ResourceType.Prop);       
    }
}