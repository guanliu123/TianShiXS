﻿using StarkSDKSpace;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/LoadPanel";

    public LoadPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel= UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponent<Button>(panel).onClick.AddListener(() =>
        {
            GameRoot.Instance.StartGame();
        });
    }

    public override void OnPause()
    {
       
    }

    public override void OnResume()
    {
        
    }

    public override void OnExit()
    {
        
    }

    
}
