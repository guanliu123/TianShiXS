using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
using UnityEngine.UI;
using System.Reflection;

public class LevelPanel : BasePanel {
    private static readonly string path = "Prefabs/Panels/LevelPanel";

    public LevelPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        for(int i = 1; i <= 1; i++)
        {
            AddListener(UITool.GetOrAddComponentInChildren<Button>("Level_Btn"+i,panel), i);
        }
    }
    void AddListener(Button button, int parameter)
    {
        button.onClick.AddListener(delegate {
            GameManager.GetInstance().ChangeLevel(parameter);
        });
    }
}
