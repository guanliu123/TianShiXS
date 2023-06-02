using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;
//using UnityEngine.UIElements;
using DG.Tweening;

public class GamePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/GamePanel";

    public Slider energySlider;
    public Text moneyText;
    public int lastMoney;
    public Transform moneyLabel;
    private float hideTime;
    private float hideTimer;
    private bool isHide;

    public GamePanel() : base(new UIType(path))
    {
    }
    private void Init(GameObject panel)
    {
        hideTime = 5f;
        hideTimer = 0f;
        isHide = false;

        lastMoney = GameManager.GetInstance().levelMoney;
        moneyLabel = UITool.GetOrAddComponentInChildren<Transform>("MoneyBG", panel);
        moneyText = UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel);
        moneyText.text = GameManager.GetInstance().levelMoney + "";
    }
    public override void OnEnter()
    {     
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        Init(panel);

        energySlider = UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel);
        UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel).onValueChanged.AddListener((float value) =>
        {
            if (value >= 1)
            {
                GameManager.GetInstance().CallSkillPanel();
            }
        });      
        
        UITool.GetOrAddComponentInChildren<Button>("Quit_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().QuitGame();
            PanelManager.Instance.Clear();
            PanelManager.Instance.Push(new StartPanel());
        });

        MonoManager.GetInstance().AddUpdateListener(GameUIEvent);
    }
    public override void OnExit()
    {
        base.OnExit();
        MonoManager.GetInstance().RemoveUpdeteListener(GameUIEvent);
    }

    public void GameUIEvent()
    {
        EnergySliderListener();
        MoneyLabelTimer();
        MoneyListener();
    }
    public void EnergySliderListener()
    {
        energySlider.value = GameManager.GetInstance().playerEnergy / 100f;
    }
    public void MoneyLabelTimer()
    {
        if (!isHide)
        {
            hideTimer += Time.deltaTime;
            if (hideTimer > hideTime)
            {
                moneyLabel.DOMove(moneyLabel.position+Vector3.left*500f,1f).OnComplete(() => { moneyLabel.gameObject.SetActive(false); });
                hideTimer = 0;
                isHide = true;
            }
        }
    }
    public void MoneyListener()
    {
        if (GameManager.GetInstance().levelMoney + "" != moneyText.text)
        {
            if (isHide)
            {
                moneyLabel.gameObject.SetActive(true);
                moneyLabel.DOMove(moneyLabel.position - Vector3.left * 500f, 1f);
                isHide = false;
            }

            hideTimer = 0;
            moneyText.text = GameManager.GetInstance().levelMoney + "";
        }
    }
}
