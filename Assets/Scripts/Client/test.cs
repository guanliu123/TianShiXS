using Abelkhan;
using StarkSDKSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StarkSDKSpace.StarkAccount;

public class test : MonoBehaviour
{
    private Client.Client _client;
    private Abelkhan.login_caller _login_Caller;
    private Abelkhan.player_login_caller _player_login_Caller;

    private Abelkhan.player_client_module _player_Client_Module;

    private OnLoginSuccessCallback successCallback;
    private OnLoginFailedCallback failedCallback;

    private void DySuccessLogin(string code, string anonymousCode, bool isLogin)
    {
        Debug.Log("dysuccess");

        _client.get_hub_info("login", (hub_info) =>
        {
            _login_Caller.get_hub(hub_info.hub_name).player_login_no_token(code).callBack((string player_hub_name, string token) =>
            {
                _player_login_Caller.get_hub(player_hub_name).player_login(token, "dy_name").callBack((UserData data) =>
                {
                    Debug.Log($"player_login success!");
                    //...
                }, (err) =>
                {
                    Debug.Log($"player_login err:{err}");
                    if (err == (int)em_error.unregistered_palyer)
                    {
                        _player_login_Caller.get_hub(player_hub_name).create_role(token, "dy_name").callBack((UserData data) =>
                        {
                            Debug.Log($"create_role err success!");
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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Client Is Run");

        _client = new Client.Client();
        _login_Caller = new login_caller(_client);
        _player_login_Caller = new player_login_caller(_client);

        _player_Client_Module = new player_client_module(_client);
        _player_Client_Module.on_archive_sync += _player_Client_Module_on_archive_sync;

        successCallback = DySuccessLogin;
        if(successCallback == null)
        {
            Debug.LogError("successCallBack is null");
        }
        
        _client.connect_gate("wss://tsxs.ucat.games:3001", 3000);
        
        _client.onGateConnect += () => {
            Debug.Log("connect gate sucessed!");
            StarkSDK.API.GetAccountManager().Login(successCallback, failedCallback, true); 
        };
        
        _client.onGateConnectFaild += () => {
            Debug.Log("connect gate faild!");
        };
        _client.onHubConnect += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} sucessed!", hub_name));
        };
        _client.onHubConnectFaild += (hub_name) => {
            Debug.Log(string.Format("connect hub:{0} faild!", hub_name));
        };

    }

    private void _player_Client_Module_on_archive_sync(UserData obj)
    {
        //... do some thing
    }

    // Update is called once per frame
    void Update()
    {
        _client.poll();
    }


}
