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
    Dictionary<int, CharacterMsg> players;
    int choosePlayer;

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
            AudioManager.GetInstance().PlaySound("NormalButton");
            PanelManager.Instance.Pop();
        });

        UITool.GetOrAddComponentInChildren<Button>("Use_Btn", panel).onClick.AddListener(() =>
        {
            AudioManager.GetInstance().PlaySound("NormalButton");
            GameManager.GetInstance().ChangeRole(choosePlayer);
        });
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
        GameObject.Instantiate(players.ElementAt(index).Value.image,t).transform.parent = t;
        UITool.GetOrAddComponentInChildren<Text>("NameText", panel).text = players.ElementAt(index).Value.name;     

        //UITool.GetOrAddComponentInChildren<Text>("RoleSkill", panel).text = players.ElementAt(index).Value.describe;
        //UITool.GetOrAddComponentInChildren<Text>("RoleHP", panel).text = "血量："+ players.ElementAt(index).Value.MaxHP;
        //UITool.GetOrAddComponentInChildren<Text>("RoleAttack", panel).text = "攻击力" + players.ElementAt(index).Value.Aggressivity;
    }
}
