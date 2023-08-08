using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class AwaitPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/AwaitPanel";

    public AwaitPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
    }
}
