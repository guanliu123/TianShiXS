using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;

public class GamePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/GamePanel";

    public Slider energySlider;
    public Text moneyText;

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
                GameManager.GetInstance().CallSkillPanel();
            }
        });
        moneyText = UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel);
        UITool.GetOrAddComponentInChildren<Button>("Quit_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().QuitGame();
            PanelManager.Instance.Clear();
            PanelManager.Instance.Push(new StartPanel());
        });

        MonoManager.GetInstance().AddUpdateListener(GameUIEvent);
    }
    public void GameUIEvent()
    {
        EnergySliderListener();
        MoneyListener();
    }
    public void EnergySliderListener()
    {
        energySlider.value = GameManager.GetInstance().playerEnergy / 100f;
    }
    public void MoneyListener()
    {
        moneyText.text = GameManager.GetInstance().levelMoney + "";
    }
}
