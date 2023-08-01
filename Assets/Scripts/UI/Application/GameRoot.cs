using Abelkhan;
using Game;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using WeChatWASM;
//游戏的根管理器
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    private void Awake()
    {
        WX.InitSDK((int code) =>
        {
            PreLoadAllAssets("default");
        });
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
         if (SceneManager.GetActiveScene().name == "LoadScene")
             SceneSystem.Instance.SetScene(new LoadScene());
         if (SceneManager.GetActiveScene().name == "StartScene")
             SceneSystem.Instance.SetScene(new StartScene());    
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(Delay(sceneName));
    }

    private IEnumerator Delay(string sceneName)
    {
        var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);

        handle.Completed += (obj) =>
        {

        };
        if(sceneName!= "Scenes/LevelScene")
        {
            while (!handle.IsDone)
            {
                // 在此可使用handle.PercentComplete进行进度展示
                //打开加载读条界面
                PanelManager.Instance.Push(new LoadingPanel());
                //修改进度条数值
                LoadingPanel.PercentComplete = handle.PercentComplete;
                //等待0.5秒
                yield return new WaitForSeconds(0.5f);
            }
        }
        else if(sceneName == "Scenes/LevelScene")
        {
            while (!handle.IsDone)
            {
                // 在此可使用handle.PercentComplete进行进度展示
                //打开加载读条界面
                PanelManager.Instance.Push(new LoadingPanel());
                //加载场景资源
                GameManager.Instance.StartLoad();
                //修改进度条数值
                LoadingPanel.PercentComplete = handle.PercentComplete;
                //等待0.5秒
                yield return new WaitForSeconds(0.5f);
            }
        }
        PanelManager.Instance.Pop();
        handle.Result.ActivateAsync();
    }

    public void StartGame()
    {
        SceneSystem.Instance.SetScene(new StartScene());       
    }

    public void PreLoadAllAssets(string label)
    {
        var _handle=Addressables.DownloadDependenciesAsync(label,true);
        while(_handle.IsDone)
        {
            Debug.Log(_handle.PercentComplete);
        }
        return;

    }
}
