﻿using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class LoadingPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/LoadingPanel";

    public static float PercentComplete = 0;
    public Slider slider;
    public LoadingPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        PercentComplete = 0;
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        
        slider = UITool.GetOrAddComponentInChildren<Slider>("Slider", panel);

        MonoManager.Instance.AddUpdateListener(() =>
        {
            slider.value = PercentComplete;
        });
    }
 
 }

