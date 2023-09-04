using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;
using System.Linq;
using System;

public class SkillPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/SkillPanel";
    private List<GameObject> skillIcons = new List<GameObject>();

    public SkillPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = null;
        Time.timeScale = 0f;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;

            GameManager.GetInstance().ClearFloatDamage();
            GameManager.GetInstance().LockMove();

            //UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
            //{
            //    PanelManager.Instance.Pop();
            //});

            Transform content = UITool.GetOrAddComponentInChildren<Transform>("Content", panel);
            /*for (int i = 0; i < SkillManager.GetInstance().nowSkillIcons.Count; i++)
            {
                PoolManager.GetInstance().GetObj("SkillTag", async t =>
                {
                    await ResourceManager.GetInstance().LoadRes<Sprite>(SkillManager.GetInstance().nowSkillIcons.ElementAt(i).Value, result =>
                    {
                        UITool.GetOrAddComponentInChildren<Image>("Icon", t).sprite = result;
                    }, ResourceType.Null, ".png");
                    t.transform.position = content.position;
                    t.transform.SetParent(content);
                    skillIcons.Add(t);
                }, ResourceType.UI);
            }*/

            SkillUpgrade[] su = SkillManager.GetInstance().RandomSkill().ToArray();
            for (int i = 1; i <= 3; i++)
            {
                //GameObject skillRect = UITool.GetOrAddComponentInChildren<Transform>("Skill" + i, panel).gameObject;

                SkillChoose(su, i, panel);
            }
        });

    }
    void SkillChoose(SkillUpgrade[] su, int i, GameObject panel)
    {
        PoolManager.GetInstance().GetObj(su[i - 1].quality, async skillRect =>
        {
            var tra = UITool.GetOrAddComponentInChildren<Transform>("Skill" + i, panel);
            skillRect.transform.position = tra.position;
            skillRect.transform.SetParent(tra);
            
                ResourceManager.GetInstance().LoadRes<Sprite>(su[i - 1].iconPath, result =>
                {
                try
                {
                    UITool.GetOrAddComponentInChildren<Image>("Skill_Icon", skillRect).sprite = result;
                    }
                    catch { }
                }, ResourceType.Null, ".png");
            
            UITool.GetOrAddComponentInChildren<Transform>("Tag", skillRect).gameObject.SetActive(su[i - 1].isNew);
            var t = UITool.GetOrAddComponentInChildren<Button>("Skill_Btn", skillRect);
            Debug.Log("技能"+i+"描述："+ su[i - 1].describe);
            t.GetComponentInChildren<Text>().text = su[i - 1].describe;
            SkillPromote(t, i, su);
            //更改品级框       
        }, ResourceType.UI);
    }

    void SkillPromote(Button button, int parameter, SkillUpgrade[] su)
    {
        button.onClick.AddListener(delegate {
            
                AudioManager.GetInstance().PlaySound("NormalButton");
                su[parameter - 1].skill.OnUse();
                Time.timeScale = 1;
                GameManager.GetInstance().PlayerEvolution();
            
            /*catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }*/
            
                PanelManager.Instance.Pop();
            
        });
    }

    public override void OnExit()
    {
        base.OnExit();
        for (int i = 0; i < skillIcons.Count; i++)
        {
            PoolManager.GetInstance().PushObj("SkillTag", skillIcons[i]);
        }
        skillIcons.Clear();
        GameManager.GetInstance().UnlockMove();
    }
}