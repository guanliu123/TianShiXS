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
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        GameManager.GetInstance().ClearFloatDamage();

        Transform content = UITool.GetOrAddComponentInChildren<Transform>("Content", panel);
        for (int i = 0; i < SkillManager.GetInstance().nowSkillIcons.Count; i++)
        {
            GameObject t = PoolManager.GetInstance().GetObj("SkillTag",ResourceType.UI);
            UITool.GetOrAddComponentInChildren<Image>("Icon", t).sprite = SkillManager.GetInstance().nowSkillIcons.ElementAt(i).Value;
            t.transform.position = content.position;
            t.transform.SetParent(content);
            skillIcons.Add(t);
        }

        if (skillIcons.Count > 0)
        {
            //float scrollRectX = skillIcons.Count * (skillIcons[0].GetComponent<RectTransform>().sizeDelta.x + content.GetComponent<HorizontalLayoutGroup>().spacing);
            //content.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollRectX, content.GetComponent<RectTransform>().sizeDelta.y);
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
