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

                var taskLoad = LevelManager.GetInstance().LoadLevelRes();
                GameRoot.Instance.TryLoad(LevelScene.sceneName, taskLoad , () =>
                {
                    SceneSystem.GetInstance().SetScene(new LevelScene());
                    GameManager.GetInstance().StartGame();
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
        Debug.Log($"StartPanel OnResume!");

        if (panel != null)
        {
            FlushStrAndCoin();

            Debug.Log($"StartPanel OnResume panel.name:{panel.name}");
            if (GameManager.GetInstance().nowLevel == 0)
            {
                var tLevelNum = UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel);
                if (tLevelNum != null)
                {
                    tLevelNum.text = "关卡选择";
                }
                return;
            }
            var tLevelNum1 = UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel);
            if (tLevelNum1 != null)
            {
                tLevelNum1.text = "第" + GameManager.GetInstance().nowLevel + "关";
            }
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

            try
            {
                var tStrengthText = UITool.GetOrAddComponentInChildren<Text>("StrengthText", panel);
                if (tStrengthText != null)
                {
                    tStrengthText.text = "" + GameManager.Instance.UserData.Strength + "/100";
                }

                var tMoneyText = UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel);
                if (tMoneyText != null)
                {
                    tMoneyText.text = "" + GameManager.Instance.UserData.Coin;
                }
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}


