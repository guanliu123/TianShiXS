using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;


public class SkillPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/SkillPanel";

    public SkillPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        for(int i = 1; i <= 3; i++)
        {
            UITool.GetOrAddComponentInChildren<Button>("Skill_Btn" + i, panel).onClick.AddListener(() =>
            {
                RandomSkill();
                Time.timeScale = 1;
                PanelManager.Instance.Pop();
            });
        }
    }

    public void RandomSkill()
    {
        Debug.Log("获得技能");
    }
}
