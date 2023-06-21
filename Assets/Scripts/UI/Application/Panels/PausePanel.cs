using System.Collections;
using System.Collections.Generic;
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
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        GameManager.GetInstance().ClearFloatDamage();

        Transform content = UITool.GetOrAddComponentInChildren<Transform>("Content", panel);
        for (int i = 0; i < SkillManager.GetInstance().nowSkillIcons.Count; i++)
        {
            GameObject t = PoolManager.GetInstance().GetObj("SkillTag");
            UITool.GetOrAddComponentInChildren<Image>("Icon", t).sprite = SkillManager.GetInstance().nowSkillIcons[i];
            t.transform.position = content.position;
            t.transform.SetParent(content);
            skillIcons.Add(t);
        }

        Time.timeScale = 0;
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Continue_Btn", panel).onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Exit_Btn", panel).onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            GameManager.GetInstance().QuitGame();
            PanelManager.Instance.Clear();
            PanelManager.Instance.Push(new StartPanel());
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
