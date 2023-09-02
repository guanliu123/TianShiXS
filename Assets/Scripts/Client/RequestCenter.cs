using Abelkhan;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RequestCenter
    {
        public static void AddCoinReq(GameClient client,int n, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).add_coin(n).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
               Debug.Log("请求发送失败，错误代码："+err);
            });
        }

        public static void AddStrengthReq(GameClient client,int n, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).add_strength(10).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void AddSkillReq(GameClient client,Skill skill, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).add_skill(skill).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void AddMonsetrReq(GameClient client, Monster monster, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).add_monster(monster).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }
        public static void SetLevelReq(GameClient client,int level, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).set_level(level).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void CostCoinReq(GameClient client, int amount, EMCostCoinPath coinPath, int id, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_coin(amount,coinPath,id).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void CostStrengthReq(GameClient client, int amount, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_strength(amount).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void CostPropReq(GameClient client, int amount, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_prop(amount).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }

        public static void OpenChestReq(GameClient client, EMChestType type, Action<UserData> cb)
        {
            client._player_archive_Caller.get_hub(client._player_hub_name).open_chest(type).callBack((UserData data) =>
            {
                cb.Invoke(data);
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
        }
    }
}

