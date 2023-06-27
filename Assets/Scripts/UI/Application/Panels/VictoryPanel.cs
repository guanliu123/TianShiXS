using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/VictoryPanel";
    public VictoryPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameManager.GetInstance().ClearFloatDamage();
        GameManager.GetInstance().QuitGame();
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        UITool.GetOrAddComponentInChildren<Button>("OK_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new StartPanel());
        });
    }
}
