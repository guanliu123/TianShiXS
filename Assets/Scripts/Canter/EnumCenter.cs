using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum ResourceType
    {
        Music,
        MapSquare,
        Enemy,
        Bullet,
        BuffDoor,
        UI,
        Prop,
        Player,
    }

public enum CharacterType
{
    DaoShi,
    Bat,//普通蝙蝠小怪
    Eagle,//老鹰
    Laser,//激光怪物
    Coins,//铜钱boss
    TaoWu,//梼杌boss
    JinYiWei,
    KuiJia,
    XiaKe,
    ShiXu,
}

public enum CharacterStateType
{
    Idle,
    PrepareAttack,
    Attack
}

public enum BuffDoorType
{
    BuffDoors,
}

public enum BulletType
{
    NULL,
    Sword,
    BaseBullet,
    TrackingBullet,
}

public enum CameraPointType
{
    MainPoint,
    HighPoint,
}

public enum PropType
{
    Money,
    HuLu,
    DaoQi,
    Sword,
    Bell,
    Banners,
}

public enum BuffType
{
    Burn,//燃烧
    Frost,//冰冻
    Poison,//毒药
    Multiply,//倍增
    Crit,//暴击
}

public enum CoroutineType {//各种主要的固定协程事件如火烧buff，在这里注册才好在协程管理中统一调度并且存入字典
    BurnBuff,
    PoisonBuff,
}