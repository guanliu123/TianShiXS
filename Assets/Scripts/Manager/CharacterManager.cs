using Abelkhan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : SingletonBase<CharacterManager>
{
    public Dictionary<int, CharacterMsg> roleMsgDic = new Dictionary<int, CharacterMsg>();
    public Dictionary<int, CharacterMsg> enemyMagDic = new Dictionary<int, CharacterMsg>();
    //public Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<int, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<int, Dictionary<int, CharacterData>>();

    public CharacterManager()
    {
        //角色数据读取部分
        RoleDataTool.ReadRoleData();
        roleMsgDic = RoleDataTool.roleMsgDic;
        characterDatasDic = new Dictionary<int, Dictionary<int, CharacterData>>(RoleDataTool.roleDataDic);
        if (GameManager.GetInstance()._UserData.RoleList != null)
        {
            foreach (var item in GameManager.GetInstance()._UserData.RoleList)
            {
                if (characterDatasDic.ContainsKey(item.RoleID))
                {
                    CharacterData t = new CharacterData();
                    CharacterData originData = characterDatasDic[item.RoleID][0];

                    t.MaxHP = originData.MaxHP + item.Heath;
                    t.ATK = originData.ATK + item.AttNum;
                    t.ATKSpeed = originData.ATKSpeed + item.AttSpd;

                    characterDatasDic[item.RoleID][0] = t;
                }
            }
        }
        
        enemyMagDic = EnemyDataTool.enemyMsgDic;
        EnemyDataTool.ReadEnemyData();
        foreach (var item in EnemyDataTool.enemyDataDic)
        {
            characterDatasDic.Add(item.Key, item.Value);
        }
    }
    public void UpgradeRole(int id, float increaseHP, float increaseATK,float increaseATKSpd)
    {
        foreach(var item in GameManager.GetInstance()._UserData.RoleList)
        {
            if (item.RoleID == id)
            {
                item.Heath += increaseHP;
                item.AttNum += increaseATK;
                item.AttSpd += increaseATKSpd;
                if (characterDatasDic.ContainsKey(id))
                {
                    CharacterData t = new CharacterData();
                    CharacterData originData = characterDatasDic[item.RoleID][0];

                    t.MaxHP = originData.MaxHP + increaseHP;
                    t.ATK = originData.ATK + increaseATK;
                    t.ATKSpeed = originData.ATKSpeed + increaseATKSpd;

                    characterDatasDic[item.RoleID][0] = t;
                }
            }

            UserData.UserData_to_protcol(GameManager.GetInstance()._UserData);
        }
    }
}
