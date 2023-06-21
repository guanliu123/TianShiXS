using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;


public class SkillPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/SkillPanel";
    private List<GameObject> skillIcons = new List<GameObject>();

    public SkillPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        GameManager.GetInstance().ClearFloatDamage();
        GameManager.GetInstance().LockMove();

        //UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        //{
        //    PanelManager.Instance.Pop();
        //});

        Transform content = UITool.GetOrAddComponentInChildren<Transform>("Content", panel);
        for (int i = 0; i < SkillManager.GetInstance().nowSkillIcons.Count; i++)
        {
           GameObject t =  PoolManager.GetInstance().GetObj("SkillTag");
            UITool.GetOrAddComponentInChildren<Image>("Icon", t).sprite = SkillManager.GetInstance().nowSkillIcons[i];
            t.transform.position = content.position;
            t.transform.SetParent(content);
            skillIcons.Add(t);
        }       

        List<SkillUpgrade> su = SkillManager.GetInstance().RandomSkill();
        for(int i = 1; i <= 3; i++)
        {
            UITool.GetOrAddComponentInChildren<Image>("Skill_Icon" + i, panel).sprite=su[i-1].icon;
            var t = UITool.GetOrAddComponentInChildren<Button>("Skill_Btn" + i, panel);
            t.GetComponentInChildren<Text>().text = su[i - 1].describe;           

            SkillPromote(t, i, su);
        }
    }

    void SkillPromote(Button button, int parameter,List<SkillUpgrade> su)
    {
        button.onClick.AddListener(delegate {
            su[parameter - 1].skill.OnUse();
            Time.timeScale = 1;
            GameManager.GetInstance().PlayerEvolution();
            PanelManager.Instance.Pop();
        });
    }

    public void ShowSkills()
    {

    }

    public override void OnExit()
    {
        base.OnExit();
        for(int i = 0; i < skillIcons.Count; i++)
        {
            PoolManager.GetInstance().PushObj("SkillTag", skillIcons[i]);
        }
        skillIcons.Clear();
        GameManager.GetInstance().UnlockMove();
    }
}
