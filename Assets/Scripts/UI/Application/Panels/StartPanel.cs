using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Unity;
using UnityEngine.SceneManagement;

public class StartPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/StartScenePanel";
    private static GameObject panel = null;
    private GameObject topArea;
    private GameObject midArea;

    public StartPanel():base(new UIType(path))
    {
        
    }
    public override void OnEnter()
    {
        GameObject _panel = UIManager.Instance.GetSingleUI(UIType);
        panel = _panel;
        if(_panel.activeSelf)
        {
            topArea = UITool.FindChildGameObject("TopArea", _panel);
            midArea = UITool.FindChildGameObject("MidArea", _panel);

            FlushStrAndCoin();

            UITool.GetOrAddComponentInChildren<Button>("Role_Btn", _panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Push(new RolePanel());
            });
            UITool.GetOrAddComponentInChildren<Button>("Handbook_Btn", _panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Push(new HandbookPanel());
            });
            UITool.GetOrAddComponentInChildren<Button>("Level_Btn", _panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Push(new LevelPanel());
            });

            Toggle audiotog = UITool.GetOrAddComponentInChildren<Toggle>("Audio_Tog", _panel);
            audiotog.isOn = AudioManager.GetInstance().soundValue < 0.5f ? true : false;
            audiotog.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    AudioManager.GetInstance().soundValue = 0;
                    AudioManager.GetInstance().bkValue = 0;
                }
                else
                {
                    AudioManager.GetInstance().soundValue = 1;
                    AudioManager.GetInstance().bkValue = 1;
                    AudioManager.GetInstance().PlaySound("NormalButton");
                }
            });

            UITool.GetOrAddComponentInChildren<Button>("StartGame_Btn", _panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");

                Debug.Log("StartGame_Btn onClick!");
                var loadLevelTask = LevelManager.GetInstance().LoadLevelRes();
                Debug.Log("StartGame_Btn begin LoadLevelRes!");
                GameRoot.Instance.TryLoad(LevelScene.sceneName, async () =>
                {
                    Debug.Log("TryLoad LevelScene!");
                    await loadLevelTask;
                    Debug.Log("TryLoad LevelScene down!");
                    SceneSystem.GetInstance().SetScene(new LevelScene());
                    Debug.Log("SetScene LevelScene down!");
                    GameManager.GetInstance().StartGame();
                    Debug.Log("StartGame!");
                });

            });

            UITool.GetOrAddComponentInChildren<Button>("Box_Btn", _panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Push(new ChestPanel());
            });

            if (GameManager.GetInstance().nowLevel == 0)
            {
                UITool.GetOrAddComponentInChildren<Text>("LevelNum", _panel).text = "关卡选择";
                return;
            }
            UITool.GetOrAddComponentInChildren<Text>("LevelNum", _panel).text = "第" + GameManager.GetInstance().nowLevel + "关";
        }

    }
    public override void OnPause()
    {       
        SetAreaActive(false);
    }

    public override void OnResume()
    {
        SetAreaActive(true);

        if (panel != null)
        {
            Debug.Log(panel.name);
            if (GameManager.GetInstance().nowLevel == 0)
            {
                UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "关卡选择";
                return;
            }
            UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "第" + GameManager.GetInstance().nowLevel + "关";

            UITool.GetOrAddComponentInChildren<Text>("StrengthText", panel).text = "" + GameManager.Instance.UserData.Strength + "/100";
            UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel).text = "" + GameManager.Instance.UserData.Coin;
        }
        
    }
    private void SetAreaActive(bool isShow)
    {
        topArea.SetActive(isShow);
        midArea.SetActive(isShow);
    }

    public static void FlushStrAndCoin()
    {
        Debug.Log("StartPanel FlushStr");
        if (panel != null)
        {
            Debug.Log("StartPanel FlushStr set!");
            UITool.GetOrAddComponentInChildren<Text>("StrengthText", panel).text = "" + GameManager.Instance.UserData.Strength + "/100";
            UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel).text = "" + GameManager.Instance.UserData.Coin;
        }
    }
}


