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
    BigMouse,
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
    
    Combat,
    TaiChiDart,
    BaseBullet,
    TrackingBullet,
    LaserBullet,
    PoisonousFloor,
    GasBomb,
    Fanshaped,
    NULL,
    FlowerDart,//����
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
    
    Burn,//ȼ��
    Frost,//����
    Poison,//��ҩ
    Vampirism,//��Ѫ
    Multiply,//����
    Crit,//����
    NULL,
}

public enum SkillType
{
    
}

public enum CoroutineType {//������Ҫ�Ĺ̶�Э���¼������buff��������ע��ź���Э�̹�����ͳһ���Ȳ��Ҵ����ֵ�
    BurnBuff,
    PoisonBuff,
}

public enum HPType
{
    Default,
    Treatment,
    Burn,
    Poison,
    Crit,
}