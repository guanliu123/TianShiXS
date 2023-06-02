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
    private GameObject topArea;
    private GameObject midArea;

    public StartPanel():base(new UIType(path))
    {
        
    }
    public override void OnEnter()
    {   
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        topArea = UITool.FindChildGameObject("TopArea", panel);
        midArea = UITool.FindChildGameObject("MidArea", panel);

        UITool.GetOrAddComponentInChildren<Button>("Role_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new RolePanel());
        });
        UITool.GetOrAddComponentInChildren<Button>("Handbook_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new HandbookPanel());
        });
        UITool.GetOrAddComponentInChildren<Button>("Level_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new LevelPanel());
        });
        UITool.GetOrAddComponentInChildren<Button>("StartGame_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().StartGame();
            PanelManager.Instance.Clear();
            PanelManager.Instance.Push(new GamePanel());
        });

        /*UITool.GetOrAddComponentInChildren<Button>("Audio_Btn", panel).onClick.AddListener(() =>
        {
            AudioListener.volume = Mathf.Abs(AudioListener.volume - 1);
        });*/

        UITool.GetOrAddComponentInChildren<Button>("Box_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new BoxPanel());
        });
        //UITool.GetOrAddComponentInChildren<Button>("Btn_Play", panel).onClick.AddListener(() =>
        //{
        //    SceneSystem.Instance.SetScene(new MainScene());
        //});
        //Button btn_Audio = UITool.GetOrAddComponentInChildren<Button>("Btn_Audio", panel);
        //btn_Audio.onClick.AddListener(() =>
        //{
        //    Image imgOpen = btn_Audio.transform.Find("Icon1").GetComponent<Image>();
        //    Image imgClose = btn_Audio.transform.Find("Icon2").GetComponent<Image>();
        //    if(imgOpen.enabled)
        //    {
        //        imgOpen.enabled = false;
        //        imgClose.enabled = true;
        //    }
        //    else
        //    {
        //        imgOpen.enabled = true;
        //        imgClose.enabled = false;
        //    }
        //});
    }

    public override void OnPause()
    {
        SetAreaActive(false);
    }

    public override void OnResume()
    {
        SetAreaActive(true);
    }
    private void SetAreaActive(bool isShow)
    {
        topArea.SetActive(isShow);
        midArea.SetActive(isShow);
    }

}


