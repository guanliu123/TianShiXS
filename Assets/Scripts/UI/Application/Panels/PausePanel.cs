using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/PausePanel";

    public PausePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        GameManager.GetInstance().ClearFloatDamage();

        Time.timeScale = 0;
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Continue_Btn", panel).onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Exit_Btn", panel).onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            GameManager.GetInstance().QuitGame();
            PanelManager.Instance.Clear();
            PanelManager.Instance.Push(new StartPanel());
        });
    }
}
