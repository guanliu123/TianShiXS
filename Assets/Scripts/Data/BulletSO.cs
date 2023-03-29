using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "ScriptableObject/子弹数据（各种攻击方式数据）", order = 3)]
public class BulletSO : ScriptableObject
{
    public List<BulletDatas> bulletdatas;
}
