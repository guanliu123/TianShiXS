using Abelkhan;
using Game;
using StarkSDKSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRoot : MonoBehaviour
{
    public static ClientRoot Instance { get; private set; }

    //public static Game.GameClient gameClient;
    private void Awake()
    {
        
    }
    void Start()
    {
        Debug.Log("Client Is Run");
        //GameClient.Instance = new Game.GameClient();
        GameClient.Instance._client.connect_gate("wss://tsxs.ucat.games:3001", 3000);
        GameClient.Instance._client.onGateConnect += () =>
        {
            StarkSDK.API.GetAccountManager().OpenSettingsPanel((success) =>
            {
                Debug.Log("Auth Success");
                StarkSDK.API.GetAccountManager().Login(DySuccessLogin, DyFailedLogin, true);
            }, (fail) =>
            {
                Debug.Log("Auth Fail");
            });
        };
        GameClient.Instance._client.onGateConnectFaild += () => {
            Debug.Log("connect gate faild!");
        };
        GameClient.Instance._client.onHubConnect += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} sucessed!", hub_name));
        };
        GameClient.Instance._client.onHubConnectFaild += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} faild!", hub_name));
        };
    }

    public void DySuccessLogin(string code, string anonymousCode, bool isLogin)
    {
        Debug.Log("抖音登录成功");
        GameClient.Instance._client.get_hub_info("login", (hub_info) =>
        {
            GameClient.Instance._login_Caller.get_hub(hub_info.hub_name).player_login_no_token(code).callBack((string player_hub_name, string token) =>
            {
                GameClient.Instance._player_hub_name = player_hub_name;
                GameClient.Instance._player_login_Caller.get_hub(player_hub_name).player_login(token, "dy_name").callBack((UserData data) =>
                {
                    Debug.Log($"player_login success!");
                    GameManager.GetInstance()._UserData = data;
                    //SceneSystem.Instance.SetScene(new StartScene());
                }, (err) =>
                {
                    Debug.Log($"player_login err:{err}");
                    if (err == (int)em_error.unregistered_palyer)
                    {
                        GameClient.Instance._player_login_Caller.get_hub(player_hub_name).create_role(token, "dy_name").callBack((UserData data) =>
                        {
                            Debug.Log($"create_role success!");
                            GameManager.GetInstance()._UserData = data;
                            //SceneSystem.Instance.SetScene(new StartScene());
                            //...
                        }, (error) =>
                        {
                            Console.WriteLine("player_login_dy err:{0}", err);
                        });
                    }
                });
            }, (err) =>
            {
                Debug.Log($"player_login_dy err:{err}");
            });
        });
    }

    public void DyFailedLogin(string errMsg)
    {
        Debug.LogError("抖音登录失败，失败原因：" + errMsg);
    }

    void Update()
    {
        GameClient.Instance._client.poll();
    }

}
