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

    public GameManager(){
        ChangeRole();
    }

    public void ChangeRole(CharacterType playerType=CharacterType.Player)
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

        player.AddComponent<Player>();
        player.AddComponent<TestController>();

        LevelManager.GetInstance().StartMapCreate();
        CameraMove(CameraPointType.MainPoint, 1f);
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
}