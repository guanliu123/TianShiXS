using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;

public class GameManager : BaseManager<GameManager>
{
    public static GameManager Instance { get; private set; }

    private Dictionary<CameraPointType, Vector3> cameraPos = new Dictionary<CameraPointType, Vector3>();
    private Dictionary<CameraPointType, Vector3> cameraRot = new Dictionary<CameraPointType, Vector3>();

    public float playerEnergy { get; private set; }

    public GameManager(){
        cameraPos.Add(CameraPointType.MainPoint, new Vector3(0, 4.3f, -7.15f));
        cameraRot.Add(CameraPointType.MainPoint, new Vector3(11.75f, 0, 0));
    }

    public void ChangeLevel(int levelNum)
    {
        MapManager.GetInstance().ChangeLevel(levelNum);
    }

    public void StartGame()
    {
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

        player.AddComponent<Taoist_priest>();
        player.AddComponent<TestController>();

        MapManager.GetInstance().StartMapCreate();
        CameraMove(CameraPointType.MainPoint, 1f);
    }

    private void CameraMove(CameraPointType cameraPointType,float moveTime)
    {
        Camera mainCamera = Camera.main;
        mainCamera.transform.DOMove(cameraPos[cameraPointType], moveTime);
        mainCamera.transform.DORotate(cameraRot[cameraPointType], moveTime);
    }

    public void ChangeEnergy(float num)
    {
        playerEnergy += num;
        if (playerEnergy < 0) playerEnergy = 0;
    }
    public void CallSkillPanel()
    {
        PanelManager.Instance.Push(new SkillPanel());
        Time.timeScale = 0f;
        ChangeEnergy(-playerEnergy);
    }
}