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
    Bat,//��ͨ����С��
    Eagle,//��ӥ
    Laser,//�������
    Coins,//ͭǮboss
    TaoWu,//���boss
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
    BaseSword,
    AttackSword,
    MoreSword,
    NormalBullet,
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

