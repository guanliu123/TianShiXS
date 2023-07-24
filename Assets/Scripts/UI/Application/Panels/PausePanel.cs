using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/PausePanel";
    private List<GameObject> skillIcons = new List<GameObject>();
    public PausePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = null;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;
            GameManager.GetInstance().ClearFloatDamage();

            Transform content = UITool.GetOrAddComponentInChildren<Transform>("Content", panel);
            for (int i = 0; i < SkillManager.GetInstance().nowSkillIcons.Count; i++)
            {
                PoolManager.GetInstance().GetObj("SkillTag", t =>
                {
                    ResourceManager.GetInstance().LoadRes<Sprite>(SkillManager.GetInstance().nowSkillIcons.ElementAt(i).Value, result =>
                    {
                        UITool.GetOrAddComponentInChildren<Image>("Icon", t).sprite = result;
                    }, ResourceType.Null, ".png");
                    t.transform.position = content.position;
                    t.transform.SetParent(content);
                    skillIcons.Add(t);
                }, ResourceType.UI);
            }

            Time.timeScale = 0;
            UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                Time.timeScale = 1;
                PanelManager.Instance.Pop();
            });
            UITool.GetOrAddComponentInChildren<Button>("Continue_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                Time.timeScale = 1;
                PanelManager.Instance.Pop();
            });
            UITool.GetOrAddComponentInChildren<Button>("Exit_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                Time.timeScale = 1;
                GameManager.GetInstance().QuitGame();
                PanelManager.Instance.Clear();
                PanelManager.Instance.Push(new StartPanel());
            });
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
    }
}
