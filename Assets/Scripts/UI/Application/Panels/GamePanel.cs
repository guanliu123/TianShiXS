using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;

public class GamePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/GamePanel";
    public Slider energySlider;

    public GamePanel() : base(new UIType(path))
    {
    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);

        energySlider = UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel);
        UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel).onValueChanged.AddListener((float value) =>
        {
            if (value >= 1)
            {
                PanelManager.Instance.Push(new SkillPanel());
                Time.timeScale = 0f;
                Taoist_priest._instance.nowEnergy = 0f;
            }
        });

        MonoManager.GetInstance().AddUpdateListener(GameUIEvent);
    }
    public void GameUIEvent()
    {
        EnergySliderListener();
    }
    public void EnergySliderListener()
    {
        energySlider.value = Taoist_priest._instance.nowEnergy / 100f;
    }
}
