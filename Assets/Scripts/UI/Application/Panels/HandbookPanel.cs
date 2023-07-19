using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class HandbookPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/HandbookPanel";
    GameObject panel;

    GameObject enemyArea;  
    GameObject enemyList;
    List<CharacterMsg> enemys;

    GameObject skillArea;
    GameObject skillList;
    List<SkillData> skills;
    
    IDragButton dragButton;

    public HandbookPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {

        enemys = GameManager.GetInstance().GetEnemyRole();
        skills = GameManager.GetInstance().GetSkills();

        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;
            UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Pop();

                enemyArea = UITool.GetOrAddComponentInChildren<Transform>("Enemy", panel).gameObject;
                enemyList = UITool.GetOrAddComponentInChildren<Transform>("EnemyPortrait_List", panel).gameObject;
                enemyList.GetComponent<EnemyPortraitList>()._panel = this;

                skillArea = UITool.GetOrAddComponentInChildren<Transform>("Skill", panel).gameObject;
                skillList = UITool.GetOrAddComponentInChildren<Transform>("SkillPortrait_List", panel).gameObject;
                skillList.GetComponent<SkillPortraitList>()._panel = this;
                skillArea.SetActive(false);
                dragButton = enemyList.GetComponent<IDragButton>();

                UITool.GetOrAddComponentInChildren<Button>("Enemy_Btn", panel).onClick.AddListener(() =>
                {
                    enemyArea.SetActive(true);
                    skillArea.SetActive(false);
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    dragButton = enemyList.GetComponent<IDragButton>();
                });
                UITool.GetOrAddComponentInChildren<Button>("Skill_Btn", panel).onClick.AddListener(() =>
                {
                    skillArea.SetActive(true);
                    enemyArea.SetActive(false);
                    AudioManager.GetInstance().PlaySound("NormalButton");
                    dragButton = skillList.GetComponent<IDragButton>();
                });
                UITool.GetOrAddComponentInChildren<Button>("ListBtn_Left", panel).onClick.AddListener(() =>
                {
                    dragButton.LeftButton_Click();
                });
                UITool.GetOrAddComponentInChildren<Button>("ListBtn_Right", panel).onClick.AddListener(() =>
                {
                    dragButton.RightButton_Click();
                });
            });
        });
        

    }

    public void UpdateEnemyPanel(int index, GameObject enemyPanel)
    {
        Transform t = UITool.GetOrAddComponentInChildren<Transform>("EnemyImage", enemyPanel);
        try
        {
            GameObject.Destroy(t.GetChild(0).gameObject);
        }
        catch { }
        GameObject.Instantiate(enemys[index].image,t).transform.parent = t;

        UITool.GetOrAddComponentInChildren<Text>("NameText", panel).text = enemys[index].name;
        UITool.GetOrAddComponentInChildren<Text>("IntroductionText", panel).text = enemys[index].describe;
        //UITool.GetOrAddComponentInChildren<Text>("RoleHP", panel).text = "血量：" + enemys[index].MaxHP;
        //UITool.GetOrAddComponentInChildren<Text>("RoleAttack", panel).text = "攻击力" + enemys[index].Aggressivity;
    }

    public void UpdateSkillPanel(int index, GameObject panel)
    {
        UITool.GetOrAddComponentInChildren<Image>("SkillImage", panel).sprite = skills[index].icon;
        UITool.GetOrAddComponentInChildren<Text>("SkillNameText", panel).text = skills[index].name;
        UITool.GetOrAddComponentInChildren<Text>("DescribeText", panel).text = skills[index].describe;
    }
}
