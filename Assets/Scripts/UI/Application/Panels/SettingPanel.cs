﻿using System;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;



public class SettingPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/SettingPanel";
    public SettingPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = null;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel= obj;
            UITool.GetOrAddComponentInChildren<Button>("Btn_Close", panel).onClick.AddListener(() =>
            {

                PanelManager.Instance.Pop();
            });
        });
        
    }

}

