using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/LoadingPanel";
    public LoadingPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = null;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;           
        });
    }
}
