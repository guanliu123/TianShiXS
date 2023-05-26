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
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        
        UITool.GetOrAddComponentInChildren<Button>("Back_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new StartPanel());
        });
    }
}
