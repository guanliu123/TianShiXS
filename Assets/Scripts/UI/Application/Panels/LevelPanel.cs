using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
using UnityEngine.UI;
using System.Reflection;
using System.Data.Common;
//using UnityEngine.UIElements;

public class LevelPanel : BasePanel {
    private static readonly string path = "Prefabs/Panels/LevelPanel";

    public int levelNum;

    public LevelPanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        levelNum = DataManager.GetInstance().levelDatasDic.Count;

        var t = UITool.GetOrAddComponentInChildren<LevelPortraitList>("LevelList", panel);
        t._panel = this;
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("ListBtn_Left", panel).onClick.AddListener(() =>
        {
            t.DragLeft();
        });
        UITool.GetOrAddComponentInChildren<Button>("ListBtn_Right", panel).onClick.AddListener(() =>
        {
            t.DragRight();
        });
    }
    
    public void UpdateLevelList(int index,GameObject levelPanel)
    {
        int n = Mathf.Min(3, levelNum - index * 3);
        int i;

        for(i = 1; i <= n; i++)
        {
            AddListener(UITool.GetOrAddComponentInChildren<Button>("Tag" + i, levelPanel), index*3+i);
            UITool.GetOrAddComponentInChildren<Text>("Level" +i, levelPanel).text = $"第\n{index * 3 + i}\n关";
        }
        for (; i <= 3; i++)
        {
            UITool.GetOrAddComponentInChildren<Transform>("Tag" + i, levelPanel).gameObject.SetActive(false);
        }
    }
    void AddListener(Button button, int parameter)
    {
        button.onClick.AddListener(delegate
        {
            GameManager.GetInstance().ChangeLevel(parameter);
            
            PanelManager.Instance.Pop();
        });
    }
}
