using Abelkhan;
using StarkSDKSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRoot : MonoBehaviour
{
    public static ClientRoot Instance { get; private set; }

    public GameClient.GameClient _gameClient;

    void Start()
    {
        Debug.Log("Client Is Run");
        _gameClient = new GameClient.GameClient();

        _gameClient._client.connect_gate("wss://tsxs.ucat.games:3001", 3000);
        _gameClient._client.onGateConnect += () =>
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
        _gameClient._client.onGateConnectFaild += () => {
            Debug.Log("connect gate faild!");
        };
        _gameClient._client.onHubConnect += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} sucessed!", hub_name));
        };
        _gameClient._client.onHubConnectFaild += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} faild!", hub_name));
        };
    }

    public void DySuccessLogin(string code, string anonymousCode, bool isLogin)
    {
        Debug.Log("抖音登录成功");
        _gameClient._client.get_hub_info("login", (hub_info) =>
        {
            _gameClient._login_Caller.get_hub(hub_info.hub_name).player_login_no_token(code).callBack((string player_hub_name, string token) =>
            {
                _gameClient._player_hub_name = player_hub_name;
                _gameClient._player_login_Caller.get_hub(player_hub_name).player_login(token, "dy_name").callBack((UserData data) =>
                {
                    Debug.Log($"player_login success!");
                    GameManager.GetInstance()._UserData = data;
                    //SceneSystem.Instance.SetScene(new StartScene());
                }, (err) =>
                {
                    Debug.Log($"player_login err:{err}");
                    if (err == (int)em_error.unregistered_palyer)
                    {
                        _gameClient._player_login_Caller.get_hub(player_hub_name).create_role(token, "dy_name").callBack((UserData data) =>
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
        _gameClient._client.poll();
    }

}
