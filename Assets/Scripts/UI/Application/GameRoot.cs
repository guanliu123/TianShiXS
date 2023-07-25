using Game;
using System.Collections;
using System.Collections.Generic;
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
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
        });
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
        //SceneManager.LoadScene(sceneName);
        StartCoroutine(Delay(sceneName));
    }

    private IEnumerator Delay(string sceneName)
    {
        //AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        //while(!ao.isDone)
        //{
        //    yield return new WaitForSeconds(3.0f);
        //}
       
        var handle = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, true);

        handle.Completed += (obj) =>
        {

        };
        while (!handle.IsDone)
        {
            // 在此可使用handle.PercentComplete进行进度展示
            yield return new WaitForSeconds(1.0f);
        }

    }

    public void StartGame()
    {
        //StarkSDK.API.GetAccountManager().CheckSession(() =>
        //{
        //    SceneSystem.Instance.SetScene(new StartScene());
        //},
        //(err) =>
        //{
        //    StarkSDK.API.GetAccountManager().Login(ClientRoot.Instance.DySuccessLogin, ClientRoot.Instance.DyFailedLogin, true);
        //});

        SceneSystem.Instance.SetScene(new StartScene());
    }
}
