using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/BagPanel";
    public BagPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        //UITool.GetOrAddComponentInChildren<Button>("Open_Btn", panel).onClick.AddListener(() => { DataCenter.Money += 100; });
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
            AudioManager.GetInstance().PlaySound("NormalButton");
        });
    }
}
