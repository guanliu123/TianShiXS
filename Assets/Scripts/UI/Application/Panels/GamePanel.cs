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

    public Image energySlider1;
    public Image energySlider2;
    public Text levelText;
    public Text moneyText;
    public Text stageText;
    public Transform moneyLabel;
    public Vector3 moneyOrigin;
    public Slider hpSlider;
    //public Vector3 energyOrigin;

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
        moneyOrigin = moneyLabel.position;
        moneyText = UITool.GetOrAddComponentInChildren<Text>("MoneyText", panel);
        moneyText.text = GameManager.GetInstance().levelMoney + "";
        hpSlider = UITool.GetOrAddComponentInChildren<Slider>("HpSlider", panel);

        hpSlider.value = 1f;
        //energyOrigin = energySlider1.transform.position;
    }
    public override void OnEnter()
    {     
        GameObject panel = UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {

        });

        energySlider1 = UITool.GetOrAddComponentInChildren<Image>("EnergyBar1", panel);
        energySlider2 = UITool.GetOrAddComponentInChildren<Image>("EnergyBar2", panel);
        levelText = UITool.GetOrAddComponentInChildren<Text>("LevelText", panel);

        stageText = UITool.GetOrAddComponentInChildren<Text>("StageText", panel);

        Init(panel);

        /*UITool.GetOrAddComponentInChildren<Image>("EnergySlider1", panel).fillAmount.onValueChanged.AddListener((float value) =>
        {
            if (value >= 1)
            {
                GameManager.GetInstance().CallSkillPanel();
            }
        });    */
        UITool.GetOrAddComponentInChildren<Button>("Pause_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Push(new PausePanel());
            AudioManager.GetInstance().PlaySound("NormalButton");
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
        EnergyBarListener();
        StageListener();
        MoneyLabelTimer();
        MoneyListener();
        LevelTextListener();
        HpBarListener();
    }
    public void EnergyBarListener()
    {
        energySlider1.fillAmount = GameManager.GetInstance().playerEnergy / 100f;
        energySlider2.fillAmount = GameManager.GetInstance().playerEnergy / 100f;
        if (energySlider1.fillAmount >= 1)
        {
            GameManager.GetInstance().playerLevel++;
            GameManager.GetInstance().CallSkillPanel();
        }

        /*if (LevelManager.GetInstance().isChange&&!isenergyHide)
        {
            isenergyHide = true;
            energySlider.transform.DOMove(energySlider.transform.position + Vector3.up * 300f, 1f).OnComplete(() => { energySlider.gameObject.SetActive(false); });
        }
        else if(!LevelManager.GetInstance().isChange && isenergyHide)
        {
            energySlider.gameObject.SetActive(true);
            energySlider.transform.DOMove(energyOrigin, 1f);
            isenergyHide = false;
        }*/
    }

    public void HpBarListener()
    {
        //hpSlider.value = Player._instance.nowHP / Player._instance.maxHP;
        hpSlider.value = Mathf.Lerp(hpSlider.value, Player._instance.nowHP / Player._instance.maxHP, Time.deltaTime);
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
                moneyLabel.DOMove(moneyOrigin, 1f);          
            }

            hideTimer = 0;
            moneyText.text = GameManager.GetInstance().levelMoney + "";
        }
    }
    public void LevelTextListener()
    {
        levelText.text = GameManager.GetInstance().playerLevel+"";
    }
}
