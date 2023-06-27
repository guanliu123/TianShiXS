using Abelkhan;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    public class RequestCenter
    {
        public UserData AddCoinReq(GameClient client,int n)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).add_coin(n).callBack((UserData data) =>
            {
               temp= data; 
            }, (err) =>
            {
               Debug.Log("请求发送失败，错误代码："+err);
            });
            if(temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData AddStrengthReq(GameClient client,int n)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).add_strength(10).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }
        
        public UserData AddSkillReq(GameClient client,Skill skill)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).add_skill(skill).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData AddMonsetrReq(GameClient client, Monster monster)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).add_monster(monster).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData CostCoinReq(GameClient client, int amount, EMCostCoinPath coinPath, int id)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_coin(amount,coinPath,id).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData CostStrengthReq(GameClient client, int amount)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_strength(amount).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData CostPropReq(GameClient client, int amount)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).cost_prop(amount).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }

        public UserData OpenChestReq(GameClient client, EMChestType type)
        {
            UserData temp = null;
            client._player_archive_Caller.get_hub(client._player_hub_name).open_chest(type).callBack((UserData data) =>
            {
                temp = data;
            }, (err) =>
            {
                Debug.Log("请求发送失败，错误代码：" + err);
            });
            if (temp != null)
            {
                return temp;
            }
            return null;
        }
    }
}

