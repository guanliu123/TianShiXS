using Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : BasePanel
{
    private static readonly string path = "Prefabs/Panels/FailPanel";
    public FailPanel() : base(new UIType(path))
    {

    }

    public override void OnEnter()
    {
        GameManager.GetInstance().ClearFloatDamage();
        GameManager.GetInstance().QuitGame();
        
        GameObject panel = null;
         UIManager.Instance.GetSingleUI(UIType, (obj) =>
        {
            panel = obj;
            UITool.GetOrAddComponentInChildren<Button>("Back_Btn", panel).onClick.AddListener(() =>
            {
                AudioManager.GetInstance().PlaySound("NormalButton");
            //SceneSystem.GetInstance().SetScene(new StartScene());
            /*PanelManager.Instance.Pop();
            PanelManager.Instance.Push(new StartPanel());*/
                GameRoot.Instance.TryLoad("StartScene", null, () =>
                {
                    SceneSystem.GetInstance().SetScene(new StartScene());
                });
            });
        });     
    }
}
