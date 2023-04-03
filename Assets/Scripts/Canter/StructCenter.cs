using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 资源路径名称相关数据
[Serializable]
public struct ResourceData//
{
    public string name;
    public string path;
}

[Serializable]
public struct ResourceDatas
{
    [Header("资源类型")] public ResourceType resourceType;
    [Header("资源名称与所在路径")] public List<ResourceData> resourceNPs;
}
#endregion

#region 角色相关数据
[Serializable]
   public struct CharacterData
{
    [Header("角色名称")] public string name;
    [Header("角色最大血量")] public float MaxHP;
    [Header("角色攻击方式")] public List<BulletType> bulletTypes;
    [Header("角色攻击力")] public float ATK;
    [Header("角色攻击间隔")] public float ATKInterval;
}

[Serializable]
public struct CharacterDatas{
    [Header("角色类型")] public CharacterType characterType;
    public CharacterData characterData;
}
#endregion

#region 子弹（攻击方式）相关数据
[Serializable]
public struct BulletData
{
    [Header("子弹名称")] public string bulletName;
    [Header("子弹存在时间")] public float existTime;
    [Header("子弹能否移动")] public bool isMovable;
    [Header("移动速度")] public float moveSpeed;
    [Header("移动时间")] public float moveTime;
    [Header("停止时间")] public float stopTime;
    [Header("子弹能否旋转")] public bool isRotatable;
    [Header("旋转速度")] public float rotateSpeed;
    [Header("旋转间隔")] public float rotateInterval;
}
[Serializable]
public struct BulletDatas
{
    [Header("子弹类型")] public BulletType bulletType;
    public BulletData bulletData;
}

#endregion

#region 关卡数据
[Serializable] 
public struct LevelData
{
    [Header("关卡敌人类型")] public List<CharacterType> enemyTypes;
    [Header("关卡总波次")] public int num;
    [Header("波次敌人基础数量")] public int baseEnemyNum;
    [Header("敌人增长数量")] public int riseEnemyNum;
    [Header("关卡boss")] public CharacterType characterType;
}

[Serializable]
public struct LevelDatas
{
    [Header("关卡序号")] public int id;
    public LevelData levelData;
}

#endregion