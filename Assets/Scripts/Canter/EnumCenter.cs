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
    }

public enum CharacterType
{
    Player,
    Bat,//普通蝙蝠小怪
    Eagle,//老鹰
    Laser,//激光怪物
    Coins,//铜钱boss
    TaoWu,//梼杌boss
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
    BaseSword,
    AttackSword,
    MoreSword,
    NormalBullet,
}

public enum CameraPointType
{
    MainPoint,
}

