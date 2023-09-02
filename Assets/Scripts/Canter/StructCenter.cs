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
    //[Header("��ǰ���������ؿ�")] public int levelNum;
    [Header("��ɫ���Ѫ��")] public float MaxHP;
    [Header("��ɫ������ʽ")] public List<int> bulletList;
    [Header("��ɫ�����������ӳ�")] public float ATK;
    [Header("��ɫ�������ټӳ�")] public float ATKSpeed;
    //[Header("��ɫ������")] public float avoidance;
    [Header("��ɫ������������")] public float energy;
    [Header("��ɫ����������")] public int money;
}

[Serializable]
public struct CharacterMsg
{
    //[Header("��ɫ��ǩ")] public CharacterTag tag;
    [Header("��ɫ����")] public string name;
    //[Header("��ɫ����")] public GameObject image;
    [Header("��ɫ����·��")] public string imagePath;

    [Header("��ɫ����")] public string describe;
}

[Serializable]
public struct CharacterDatas
{
    [Header("��ɫID")] public int id;
    //[Header("��ɫ����")] public CharacterType characterType;
    //[Header("��ɫ��ǩ")] public CharacterTag characterTag;
    public CharacterMsg characterMsg;
    public List<CharacterData> characterData;
}
#endregion

#region �ӵ���������ʽ���������
[Serializable]
public struct BulletData
{
    [Header("����Ƶ��")] public float transmissionFrequency;
    [Header("�Ƿ����漴������")] public bool isRandomShoot;
    [Header("�ӵ�����ʱ��")] public float existTime;
    [Header("�ƶ��ٶ�")] public float moveSpeed;
    [Header("��ת�ٶ�")] public float rotateSpeed;
    [Header("��ʼ���������б�")] public List<int> buffList;
    [Header("�ɽ�������")] public List<int> evolvableList;
    [Header("������(�ٷ��ƣ���������ͬ")] public float crit;
    [Header("��������")] public float critRate;
    [Header("����������")] public float ATK;
    [Header("�˺����")] public float damageInterval;
    [Header("�Ƿ�����������")] public bool isFollowShooter;
    [Header("���伸��")] public float shootProbability;
    [Header("������Ч")] public string audioPath;
    [Header("������Ч")] public string effectPath;
}
[Serializable]
public struct BulletDatas
{
    [Header("�ӵ�ID")] public int bulletID;
    //[Header("�ӵ�����")] public BulletType bulletType;
    public BulletData bulletData;
}

#endregion

#region Buff����
[Serializable]
public struct BuffData
{
    //[Header("buffͼ��")] public Sprite icon;
    [Header("buff��Ч")] public string audioPath;
    [Header("������Ч")] public string effectPath;
    [Header("����ʱ��")] public float duration;
    [Header("��������")] public float probability;
}

[Serializable]
public struct BuffDatas
{
    [Header("Buff����")] public int buffID;
    public BuffData buffData;
}
#endregion

#region ������ؽṹ��
public struct SkillUpgrade
{
    //public Sprite iconPath;//��Ӧ���ܵ�ͼ��
    public string iconPath;//��Ӧ���ܵ�ͼ��

    public string name;//��Ӧ��������
    public string describe;//��Ӧ���ܵ�����
    public string quality;
    public bool isNew;//�Ƿ����¼���
    public SkillBase skill;
}
[Serializable]
public struct SkillData
{
    [Header("����ID")] public int id;
    [Header("��������")] public string name;
    //[Header("����ͼ��")] public Sprite iconPath;
    [Header("����ͼ��")] public string iconPath;

    [Header("��������")] public string describe;
    [Header("���ܳ��ָ���")] public float probability;
    [Header("����Ʒ��")] public string quality;
    [Header("���ܿɳ��ִ���")] public int num;
    [Header("ǰ�ü���id�б�")] public List<int> beforeSkills;
    //[Header("����Ʒ��")]
}
#endregion

#region �ؿ�����
[Serializable]
public struct StageData
{
    [Header("�׶α��")] public int StageID;
    [Header("�Ƿ���������׶�")] public bool isSpecial;
    //[Header("�Ƿ�bossս")] public bool isBoss;
    [Header("BOSS����")] public List<int> BOSSList;
    [Header("�ؿ����μ���������")] public List<int> WaveEnemyNum;
    [Header("ÿ����������")] public List<int> WaveEnemyList;
}

[Serializable]
public struct LevelData
{
    public int energy;
    [Header("�ؿ���ͨ��������")] public List<string> normalPlanes;
    [Header("�ؿ���ͨ���泤�Ϳ�")] public float[] normalSize;
    [Header("�ؿ������������")] public List<string> widthPlanes;
    [Header("�ؿ�������泤�Ϳ�")] public float[] widthSize;
    [Header("�ؿ���պ�")] public string skyboxName;
    [Header("�ؿ��׶�")] public List<StageData> StageDatas;
}

[Serializable]
public struct LevelDatas
{
    [Header("�ؿ����")] public int id;
    public LevelData levelData;
}

#endregion