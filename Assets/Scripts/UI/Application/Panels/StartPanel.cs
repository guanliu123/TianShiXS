using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using Unity;

public class StartPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/StartScenePanel";
    GameObject panel;
    private GameObject topArea;
    private GameObject midArea;

    public StartPanel():base(new UIType(path))
    {
        
    }
    public override void OnEnter()
    {
        UIManager.Instance.GetSingleUI(UIType,(obj)=>
        {
            panel = obj;
            if(obj.activeSelf)
            {
                topArea = UITool.FindChildGameObject("TopArea", obj);
                midArea = UITool.FindChildGameObject("MidArea", obj);

                //UITool.GetOrAddComponentInChildren<Text>("StrengthText", topArea).text = GameManager.GetInstance()._UserData.Strength + " / " + 100;
                //UITool.GetOrAddComponentInChildren<Text>("MoneyText", topArea).text = GameManager.GetInstance()._UserData.Coin + "";

                //Debug.Log(GameManager.GetInstance()._UserData.Strength);

                UITool.GetOrAddComponentInChildren<Button>("Role_Btn", obj).onClick.AddListener(() =>
                {
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    PanelManager.Instance.Push(new RolePanel());
                });
                UITool.GetOrAddComponentInChildren<Button>("Handbook_Btn", obj).onClick.AddListener(() =>
                {
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    PanelManager.Instance.Push(new HandbookPanel());
                });
                UITool.GetOrAddComponentInChildren<Button>("Level_Btn", obj).onClick.AddListener(() =>
                {
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    PanelManager.Instance.Push(new LevelPanel());
                });

                Toggle audiotog = UITool.GetOrAddComponentInChildren<Toggle>("Audio_Tog", obj);
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

                UITool.GetOrAddComponentInChildren<Button>("StartGame_Btn", obj).onClick.AddListener(async () =>
                {
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    //if (GameManager.Instance._UserData.Strength > 5)
                    //PanelManager.GetInstance().Push(new LoadingPanel());
                    await LevelManager.GetInstance().LoadLevelRes();
                    GameRoot.Instance.TryLoad(LevelScene.sceneName, () =>
                    {
                        SceneSystem.GetInstance().SetScene(new LevelScene());
                        GameManager.GetInstance().StartGame();
                    });
                    
                    
                });

                UITool.GetOrAddComponentInChildren<Button>("Box_Btn", obj).onClick.AddListener(() =>
                {
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    PanelManager.Instance.Push(new ChestPanel());
                });

                if (GameManager.GetInstance().nowLevel == 0)
                {
                    UITool.GetOrAddComponentInChildren<Text>("LevelNum", obj).text = "关卡选择";
                    return;
                }
                UITool.GetOrAddComponentInChildren<Text>("LevelNum", obj).text = "第" + GameManager.GetInstance().nowLevel + "关";
            }
        });
        GameRoot.Instance.PreLoadAllAssets("gameing");


        //MonoManager.GetInstance().AddUpdateListener(StartUIEvent);
        /*UITool.GetOrAddComponentInChildren<Button>("Audio_Btn", panel).onClick.AddListener(() =>
        {
            AudioListener.volume = Mathf.Abs(AudioListener.volume - 1);
        });*/
    }
    public override void OnPause()
    {       
        SetAreaActive(false);
    }

    public override void OnResume()
    {
        SetAreaActive(true);
        //UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel).text = DataCenter.Money + "";
        Debug.Log(panel.name);
        if (GameManager.GetInstance().nowLevel == 0)
        {
            UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "关卡选择";
            return;
        }
        UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "第" + GameManager.GetInstance().nowLevel + "关";
    }
    private void SetAreaActive(bool isShow)
    {
        topArea.SetActive(isShow);
        midArea.SetActive(isShow);
    }
}


