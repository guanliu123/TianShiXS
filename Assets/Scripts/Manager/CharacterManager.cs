using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : BaseManager<CharacterManager>
{
    public Dictionary<CharacterType, CharacterMsg> roleMsgDic = new Dictionary<CharacterType, CharacterMsg>();
    public Dictionary<CharacterType, CharacterMsg> enemyMagDic = new Dictionary<CharacterType, CharacterMsg>();
    public Dictionary<CharacterType, Dictionary<int, CharacterData>> characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>();

    public CharacterManager()
    {
        RoleDataTool.ReadRoleData();
        roleMsgDic = RoleDataTool.roleMsgDic;
        characterDatasDic = new Dictionary<CharacterType, Dictionary<int, CharacterData>>(RoleDataTool.roleDataDic);
        enemyMagDic = EnemyDataTool.enemyMsgDic;
        EnemyDataTool.ReadEnemyData();
        foreach(var item in EnemyDataTool.enemyDataDic)
        {
            characterDatasDic.Add(item.Key, item.Value);
        }
    }
}
