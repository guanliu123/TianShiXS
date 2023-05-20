using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffSO", menuName = "ScriptableObject/Buff数据（基础数据）", order = 4)]
public class BuffSO : ScriptableObject
{
    public List<BuffDatas> buffdatas;
}
