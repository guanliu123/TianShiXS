using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/FailPanel";
    public FailPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameManager.GetInstance().ClearFloatDamage();
        GameManager.GetInstance().QuitGame();
        GameObject panel = UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {

        });

        UITool.GetOrAddComponentInChildren<Button>("Back_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new StartPanel());
        });
    }
}
