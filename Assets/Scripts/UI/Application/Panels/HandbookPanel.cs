using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class HandbookPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/HandbookPanel";

    GameObject enemyList;
    List<CharacterMsg> enemys;
    List<SkillDatas> skills;

    GameObject skillList;
    IDragButton dragButton;

    public HandbookPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        enemys = GameManager.GetInstance().GetEnemyRole();
        skills = GameManager.GetInstance().GetSkills();

        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        enemyList = UITool.GetOrAddComponentInChildren<Transform>("EnemyPortrait_List", panel).gameObject;
        enemyList.GetComponent<EnemyPortraitList>()._panel = this;
        skillList= UITool.GetOrAddComponentInChildren<Transform>("SkillPortrait_List", panel).gameObject;
        skillList.GetComponent<SkillPortraitList>()._panel = this;
        skillList.SetActive(false);
        dragButton=enemyList.GetComponent<IDragButton>();

        UITool.GetOrAddComponentInChildren<Button>("Enemy_Btn", panel).onClick.AddListener(() =>
        {
            enemyList.SetActive(true);
            skillList.SetActive(false);
            dragButton = enemyList.GetComponent<IDragButton>();
        });
        UITool.GetOrAddComponentInChildren<Button>("Skill_Btn", panel).onClick.AddListener(() =>
        {
            skillList.SetActive(true);
            enemyList.SetActive(false);
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
    }

    public void UpdateEnemyPanel(int index, GameObject panel)
    {
        Transform t = UITool.GetOrAddComponentInChildren<Transform>("EnemyImage", panel);
        try
        {
            GameObject.Destroy(t.GetChild(0).gameObject);
        }
        catch { }
        GameObject.Instantiate(enemys[index].icon,t).transform.parent = t;
        UITool.GetOrAddComponentInChildren<Text>("RoleName", panel).text = enemys[index].name;
        UITool.GetOrAddComponentInChildren<Text>("RoleSkill", panel).text = enemys[index].describe;
        //UITool.GetOrAddComponentInChildren<Text>("RoleHP", panel).text = "血量：" + enemys[index].MaxHP;
        //UITool.GetOrAddComponentInChildren<Text>("RoleAttack", panel).text = "攻击力" + enemys[index].Aggressivity;
    }

    public void UpdateSkillPanel(int index, GameObject panel)
    {
        UITool.GetOrAddComponentInChildren<Image>("SkillImage", panel).sprite = skills[index].icon[0];
        UITool.GetOrAddComponentInChildren<Text>("SkillText", panel).text = skills[index].describe;
    }
}
