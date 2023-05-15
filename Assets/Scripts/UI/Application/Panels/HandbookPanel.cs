using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class HandbookPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/HandbookPanel";

    public HandbookPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
    }
}
