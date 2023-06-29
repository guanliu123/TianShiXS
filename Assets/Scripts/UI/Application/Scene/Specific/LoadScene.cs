using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : SceneBase
{
    private static readonly string sceneName = "Scenes/LoadScene";

    public override void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != "LoadScene")
        {
            //SceneManager.LoadScene(sceneName);
            GameRoot.Instance.SwitchScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            PanelManager.Instance.Push(new LoadPanel());
        }
        //MapManager.GetInstance().StartMapCreate();
    }

    public override void OnExit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        PanelManager.Instance.Clear();
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PanelManager.Instance.Push(new LoadPanel());
    }
}
