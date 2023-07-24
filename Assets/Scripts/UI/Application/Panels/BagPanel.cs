using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/BagPanel";

    //记入道具的类型，方便切换面板的时候隐藏不同组的道具
    private Dictionary<int, List<GameObject>> props;

    public BagPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameObject panel = null;
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;
        });
        //UITool.GetOrAddComponentInChildren<Button>("Open_Btn", panel).onClick.AddListener(() => { DataCenter.Money += 100; });
        Transform bagPanel = UITool.GetOrAddComponentInChildren<Transform>("ObjArea", panel);
        
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
            AudioManager.GetInstance().PlaySound("NormalButton");
        });

        //这里是背包界面Toggle组的事件，通过当前所选Toggle来判断隐藏props字典中的哪些项目
        UITool.GetOrAddComponentInChildren<Toggle>("All_Toggle", panel).onValueChanged.AddListener((value) =>
        {
            if (value) { 
                foreach(var item in props)
                {
                    if (!item.Value[0].activeInHierarchy)
                    {
                        foreach (var t in item.Value) t.SetActive(true);
                    }
                }
            }
        });
        UITool.GetOrAddComponentInChildren<Toggle>("Warehouse_Toggle", panel).onValueChanged.AddListener((value) =>
        {
            if (value) { foreach (var item in props[1]) item.SetActive(false); foreach (var item in props[0]) item.SetActive(true); }
        });
        UITool.GetOrAddComponentInChildren<Toggle>("Debris_Toggle", panel).onValueChanged.AddListener((value) =>
        {
            if (value) { foreach (var item in props[0]) item.SetActive(false); foreach (var item in props[1]) item.SetActive(true); }
        });
        //加入组的同时记录道具类型
        foreach (var item in GameManager.GetInstance()._UserData.PropList)
        {
            if (!props.ContainsKey(item.PropID % 10)) props.Add(item.PropID % 10, new List<GameObject>());
            ResourceManager.GetInstance().LoadByName<GameObject>("BagProp", result =>
            {
                UITool.GetOrAddComponentInChildren<Image>("ObjIcom", result).sprite = ResourceManager.GetInstance().LoadByPath<Sprite>("");
                UITool.GetOrAddComponentInChildren<Text>("ObjCount", result).text = item.Count + "";
                t.transform.SetParent(bagPanel);

                props[item.PropID % 10].Add(result);
            } , ResourceType.UI);
            //载入物品的图标            
        }
    }
}
