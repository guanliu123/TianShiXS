using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
public class LevelScene : SceneBase
{
    private static readonly string sceneName = "Scenes/LevelScene";

    public override void OnEnter()
    {
        if (SceneManager.GetActiveScene().name != "LevelScene")
        {
            //SceneManager.LoadScene(sceneName);
            GameRoot.Instance.SwitchScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
        {
            PanelManager.Instance.Push(new GamePanel());
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
        PanelManager.Instance.Push(new GamePanel());
        GameManager.GetInstance().StartGame();
    }
}
