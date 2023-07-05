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
        panel = UIManager.Instance.GetSingleUI(UIType);
        topArea = UITool.FindChildGameObject("TopArea", panel);
        midArea = UITool.FindChildGameObject("MidArea", panel);

        UITool.GetOrAddComponentInChildren<Text>("StrengthText", topArea).text = GameManager.GetInstance()._UserData.Strength+" / "+100;
        UITool.GetOrAddComponentInChildren<Text>("MoneyText", topArea).text = GameManager.GetInstance()._UserData.Coin+"";

        Debug.Log(GameManager.GetInstance()._UserData.Strength);

        UITool.GetOrAddComponentInChildren<Button>("Role_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Push(new RolePanel());
        });
        UITool.GetOrAddComponentInChildren<Button>("Handbook_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Push(new HandbookPanel());
        });
        UITool.GetOrAddComponentInChildren<Button>("Level_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Push(new LevelPanel());
        });

        Toggle audiotog = UITool.GetOrAddComponentInChildren<Toggle>("Audio_Tog", panel);
        audiotog.isOn=AudioManager.GetInstance().soundValue<0.5f?true:false;
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
       
        UITool.GetOrAddComponentInChildren<Button>("StartGame_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            if (GameManager.Instance._UserData.Strength > 5)
            {
                GameManager.GetInstance().StartGame();
                PanelManager.Instance.Clear();
                PanelManager.Instance.Push(new GamePanel());
            }
        });


        //MonoManager.GetInstance().AddUpdateListener(StartUIEvent);
        /*UITool.GetOrAddComponentInChildren<Button>("Audio_Btn", panel).onClick.AddListener(() =>
        {
            AudioListener.volume = Mathf.Abs(AudioListener.volume - 1);
        });*/

        UITool.GetOrAddComponentInChildren<Button>("Box_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Push(new ChestPanel());
        });

        if (GameManager.GetInstance().nowLevel == 0)
        {
            UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "关卡选择";
            return;
        }
        UITool.GetOrAddComponentInChildren<Text>("LevelNum", panel).text = "第" + GameManager.GetInstance().nowLevel + "关";

    }
    public override void OnPause()
    {       
        SetAreaActive(false);
    }

    public override void OnResume()
    {
        SetAreaActive(true);
        //UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel).text = DataCenter.Money + "";
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


