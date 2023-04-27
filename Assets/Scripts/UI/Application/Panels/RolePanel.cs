using System;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class RolePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/RolePanel";
    public RolePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        GameObject panel = UIManager.Instance.GetSingleUI(UIType);
        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });
        UITool.GetOrAddComponentInChildren<Button>("Player1_Btn", panel).onClick.AddListener(() => 
        { 
            GameManager.GetInstance().ChangeRole(CharacterType.JinYiWei); 
        });
        UITool.GetOrAddComponentInChildren<Button>("Player2_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeRole(CharacterType.KuiJia);
        });
        UITool.GetOrAddComponentInChildren<Button>("Player3_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeRole(CharacterType.XiaKe);
        });
        UITool.GetOrAddComponentInChildren<Button>("Player4_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeRole(CharacterType.ShiXu);
        });
    }
}
