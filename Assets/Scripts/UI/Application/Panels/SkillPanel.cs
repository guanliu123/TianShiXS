using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFrameWork;
using System.Linq;

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

            List<SkillUpgrade> su = SkillManager.GetInstance().RandomSkill();
            for (int i = 1; i <= 3; i++)
            {
                //GameObject skillRect = UITool.GetOrAddComponentInChildren<Transform>("Skill" + i, panel).gameObject;

                SkillChoose(su, i, panel);
            }
        });

    }
    void SkillChoose(List<SkillUpgrade> su, int i, GameObject panel)
    {
        PoolManager.GetInstance().GetObj(su[i - 1].quality, async skillRect =>
        {
            var tra = UITool.GetOrAddComponentInChildren<Transform>("Skill" + i, panel);
            skillRect.transform.position = tra.position;
            skillRect.transform.SetParent(tra);
            await ResourceManager.GetInstance().LoadRes<Sprite>(su[i - 1].iconPath, result =>
            {
                UITool.GetOrAddComponentInChildren<Image>("Skill_Icon", skillRect).sprite = result;
                UITool.GetOrAddComponentInChildren<Transform>("Tag", skillRect).gameObject.SetActive(su[i - 1].isNew);
                var t = UITool.GetOrAddComponentInChildren<Button>("Skill_Btn", skillRect);
                t.GetComponentInChildren<Text>().text = su[i - 1].describe;
                SkillPromote(t, i, su);
            }, ResourceType.Null, ".png");
            //更改品级框       
        }, ResourceType.UI);
    }

    void SkillPromote(Button button, int parameter, List<SkillUpgrade> su)
    {
        button.onClick.AddListener(delegate {
            try
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                su[parameter - 1].skill.OnUse();
                Time.timeScale = 1;
                GameManager.GetInstance().PlayerEvolution();
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            finally
            {
                PanelManager.Instance.Pop();
            }
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