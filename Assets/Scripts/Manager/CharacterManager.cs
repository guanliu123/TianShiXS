using Abelkhan;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeChatWASM;

public class CharacterManager : SingletonBase<CharacterManager>
{
    public Dictionary<int, CharacterMsg> roleMsgDic = new Dictionary<int, CharacterMsg>();
    public Dictionary<int, CharacterMsg> enemyMsgDic = new Dictionary<int, CharacterMsg>();
    //public Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<int, CharacterData> characterDatasDic = new Dictionary<int, CharacterData>();

    public CharacterManager()
    {
        //角色数据读取部分
        RoleDataTool.ReadRoleData();
        roleMsgDic = RoleDataTool.roleMsgDic;
        characterDatasDic = new Dictionary<int, CharacterData>(RoleDataTool.roleDataDic);
        if (GameManager.GetInstance().UserData.RoleList != null)
        {
            foreach (var item in GameManager.GetInstance().UserData.RoleList)
            {
                if (characterDatasDic.ContainsKey(item.RoleID))
                {
                    CharacterData t = new CharacterData();
                    CharacterData originData = characterDatasDic[item.RoleID];

                    t.level = item.Level;
                    t.MaxHP = originData.MaxHP + item.Heath;
                    t.ATK = originData.ATK + item.AttNum;
                    //t.ATKSpeed = originData.ATKSpeed + item.AttSpd;
                    t.ATKSpeed = originData.ATKSpeed;

                    characterDatasDic[item.RoleID] = t;
                }
            }
        }

        EnemyDataTool.ReadEnemyData();
        enemyMsgDic = EnemyDataTool.enemyMsgDic;        
        foreach (var item in EnemyDataTool.enemyDataDic)
        {
            characterDatasDic[item.Key] = item.Value;
        }
    }
    public void UpgradeRole(int id)
    {
        float increaseHP=0;
        float increaseATK=0;
        float increaseATKSpd=0;
        switch (id)
        {
            case 1001:increaseHP =10;increaseATK =15;increaseATKSpd =0; break;   
            case 1002: increaseHP =10; increaseATK =15; increaseATKSpd =0; break;   
            case 1003: increaseHP =10; increaseATK =15; increaseATKSpd =0; break;   
            case 1004: increaseHP =10; increaseATK =15; increaseATKSpd =0; break;   
            case 1005: increaseHP =20; increaseATK =20; increaseATKSpd =0; break;   
        }
        if (GameManager.GetInstance().UserData.RoleList != null)
        {
            foreach (var item in GameManager.GetInstance().UserData.RoleList)
            {
                if (item.RoleID == id)
                {
                    Role role = item;
                    role.Level++;
                    role.Heath += increaseHP;
                    role.AttNum += increaseATK;
                    //攻速

                    RequestCenter.SetRoleInfo(GameClient.Instance, role, (data) =>
                    {
                        GameManager.GetInstance().UserData = data;
                    });
                }
            }
        }     

        if (characterDatasDic.ContainsKey(id))
        {
            Debug.Log(2);
            CharacterData t = new CharacterData();
            CharacterData originData = characterDatasDic[id];

            t.level++;
            t.MaxHP = originData.MaxHP + increaseHP;
            t.ATK = originData.ATK + increaseATK;
            t.ATKSpeed = originData.ATKSpeed + increaseATKSpd;

            characterDatasDic[id] = t;
        }
        /*foreach(var item in GameManager.GetInstance().UserData.RoleList)
        {
            if (item.RoleID == id)
            {
                item.Heath += increaseHP;
                item.AttNum += increaseATK;
                //item.AttSpd += increaseATKSpd;
                if (characterDatasDic.ContainsKey(id))
                {
                    CharacterData t = new CharacterData();
                    CharacterData originData = characterDatasDic[item.RoleID];

                    t.MaxHP = originData.MaxHP + increaseHP;
                    t.ATK = originData.ATK + increaseATK;
                    t.ATKSpeed = originData.ATKSpeed + increaseATKSpd;

                    characterDatasDic[item.RoleID] = t;
                }
            }
        }*/
    }
}
