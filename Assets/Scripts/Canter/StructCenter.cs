using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skills;

#region ��Դ·�������������
[Serializable]
public struct ResourceData//
{
    public string name;
    public string path;
}

[Serializable]
public struct ResourceDatas
{
    [Header("��Դ����")] public ResourceType resourceType;
    [Header("��Դ����������·��")] public List<ResourceData> resourceNPs;
}
#endregion

#region ��ɫ�������
[Serializable]
public struct CharacterData
{
    [Header("��ɫ��ǩ")] public CharacterTag tag;
    [Header("��ɫ����")] public string name;
    [Header("��ɫ����")] public GameObject icon;
    [Header("��ɫ���Ѫ��")] public float MaxHP;
    [Header("��ɫ������ʽ")] public List<BulletType> bulletTypes;
    [Header("��ɫ�����������ӳ�")] public float Aggressivity;
    [Header("��ɫ�������ټӳ�")] public float ATKSpeed;
    [Header("��ɫ������")] public float avoidance;
    [Header("��ɫ������������")] public float energy;
    [Header("��ɫ����������")] public int money;
    [Header("��ɫ����")] public string describe;
}

[Serializable]
public struct CharacterDatas{
    [Header("��ɫ����")] public CharacterType characterType;
    
    public CharacterData characterData;
}
#endregion

#region �ӵ���������ʽ���������
[Serializable]
public struct BulletData
{
    [Header("����Ƶ��")] public float transmissionFrequency;
    [Header("�Ƿ����漴������")] public bool isRandomShoot;
    [Header("�ӵ�����ʱ��")] public float existTime;
    //[Header("�ӵ��ܷ��ƶ�")] public bool isMovable;
    [Header("�ƶ��ٶ�")] public float moveSpeed;
    //[Header("�ƶ�ʱ��")] public float moveTime;
    //[Header("ֹͣʱ��")] public float stopTime;
    //[Header("�ӵ��ܷ���ת")] public bool isRotatable;
    [Header("��ת�ٶ�")] public float rotateSpeed;
    //[Header("��ת���")] public float rotateInterval;
    [Header("��ʼ���������б�")] public List<BuffType> buffList;
    [Header("�ɽ�������")] public List<BuffType> evolvableList;
    [Header("������(�ٷ��ƣ���������ͬ")] public int crit;
    [Header("��������")] public float critRate;
    [Header("����������")] public float ATK;
    [Header("�˺����")] public float damageInterval;
    [Header("�Ƿ�����������")] public bool isFollowShooter;
    [Header("���伸��")] public float shootProbability;
}
[Serializable]
public struct BulletDatas
{
    [Header("�ӵ�����")] public BulletType bulletType;
    public BulletData bulletData;
}

#endregion

#region Buff����
[Serializable]
public struct BuffData
{
    [Header("buffͼ��")] public Sprite icon;
    [Header("����ʱ��")] public float duration;
    [Header("��������")] public float probability;
}

[Serializable]
public struct BuffDatas
{
    [Header("Buff����")] public BuffType buffType;
    public BuffData buffData;
}
#endregion

#region ������ؽṹ��
public struct SkillUpgrade
{
    public Sprite icon;//��Ӧ���ܵ�ͼ��
    public string name;//��Ӧ��������
    public string describe;//��Ӧ���ܵ�����
    public ISkill skill;
}
[Serializable]
public struct SkillDatas
{
    [Header("����ID")] public int id;
    [Header("��������")] public string name;
    [Header("����ͼ��")] public Sprite[] icon;
    [Header("��������")] public string describe;
    [Header("���ܳ��ָ���")] public float probability;
    [Header("���ܿɳ��ִ���")] public int num;
    [Header("ǰ�ü���id�б�")] public List<int> beforeSkills;
    //[Header("����Ʒ��")]
}
#endregion

#region �ؿ�����
[Serializable]
public struct StageData
{
    [Header("�׶α��")] public int StageNum;
    [Header("�Ƿ���������׶�")] public bool isSpecial;
    [Header("�Ƿ�bossս")] public bool isBoss;
    [Header("BOSS����")] public CharacterType[] BOSSType;
    [Header("�ؿ����μ���������")] public int[] WaveEnemyNum;
    [Header("ÿ����������")] public CharacterType[] WaveEnemyType;
}

[Serializable] 
public struct LevelData
{
    [Header("�ؿ���ͨ��������")] public List<GameObject> normalPlanes;
    [Header("�ؿ���ͨ���泤�Ϳ�")] public float[] normalSize;
    [Header("�ؿ������������")] public List<GameObject> widthPlanes;
    [Header("�ؿ�������泤�Ϳ�")] public float[] widthSize;
    [Header("�ؿ���պ�")] public Material skybox;
    /*[Header("�ؿ���������")] public List<CharacterType> enemyTypes;
    [Header("�ؿ��ܲ���")] public int waveNum;
    [Header("���ε��˻�������")] public int baseEnemyNum;
    [Header("������������")] public int riseEnemyNum;
    [Header("����buff�Ż�������")] public int baseBuffDoorNum;
    [Header("����buff����������")] public int riseBuffDoorNum;
    [Header("�ؿ�boss")] public CharacterType bossType;*/
    [Header("�ؿ��׶�")] public List<StageData> StageDatas;
}

[Serializable]
public struct LevelDatas
{
    [Header("�ؿ����")] public int id;
    public LevelData levelData;
}

#endregion