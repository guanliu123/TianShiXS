using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObject/技能池数据", order = 5)]
public class SkillSO : ScriptableObject
{
    public List<SkillDatas> skilldatas;
}
