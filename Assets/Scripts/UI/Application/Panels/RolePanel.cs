using System;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class RolePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/RolePanel";

    GameObject panel;
    List<CharacterDatas> players;
    CharacterType choosePlayer;

    public RolePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        panel = UIManager.Instance.GetSingleUI(UIType);

        players= GameManager.GetInstance().GetRole();
        UpdatePlayerPanel(0);

        UITool.GetOrAddComponentInChildren<list>("list", panel)._panel = this;

        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });

        UITool.GetOrAddComponentInChildren<Button>("Use_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeRole(choosePlayer);
        });
    }


    public void UpdatePlayerPanel(int index)
    {
        choosePlayer = players[index].characterType;
        UITool.GetOrAddComponentInChildren<Text>("RoleName", panel).text = players[index].characterData.name;
        UITool.GetOrAddComponentInChildren<Text>("RoleSkill", panel).text = 
            "角色攻击方式："+ players[index].characterData.bulletTypes.ToString();
        UITool.GetOrAddComponentInChildren<Text>("RoleAttr", panel).text =
            "角色血量：" + players[index].characterData.MaxHP+"\n角色攻击力加成："+players[index].characterData.Aggressivity;
    }
}
