using Abelkhan;
using Game;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        {
            SceneSystem.Instance.SetScene(new LoadScene());
        }
        else if (SceneManager.GetActiveScene().name == "StartScene")
        {
            SceneSystem.Instance.SetScene(new StartScene());
        }  
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(Delay(sceneName));
    }

    private IEnumerator Delay(string sceneName)
    {
        Debug.Log($"start load {sceneName}!");

        Task tLoadLevel = null;

        var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
        handle.Completed += async (obj) =>
        {
            if (sceneName == StartScene.sceneName)
            {
                SceneSystem.Instance.SetScene(new StartScene());
            }
            else if (sceneName == LevelScene.sceneName)
            {
                if (tLoadLevel != null)
                {
                    await tLoadLevel;
                }
                SceneSystem.Instance.SetScene(new LevelScene());
            }
        };

        if (sceneName == StartScene.sceneName)
        {
            //打开加载读条界面
            PanelManager.Instance.Push(new LoadingPanel());
            Debug.Log("load Scenes/StartScene!");
            while (!handle.IsDone)
            {
                Debug.Log("wait load Scenes/StartScene!");
                // 在此可使用handle.PercentComplete进行进度展示
                //修改进度条数值
                LoadingPanel.PercentComplete = handle.PercentComplete;
                //等待0.5秒
                yield return new WaitForSeconds(1.5f);
            }
            PanelManager.Instance.Pop();
        }
        else if(sceneName == LevelScene.sceneName)
        {
            Debug.Log("load Scenes/LevelScene!");
            //加载场景资源
            tLoadLevel = GameManager.Instance.StartLoad();
            //打开加载读条界面
            PanelManager.Instance.Push(new LoadingPanel());
            while (!handle.IsDone)
            {
                Debug.Log("wait load Scenes/LevelScene!");
                // 在此可使用handle.PercentComplete进行进度展示
                //修改进度条数值
                LoadingPanel.PercentComplete = handle.PercentComplete;
                //等待0.5秒
                yield return new WaitForSeconds(1.5f);
            }
            PanelManager.Instance.Pop();
        }
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
