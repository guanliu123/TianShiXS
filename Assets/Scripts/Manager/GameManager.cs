using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    //public static GameManager Instance { get; private set; }

    public float playerEnergy { get; private set; }
    public int playerMoney {get; private set;}

    public CharacterType nowPlayerType { get; private set; }

    public int existEnemy { get; private set; }

    public int existBOSS { get; private set; }

    private int evolutionNum = 0;

    public GameManager(){
        ChangeRole();
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
        player.transform.position = Vector3.zero;
        //playerObj.transform.position = Vector3.zero;
        playerObj.transform.parent = player.transform;

        LevelManager.GetInstance().Start();
        CameraMove(CameraPointType.MainPoint, 1f);
        CameraManager.GetInstance().StartCameraEvent();
    }
    public void QuitGame()
    {
        GameObject t=GameObject.FindGameObjectsWithTag("Player")[0];

            for (int i = 0; t != null && i < t.transform.childCount; i++)
            {
                var child = t.transform.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }

        LevelManager.GetInstance().Stop();
        GameObject.Destroy(t.GetComponent<Player>());
        GameObject.Destroy(t.GetComponent<TestController>());
        CameraManager.GetInstance().StopCameraEvent();
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

    public void EnemyDecrease()
    {
        existEnemy--;
        if (existEnemy <= 0)
        {
            LevelManager.GetInstance().WaveIncrease();
        }
    }

    public void EnemyIncrease()
    {
        existEnemy++;
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
        t.transform.position = Player._instance.transform.position;
        t.transform.parent = Player._instance.transform;
    }
}