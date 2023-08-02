using Abelkhan;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using WeChatWASM;
//游戏的根管理器
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance { get; private set; }

    public string loadingScene;

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
         //if (SceneManager.GetActiveScene().name == "LoadScene")
         //    SceneSystem.Instance.SetScene(new LoadScene());
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
        handle.Result.ActivateAsync();
        PanelManager.Instance.Pop();

    }

    public void TryLoad(string _nextSceneName,Action _callBack)
    {
        //加载中间过渡场景
        var _lastLoadHandle = Addressables.LoadSceneAsync(loadingScene,LoadSceneMode.Single);

        _lastLoadHandle.Completed += (op) =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                //载入成功后做些什么...

                //开始协程
                StartCoroutine(WaitForLoading(_lastLoadHandle, _nextSceneName, _callBack));
            }
        };
    }

    private IEnumerator WaitForLoading(AsyncOperationHandle<SceneInstance> _lastLoadHandle,string _nextSceneName,Action _callBack)
    {
        var _currLoadHandle = Addressables.LoadSceneAsync(_nextSceneName, LoadSceneMode.Additive);
        while(_currLoadHandle.Status==AsyncOperationStatus.None) 
        {
            //开始加载时...
            LoadScene.Instance.SetPercent(_currLoadHandle.PercentComplete);
            yield return null;

        }

        if(_currLoadHandle.Status==AsyncOperationStatus.Succeeded)
        {
            //加载完毕时...
            LoadScene.Instance.SetPercent(1.0f);
            yield return new WaitForSeconds(0.5f);
            //卸载上一场景
            Addressables.UnloadSceneAsync(_lastLoadHandle).Completed += (op) =>
            {

                if(op.Status == AsyncOperationStatus.Succeeded)
                {
                    //所有流程结束后回调
                    _callBack?.Invoke();
                }
            };
        }

        yield return null;
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
