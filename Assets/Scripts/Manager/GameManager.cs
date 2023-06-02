using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : BaseManager<GameManager>
{
    //public static GameManager Instance { get; private set; }
    public GameObject mainCanvas;

    public int nowLevel;

    public float playerEnergy { get; private set; }
    public int levelMoney {get; private set;}

    public CharacterType nowPlayerType { get; private set; }
    public int playerCount;
    public int enemyCount;
    public int skillCount;

    public List<GameObject> enemyList = new List<GameObject>();

    public int existBOSS { get; private set; }

    private int evolutionNum = 0;
    //private GameObject gameCanvas;

    //=====================所有会因为技能变化的公共数据==========================//
    public float critProbability;
    public float critRate;//子弹暴击倍率
    public int increaseMoney;
    public float increaseEnergy;

    public GameManager(){
        ChangeRole();
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        //gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
    }

    public List<CharacterDatas> GetPlayerRole()
    {
        List<CharacterDatas> players = new List<CharacterDatas>();

        foreach(var item in DataManager.GetInstance().characterSO.characterdatas)
        {
            if (item.characterData.tag == CharacterTag.Player) players.Add(item);
        }

        playerCount = players.Count;
        return players;
    }
    public List<CharacterDatas> GetEnemyRole()
    {
        List<CharacterDatas> enemys = new List<CharacterDatas>();
        foreach(var item in DataManager.GetInstance().characterSO.characterdatas)
        {
            if (item.characterData.tag == CharacterTag.Enemy) enemys.Add(item);
        }

        enemyCount = enemys.Count;
        return enemys;
    }
    public List<SkillDatas> GetSkills()
    {
        List<SkillDatas> skills = new List<SkillDatas>();
        foreach(var item in DataManager.GetInstance().skillSO.skilldatas)
        {
            skills.Add(item);
        }

        skillCount = skills.Count;
        return skills;
    }

    public void ChangeRole(CharacterType playerType=CharacterType.DaoShi)
    {
        nowPlayerType = playerType;
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevel = levelNum;
        LevelManager.GetInstance().ChangeLevel(levelNum);
    }

    public void StartGame()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        GameObject playerObj = PoolManager.GetInstance().GetObj(nowPlayerType.ToString());
        
        player.AddComponent<Player>().InitPlayer();
        player.AddComponent<PlayerController>();
        //player.AddComponent<TestController>();
        player.transform.GetChild(0).gameObject.SetActive(true);
        //player.transform.position = Vector3.zero+ Vector3.up; 
        player.transform.position = Vector3.zero + new Vector3(0,1,-1f);
        
        playerObj.transform.parent = player.transform;
        playerObj.transform.position = new Vector3(0, 0, -1);

        LevelManager.GetInstance().Start();
        CameraMove(CameraPointType.MainPoint, 1f);
        CameraManager.GetInstance().StartCameraEvent();

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
    }
    public void QuitGame()
    {
        GameObject t = GameObject.FindGameObjectsWithTag("Player")[0];
        t.transform.GetChild(0).gameObject.SetActive(false);

        for (int i = 1; t != null && i < t.transform.childCount; i++)
        {
            var child = t.transform.GetChild(i).gameObject;
            GameObject.Destroy(child);
        }

        for(int i = 0; i < enemyList.Count; i++)
        {
            GameObject.Destroy(enemyList[i]);
        }
        enemyList.Clear();

        LevelManager.GetInstance().Stop();
        MonoManager.GetInstance().ClearActions(); 
        t.GetComponent<Player>().ClearPlayer();
        GameObject.Destroy(t.GetComponent<Player>());
        GameObject.Destroy(t.GetComponent<PlayerController>());
        MonoManager.GetInstance().KillAllCoroutines();
        PoolManager.GetInstance().Clear();
        CameraMove(CameraPointType.OrginPoint, 1f);
        DataCenter.Money += levelMoney;
        PanelManager.Instance.Push(new FailPanel());
    }

    public void PlayerReset()
    {
        Player._instance.transform.DOMove(Vector3.zero+Vector3.up, 1f);
    }
    public void SwitchMode()
    {
        PlayerController t = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
            GameObject t = PoolManager.GetInstance().GetObj(PropType.Money.ToString());

            t.transform.position = point.position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
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
        GameObject obj = PoolManager.GetInstance().GetObj("FloatDamage");

        obj.transform.parent = mainCanvas.transform;
        
        obj.GetComponent<FloatDamage>().Init(point, damage, hpType);
    }

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
        GameObject t = PoolManager.GetInstance().GetObj(propType.ToString());
        t.transform.position = new Vector3(Player._instance.transform.position.x
            , Player._instance.transform.position.y-1
            , Player._instance.transform.position.z);
        t.transform.parent = Player._instance.transform;
    }
}