using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
using UnityEngine.UI;

public class ChestPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/ChestPanel";
    public ChestPanel() : base(new UIType(path))
    {

    }

    GameObject panel;

    private Transform selectPanel;
    private Transform obtainPanel;

    private int selectNum;

    private List<Toggle> selectTogs;

    public override void OnEnter()
    {
        panel = UIManager.Instance.GetSingleUI(UIType);
        selectTogs = new List<Toggle>();

        selectPanel = UITool.GetOrAddComponentInChildren<Transform>("SelectPanel", panel);
        obtainPanel = UITool.GetOrAddComponentInChildren<Transform>("ObtainPanel", panel);

        UITool.GetOrAddComponentInChildren<Button>("OpenChest_Btn", panel).onClick.AddListener(() =>
        {
               OpenChest();
        });

        UITool.GetOrAddComponentInChildren<Button>("ObtainPanel", panel).onClick.AddListener(() =>
         {
             CloseTopPanel();
         });

        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
         {
             PanelManager.Instance.Pop();
         });

        for (int i = 0; i < 5; i++)
        {
            string _path = "select_" + i;
            selectTogs.Add(UITool.GetOrAddComponentInChildren<Toggle>(_path, panel));
            selectTogs[i].onValueChanged.AddListener(Selected);
        }

        selectPanel.gameObject.SetActive(false);
        obtainPanel.gameObject.SetActive(false);
    }

    private void OpenChest()
    {
        selectPanel.gameObject.SetActive(true);

    }

    private void Selected(bool _value)
    {
        selectPanel.gameObject.SetActive(false);
        obtainPanel.gameObject.SetActive(true);
        for(int i=0;i<selectTogs.Count;i++)
        {
            if(selectTogs[i].isOn)
            {
                selectNum = i;
                break;
            }
        }
        Text _text;
        _text = GameObject.Find("ObtainPanel/Image/Text").transform.GetComponent<Text>();

        _text.text = "获得物品" + (selectNum + 1);
    }

    private void CloseTopPanel()
    {
        selectPanel.gameObject.SetActive(false);
        obtainPanel.gameObject.SetActive(false);
    }
}
