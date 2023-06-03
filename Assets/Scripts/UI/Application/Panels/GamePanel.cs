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
    public Text stageText;
    public Transform moneyLabel;

    private float hideTime;
    private float hideTimer;
    private bool ismoneyHide;
    private bool isenergyHide;

    public GamePanel() : base(new UIType(path))
    {
    }
    private void Init(GameObject panel)
    {
        hideTime = 5f;
        hideTimer = 0f;
        ismoneyHide = false;
        isenergyHide = false;

        moneyLabel = UITool.GetOrAddComponentInChildren<Transform>("MoneyBG", panel);
        moneyText = UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel);
        moneyText.text = GameManager.GetInstance().levelMoney + "";
    }
    public override void OnEnter()
    {     
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        Init(panel);

        energySlider = UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel);
        stageText = UITool.GetOrAddComponentInChildren<Text>("StageText", panel);

        UITool.GetOrAddComponentInChildren<Slider>("EnergySlider", panel).onValueChanged.AddListener((float value) =>
        {
            if (value >= 1)
            {
                GameManager.GetInstance().CallSkillPanel();
            }
        });      
        
        UITool.GetOrAddComponentInChildren<Button>("Pause_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new PausePanel());
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
        StageListener();
        MoneyLabelTimer();
        MoneyListener();
    }
    public void EnergySliderListener()
    {
        energySlider.value = GameManager.GetInstance().playerEnergy / 100f;
        if (LevelManager.GetInstance().isChange&&!isenergyHide)
        {
            isenergyHide = true;
            energySlider.transform.DOMove(energySlider.transform.position + Vector3.up * 300f, 1f).OnComplete(() => { energySlider.gameObject.SetActive(false); });
        }
        else if(!LevelManager.GetInstance().isChange && isenergyHide)
        {
            energySlider.gameObject.SetActive(true);
            energySlider.transform.DOMove(energySlider.transform.position - Vector3.up * 300, 1f);
            isenergyHide = false;
        }
    }
    public void StageListener()
    {
        stageText.text ="阶段：" + (LevelManager.GetInstance().nowStage+1)+"/"+LevelManager.GetInstance().maxStage;
    }
    public void MoneyLabelTimer()
    {
        if (!ismoneyHide)
        {
            hideTimer += Time.deltaTime;
            if (hideTimer > hideTime)
            {
                moneyLabel.DOMove(moneyLabel.position+Vector3.left*500f,1f).OnComplete(() => { moneyLabel.gameObject.SetActive(false); });
                hideTimer = 0;
                ismoneyHide = true;
            }
        }
    }
    public void MoneyListener()
    {
        if (GameManager.GetInstance().levelMoney + "" != moneyText.text)
        {
            if (ismoneyHide)
            {
                moneyLabel.gameObject.SetActive(true);
                ismoneyHide = false;
                moneyLabel.DOMove(moneyLabel.position - Vector3.left * 500f, 1f);          
            }

            hideTimer = 0;
            moneyText.text = GameManager.GetInstance().levelMoney + "";
        }
    }
}
