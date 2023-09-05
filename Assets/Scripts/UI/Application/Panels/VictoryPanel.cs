using Abelkhan;
using Game;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/VictoryPanel";
    private int levelNum;
    public VictoryPanel(int levelNum) : base(new UIType(path))
    {
        this.levelNum = levelNum;
    }

    public override void OnEnter()
    {
        GameManager.GetInstance().ClearFloatDamage();
        if (levelNum>=GameManager.GetInstance().UserData.LevelNum)
        {
            RequestCenter.SetLevelReq(GameClient.Instance, GameManager.GetInstance().UserData.LevelNum + 1, (data) =>
            {
              GameManager.GetInstance().UserData = data;
            });
        }
        GameManager.GetInstance().QuitGame();

        GameObject panel = null;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel= obj;
            UITool.GetOrAddComponentInChildren<Button>("OK_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                //PanelManager.Instance.Pop();
                //PanelManager.Instance.Push(new StartPanel());
                GameRoot.Instance.TryLoad("StartScene", () =>
                {
                    SceneSystem.GetInstance().SetScene(new StartScene());
                });
            });
        });
    }
}
