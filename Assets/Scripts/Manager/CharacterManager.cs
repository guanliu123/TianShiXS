using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public Dictionary<int, CharacterMsg> roleMsgDic = new Dictionary<int, CharacterMsg>();
    public Dictionary<int, CharacterMsg> enemyMagDic = new Dictionary<int, CharacterMsg>();
    //public Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();
    public Dictionary<int, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<int, Dictionary<int, CharacterData>>();

    public CharacterManager()
    {
        RoleDataTool.ReadRoleData();
        roleMsgDic = RoleDataTool.roleMsgDic;
        characterDatasDic = new Dictionary<int, Dictionary<int, CharacterData>>(RoleDataTool.roleDataDic);
        enemyMagDic = EnemyDataTool.enemyMsgDic;
        EnemyDataTool.ReadEnemyData();
        foreach(var item in EnemyDataTool.enemyDataDic)
        {
            characterDatasDic.Add(item.Key, item.Value);
        }
    }
}
