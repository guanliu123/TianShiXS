using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "ScriptableObject/关卡数据", order = 3)]
public class LevelSO : ScriptableObject
{
    public List<LevelDatas> leveldatas;
}