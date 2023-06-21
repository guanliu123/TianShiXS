using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class SuccessPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/SuccessPanel";
    public SuccessPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameManager.GetInstance().ClearFloatDamage();
        GameManager.GetInstance().QuitGame();
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("Back_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new StartPanel());
        });
    }
}
