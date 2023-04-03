using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSO", menuName = "ScriptableObject/所有角色数据", order = 1)]
public class CharacterSO : ScriptableObject
{
    public List<CharacterDatas> characterdatas;
}

