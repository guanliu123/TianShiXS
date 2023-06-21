using System;
using System.Collections.Generic;
using System.Linq;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class RolePanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/RolePanel";

    GameObject panel;
    Dictionary<CharacterType, CharacterMsg> players;
    CharacterType choosePlayer;

    public RolePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        panel = UIManager.Instance.GetSingleUI(UIType);

        players= GameManager.GetInstance().GetPlayerRole();

        //UpdatePlayerPanel(0);

        var t= UITool.GetOrAddComponentInChildren<PlayerPortraitList>("Portrait_List", panel);
        t._panel = this;

        UITool.GetOrAddComponentInChildren<Button>("Close_Btn", panel).onClick.AddListener(() =>
        {
            PanelManager.Instance.Pop();
        });

        UITool.GetOrAddComponentInChildren<Button>("Use_Btn", panel).onClick.AddListener(() =>
        {
            GameManager.GetInstance().ChangeRole(choosePlayer);
        });
        //UITool.GetOrAddComponentInChildren<Button>("ListBtn_Left", panel).onClick.AddListener(() =>
        //{
        //    t.LeftButton_Click();
        //});
        //UITool.GetOrAddComponentInChildren<Button>("ListBtn_Right", panel).onClick.AddListener(() =>
        //{
        //    t.RightButton_Click();
        //});
    }


    public void UpdatePlayerPanel(int index,GameObject playerPanel)
    {
        choosePlayer = players.ElementAt(index).Key;
        //UITool.GetOrAddComponentInChildren<Transform>("PlayerImage", panel) = players[index].characterData.icon;
        Transform t = UITool.GetOrAddComponentInChildren<Transform>("PlayerImage", playerPanel);
        try
        {
            GameObject.Destroy(t.GetChild(0).gameObject);
        }
        catch{ }
        GameObject.Instantiate(players.ElementAt(index).Value.icon,t).transform.parent = t;
        UITool.GetOrAddComponentInChildren<Text>("NameText", panel).text = players.ElementAt(index).Value.name;     

        //UITool.GetOrAddComponentInChildren<Text>("RoleSkill", panel).text = players.ElementAt(index).Value.describe;
        //UITool.GetOrAddComponentInChildren<Text>("RoleHP", panel).text = "血量："+ players.ElementAt(index).Value.MaxHP;
        //UITool.GetOrAddComponentInChildren<Text>("RoleAttack", panel).text = "攻击力" + players.ElementAt(index).Value.Aggressivity;
    }
}
