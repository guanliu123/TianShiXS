using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public enum ResourceType
    {
        Audio,
        MapGround,
        Enemy,
        Bullet,
        MapItem,
        UI,
        Prop,
        Player,
        Null,
        Effect,
        Skybox,
    }

/*public enum CharacterType
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
    Turtle,
    Claw,
}*/
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

public enum MapItemType
{
    BuffDoors,
}

/*public enum BulletType
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
    PoisonBullet,//����
    PerpetualBullet,//������Ļ
    FireBall,
    RotateBullet,
    BounceBullet,
    NormalBullet,
    IceBullet,
    PerpetualBullet_Base,
}*/

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
    Shield
}

/*public enum BuffType
{
    
    Burn,//ȼ��
    Frost,//����
    Poison,//��ҩ
    Vampirism,//��Ѫ
    Multiply,//����
    Crit,//����
    NULL,
    Reflect,//�����˺�
    Shield,
}*/

public enum CoroutineType {//������Ҫ�Ĺ̶�Э���¼������buff��������ע��ź���Э�̹�����ͳһ���Ȳ��Ҵ����ֵ�
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