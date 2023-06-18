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
    BigMouse,
    TaoWu,//梼杌boss
    JinYiWei,
    KuiJia,
    XiaKe,
    ShiXu,
    Turtle,
    Claw,
}
public enum CharacterTag {
    Null,
    Player,
    Enemy,
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
    
    Combat,
    TaiChiDart,
    BaseBullet,
    TrackingBullet,
    LaserBullet,
    PoisonousFloor,
    GasBomb,
    Fanshaped,
    NULL,
    FlowerDart,//毒镖
    PerpetualBullet,//连击弹幕
    FireBall,
    RotateBullet,
    BounceBullet,
    NormalBullet
}

public enum CameraPointType
{
    MainPoint,
    HighPoint,
    OrginPoint,
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
    Vampirism,//吸血
    Multiply,//倍增
    Crit,//暴击
    NULL,
    Reflect,//反弹伤害
    Shield,
}

public enum CoroutineType {//各种主要的固定协程事件如火烧buff，在这里注册才好在协程管理中统一调度并且存入字典
    BurnBuff,
    PoisonBuff,
    ShieldBuff,
}

public enum HPType
{
    Default,
    Treatment,
    Burn,
    Poison,
    Crit,
}