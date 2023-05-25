using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : BaseManager<GameManager>
{
    //public static GameManager Instance { get; private set; }
    public GameObject mainCanvas;
    public float playerEnergy { get; private set; }
    public int playerMoney {get; private set;}

    public CharacterType nowPlayerType { get; private set; }
    public List<GameObject> enemyList = new List<GameObject>();

    public int existBOSS { get; private set; }

    private int evolutionNum = 0;

    private Dictionary<HPType, Color> damageColor = new Dictionary<HPType, Color>();
    //private GameObject gameCanvas;

    //=====================所有会因为技能变化的公共数据==========================//
    public float critProbability;
    public float critRate;//子弹暴击倍率

    public GameManager(){
        ChangeRole();
        damageColor.Add(HPType.Default, Color.red);
        damageColor.Add(HPType.Burn, Color.yellow);
        mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas");
        //gameCanvas = GameObject.FindGameObjectWithTag("GameCanvas");
    }

    public void ChangeRole(CharacterType playerType=CharacterType.DaoShi)
    {
        nowPlayerType = playerType;
    }

    public void ChangeLevel(int levelNum)
    {
        LevelManager.GetInstance().ChangeLevel(levelNum);
    }

    public void StartGame()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        GameObject playerObj = PoolManager.GetInstance().GetObj(nowPlayerType.ToString());
        
        player.AddComponent<Player>();
        //player.AddComponent<PlayerController>();
        player.AddComponent<TestController>();
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.position = Vector3.zero+Vector3.up;
        //playerObj.transform.position = Vector3.zero;
        playerObj.transform.parent = player.transform;

        LevelManager.GetInstance().Start();
        CameraMove(CameraPointType.MainPoint, 1f);
        CameraManager.GetInstance().StartCameraEvent();
    }
    public void QuitGame()
    {
        GameObject t = GameObject.FindGameObjectsWithTag("Player")[0];

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
        GameObject.Destroy(t.GetComponent<Player>());
        GameObject.Destroy(t.GetComponent<TestController>());
        CameraManager.GetInstance().StopCameraEvent();
        MonoManager.GetInstance().KillAllCoroutines();
        CameraMove(CameraPointType.OrginPoint, 1f);
    }

    public void PlayerReset()
    {
        Player._instance.transform.DOMove(Vector3.zero, 1f);
    }

    public void CameraMove(CameraPointType cameraPointType,float moveTime)
    {
        CameraManager.GetInstance().CameraMove(cameraPointType, moveTime);
    }

    public void ChangeEnergy(float num)
    {
        playerEnergy += num;
        if (playerEnergy < 0) playerEnergy = 0;
    }
    public void ChangeMoney(int money)
    {
        playerMoney += money;
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

        //Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
        obj.transform.parent = mainCanvas.transform;
        //obj.transform.position = point.position+randomOffset;
        
        obj.GetComponent<FloatDamage>().Init(point, damage, hpType);

        //float posY = obj.transform.position.y + 1f;
        //obj.transform.DOMoveY(posY, 1f).OnComplete(() => { PoolManager.GetInstance().PushObj("FloatDamage", obj); });
    }

    /*public List<SkillUpgrade> RandomSkill()
    {
        
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
        GameObject t = PoolManager.GetInstance().GetObj(propType.ToString());
        t.transform.position = Player._instance.transform.position;
        t.transform.parent = Player._instance.transform;
    }
}