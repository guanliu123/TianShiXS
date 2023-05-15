using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Header("��ɫ���Ѫ��")] public float MaxHP;
    [Header("��ɫ������ʽ")] public List<BulletType> bulletTypes;
    [Header("��ɫ�����������ӳ�")] public float Aggressivity;
    [Header("��ɫ�������ټӳ�")] public float ATKSpeed;
    [Header("��ɫ������������")] public float energy;
    [Header("��ɫ����������")] public int money;
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
    [Header("������")] public float crit;
    [Header("��������")] public float critRate;
    [Header("����������")] public float baseATK;
}
[Serializable]
public struct BulletDatas
{
    [Header("�ӵ�����")] public BulletType bulletType;
    public BulletData bulletData;
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
    [Header("�ؿ������������")] public List<GameObject> widthPlanes;
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