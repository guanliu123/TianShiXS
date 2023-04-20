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
    Bat,//��ͨ����С��
    Eagle,//��ӥ
    Laser,//�������
    Coins,//ͭǮboss
    TaoWu,//���boss
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

