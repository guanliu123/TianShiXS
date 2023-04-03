using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourcepathSO", menuName = "ScriptableObject/导入数据的路径所在数据", order = 0)]
public class ResourcepathSO : ScriptableObject
{
    public List<ResourceDatas> resourcePaths;
}
