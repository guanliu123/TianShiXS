using Abelkhan;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

public class ClientRoot : MonoBehaviour
{
    public static ClientRoot Instance { get; private set; }

    public static bool WXLoggedIn = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Debug.Log("Client Is Run");
        GameClient.Instance._client.connect_gate("wss://tsxs.ucat.games:3001", 3000);
        GameClient.Instance._client.onGateConnect += () =>
        {
            Debug.Log("onGateConnect begin!");
            WXlogin();
        };
        GameClient.Instance._client.onGateConnectFaild += () =>
        {
            Debug.Log("connect gate faild!");
        };
        GameClient.Instance._client.onHubConnect += (hub_name) =>
        {
            Debug.Log(string.Format("connect hub:{0} sucessed!", hub_name));
        };
        GameClient.Instance._client.onHubConnectFaild += (hub_name) =>
        {
            Debug.Log(string.Format("connect hub:{0} faild!", hub_name));
        };
    }

    public void WxSuccessLogin(string code , string nickName)
    {
        Debug.Log("WX登录成功");
        GameClient.Instance._client.get_hub_info("login", (hub_info) =>
        {
            Debug.Log("get_hub_info begin!");
            GameClient.Instance._login_Caller.get_hub(hub_info.hub_name).player_login_wx(code).callBack((string player_hub_name, string token) =>
            {
                Debug.Log("player_login_wx callback!");

                GameClient.Instance._player_hub_name = player_hub_name;
                GameClient.Instance._player_login_Caller.get_hub(player_hub_name).player_login(token, nickName).callBack((UserData data) =>
                {
                    Debug.Log($"player_login success!");
                    GameManager.GetInstance()._UserData = data;
                    WXLoggedIn = true;
                    //SceneSystem.Instance.SetScene(new StartScene());
                }, (err) =>
                {
                    Debug.Log($"player_login err:{err}");
                    if (err == (int)em_error.unregistered_palyer)
                    {
                        GameClient.Instance._player_login_Caller.get_hub(player_hub_name).create_role(token, nickName).callBack((UserData data) =>
                        {
                            Debug.Log($"create_role success!");
                            GameManager.GetInstance()._UserData = data;
                            WXLoggedIn = true;
                            //SceneSystem.Instance.SetScene(new StartScene());
                        }, (error) =>
                        {
                            Console.WriteLine("player_login_wx err:{0}", err);
                        });
                    }
                });
            }, (err) =>
            {
                Debug.Log($"player_login_dy err:{err}");
            });
            Debug.Log("get_hub_info end!");
        });
    }

    public void WXlogin()
    {
        LoginOption _callBack = new LoginOption();
        _callBack.success = (e) =>
        {
            Debug.Log($"_callBack success begin e.code:{e.code}!");
            if (e.code != null)
            {
                Debug.Log("_callBack success begin!");
                GetSettingOption _getSettingCallBack = new GetSettingOption();
                _getSettingCallBack.success = (res) =>
                {
                    if (res.authSetting["scope.userInfo"])
                    {
                        GetUserInfoOption _userInfoCallBack = new GetUserInfoOption();
                        _userInfoCallBack.lang = "zh_CN";
                        _userInfoCallBack.withCredentials = true;
                        _userInfoCallBack.success = (ee) =>
                        {
                            WxSuccessLogin(e.code, ee.userInfo.nickName);
                        };
                        WX.GetUserInfo(_userInfoCallBack);
                    }
                    else
                    {
                        var sysinfo = WX.GetSystemInfoSync();
                        var button = WX.CreateUserInfoButton((int)sysinfo.windowWidth / 2 - 50, (int)sysinfo.windowHeight / 2 - 50, 100, 60, "zh_CN", true);
                        button.OnTap((eee) =>
                        {
                            if(eee != null)
                            {
                                button.Destroy();
                                WxSuccessLogin(e.code, eee.userInfo.nickName);
                            }
                            else
                            {
                                ShowModalOption _showModalCallBack= new ShowModalOption();
                                _showModalCallBack.title = "温馨提示";
                                _showModalCallBack.content = "需要您的用户信息登录游戏！";
                                _showModalCallBack.showCancel = false;
                                WX.ShowModal(_showModalCallBack);
                            }
                           
                        });
                        button.Show();
                    }
                };
                WX.GetSetting(_getSettingCallBack);
            }
            else
            {
                Debug.Log("微信登录失败:" + e.errMsg);
            }
        };
        WX.Login(_callBack);
    }

    void Update()
    {
        GameClient.Instance._client.poll();
    }

}
