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

        List<SkillUpgrade> su = SkillManager.GetInstance().RandomSkill();
        for(int i = 1; i <= 3; i++)
        {
            var t = UITool.GetOrAddComponentInChildren<Button>("Skill_Btn" + i, panel);

            t.GetComponentInChildren<Text>().text = su[i - 1].describe;
        }
        /*List<SkillUpgrade> su= GameManager.GetInstance().RandomSkill();
        for (int i = 1; i <= 3; i++)
        {
            var t = UITool.GetOrAddComponentInChildren<Button>("Skill_Btn" + i, panel);

            t.GetComponentInChildren<Text>().text = su[i - 1].describe;
            SkillPromote(t, i, su);

        }*/
    }

    void SkillPromote(Button button, int parameter,List<SkillUpgrade> su)
    {
        button.onClick.AddListener(delegate {
            /*BulletManager.GetInstance().BulletEvolute(su[parameter - 1].buffType, su[parameter - 1].bulletType);
            Time.timeScale = 1;
            GameManager.GetInstance().PlayerEvolution();
            PanelManager.Instance.Pop();*/
        });
    }
}
