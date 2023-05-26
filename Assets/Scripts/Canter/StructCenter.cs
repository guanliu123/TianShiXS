using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skills;

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
    [Header("角色标签")] public string tag;
    [Header("角色名称")] public string name;
    [Header("角色立绘")] public Image icon;
    [Header("角色最大血量")] public float MaxHP;
    [Header("角色攻击方式")] public List<BulletType> bulletTypes;
    [Header("角色基础攻击力加成")] public float Aggressivity;
    [Header("角色基础攻速加成")] public float ATKSpeed;
    [Header("角色死亡增加能量")] public float energy;
    [Header("角色死亡掉落金币")] public int money;
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
    [Header("发射频率")] public float transmissionFrequency;
    [Header("是否向随即方向发射")] public bool isRandomShoot;
    [Header("子弹存在时间")] public float existTime;
    //[Header("子弹能否移动")] public bool isMovable;
    [Header("移动速度")] public float moveSpeed;
    //[Header("移动时间")] public float moveTime;
    //[Header("停止时间")] public float stopTime;
    //[Header("子弹能否旋转")] public bool isRotatable;
    [Header("旋转速度")] public float rotateSpeed;
    //[Header("旋转间隔")] public float rotateInterval;
    [Header("初始附带特殊列表")] public List<BuffType> buffList;
    [Header("可进化方向")] public List<BuffType> evolvableList;
    [Header("暴击率(百分制，暴击倍率同")] public int crit;
    [Header("暴击倍率")] public float critRate;
    [Header("基础攻击力")] public float baseATK;
    [Header("伤害间隔")] public float damageInterval;
    [Header("是否跟随射出物体")] public bool isFollowShooter;
    [Header("发射几率")] public float shootProbability;
}
[Serializable]
public struct BulletDatas
{
    [Header("子弹类型")] public BulletType bulletType;
    public BulletData bulletData;
}

#endregion

#region Buff数据
[Serializable]
public struct BuffData
{
    [Header("buff图标")] public Image icon;
    [Header("持续时间")] public float duration;
    [Header("触发几率")] public float probability;
}

[Serializable]
public struct BuffDatas
{
    [Header("Buff类型")] public BuffType buffType;
    public BuffData buffData;
}
#endregion

#region 技能相关结构体
public struct SkillUpgrade
{
    public Image icon;//对应技能的图标
    public string name;//对应技能名字
    public string describe;//对应技能的描述
    public ISkill skill;
}
[Serializable]
public struct SkillDatas
{
    [Header("技能ID")] public int id;
    [Header("技能名字")] public string name;
    [Header("技能图标")] public Image icon;
    [Header("技能描述")] public string describe;
    [Header("技能出现概率")] public float probability;
    [Header("技能可出现次数")] public int num;
    [Header("前置技能id列表")] public List<int> beforeSkills;
    //[Header("技能品级")]
}
#endregion

#region 关卡数据
[Serializable]
public struct StageData
{
    [Header("阶段编号")] public int StageNum;
    [Header("是否属于特殊阶段")] public bool isSpecial;
    [Header("是否boss战")] public bool isBoss;
    [Header("BOSS类型")] public CharacterType[] BOSSType;
    [Header("关卡波次及敌人数量")] public int[] WaveEnemyNum;
    [Header("每波敌人类型")] public CharacterType[] WaveEnemyType;
}

[Serializable] 
public struct LevelData
{
    [Header("关卡普通地面类型")] public List<GameObject> normalPlanes;
    [Header("关卡特殊地面类型")] public List<GameObject> widthPlanes;
    /*[Header("关卡敌人类型")] public List<CharacterType> enemyTypes;
    [Header("关卡总波次")] public int waveNum;
    [Header("波次敌人基础数量")] public int baseEnemyNum;
    [Header("敌人增长数量")] public int riseEnemyNum;
    [Header("波次buff门基础数量")] public int baseBuffDoorNum;
    [Header("波次buff门增长数量")] public int riseBuffDoorNum;
    [Header("关卡boss")] public CharacterType bossType;*/
    [Header("关卡阶段")] public List<StageData> StageDatas;
}

[Serializable]
public struct LevelDatas
{
    [Header("关卡序号")] public int id;
    public LevelData levelData;
}

#endregion