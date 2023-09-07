using Abelkhan;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (SceneManager.GetActiveScene().name == "StartScene")
            SceneSystem.Instance.SetScene(new StartScene());
    }

    public void TryLoad(string _nextSceneName,Action _callBack)
    {
        //加载中间过渡场景
        AsyncOperationHandle<SceneInstance> _lastLoadHandle = Addressables.LoadSceneAsync(loadingScene,LoadSceneMode.Single);

        Debug.Log($"TryLoad _lastLoadHandle {loadingScene}");
        _lastLoadHandle.Completed += (op) =>
        {
            if (op.IsDone)
            {
                //载入成功后做些什么...
                StartCoroutine(LevelManager.GetInstance().LoadMap());
                //开始协程
                StartCoroutine(WaitForLoading(_lastLoadHandle, _nextSceneName, _callBack));
            }
        };
    }

    private IEnumerator WaitForLoading(AsyncOperationHandle<SceneInstance> _lastLoadHandle,string _nextSceneName,Action _callBack)
    {
        Debug.Log($"TryLoad WaitForLoading {_nextSceneName}");
        Addressables.InitializeAsync();
        var _currLoadHandle = Addressables.LoadSceneAsync(_nextSceneName, LoadSceneMode.Single,false);
        while (_currLoadHandle.Status==AsyncOperationStatus.None)
        {
            //开始加载时...
            LoadScene.Instance.SetPercent(_currLoadHandle.PercentComplete);
            Debug.Log($"TryLoad WaitForLoading _currLoadHandle {_currLoadHandle.PercentComplete}");
            yield return new WaitForSeconds(0.5f);
            //yield return null;

        }
        if(_nextSceneName==LevelScene.sceneName)
        {
            while (!LevelManager.GetInstance().LoadMapHandel)
            {
                yield return null;
            }
        }
        Debug.Log($"TryLoad WaitForLoading _currLoadHandle {_currLoadHandle.IsDone}");
        
        if (_currLoadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            //加载完毕时...
            LoadScene.Instance.SetPercent(1.0f);
            _currLoadHandle.WaitForCompletion().ActivateAsync();
            //Addressables.UnloadSceneAsync(_lastLoadHandle);
            yield return new WaitForSeconds(0f);
            _callBack?.Invoke();
        }
    }

    public IEnumerator LoadLevelScene(AsyncOperationHandle<SceneInstance> _lastLoadHandle, Action _callBack)
    {
        var _currLoadHandle = Addressables.LoadSceneAsync("Scenes/LevelScene", LoadSceneMode.Additive);
        while(_currLoadHandle.Status == AsyncOperationStatus.None)
            yield return null;
        Debug.Log($"TryLoad WaitForLoading _currLoadHandle {_currLoadHandle.Status}");
        if (_currLoadHandle.Status == AsyncOperationStatus.Succeeded)
        {
            Addressables.UnloadSceneAsync(_lastLoadHandle).Completed+=(op)=>
            {
                _callBack?.Invoke();
            };
        }
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

    private void Update()
    {

    }
}
