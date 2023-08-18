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
    Dictionary<int, CharacterMsg> players = new Dictionary<int, CharacterMsg>();
    int choosePlayer;

    public RolePanel() : base(new UIType(path))
    {

    }
    public override void OnEnter()
    {
        UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;
            UITool.GetOrAddComponentInChildren<Button>("Close_Btn", obj).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                PanelManager.Instance.Pop();
            });

            UITool.GetOrAddComponentInChildren<Button>("Use_Btn", obj).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
                GameManager.GetInstance().ChangeRole(choosePlayer);
            });

            var t = UITool.GetOrAddComponentInChildren<PlayerPortraitList>("Portrait_List", obj);
            t._panel = this;
        });

        players = GameManager.GetInstance().GetPlayerRole();

        //UpdatePlayerPanel(0);      
    }


    public async void UpdatePlayerPanel(int index, GameObject playerPanel)
    {
        if (!players.ContainsKey(index))
        {
            return;
        }

        choosePlayer = players.ElementAt(index).Key;
        //UITool.GetOrAddComponentInChildren<Transform>("PlayerImage", panel) = players[index].characterData.icon;
        Transform t = UITool.GetOrAddComponentInChildren<Transform>("PlayerImage", playerPanel);
        try
        {
            GameObject.Destroy(t.GetChild(0).gameObject);
        }
        catch { }
        //GameObject.Instantiate(players.ElementAt(index).Value.imagePath,t).transform.parent = t;
        await ResourceManager.GetInstance().LoadRes<GameObject>(players.ElementAt(index).Value.imagePath, temp => {
            GameObject.Instantiate(temp, t).transform.parent = t;
        }, ResourceType.UI);

        UITool.GetOrAddComponentInChildren<Text>("NameText", panel).text = players.ElementAt(index).Value.name;

        //UITool.GetOrAddComponentInChildren<Text>("RoleSkill", panel).text = players.ElementAt(index).Value.describe;
        //UITool.GetOrAddComponentInChildren<Text>("RoleHP", panel).text = "血量："+ players.ElementAt(index).Value.MaxHP;
        //UITool.GetOrAddComponentInChildren<Text>("RoleAttack", panel).text = "攻击力" + players.ElementAt(index).Value.Aggressivity;
    }
}