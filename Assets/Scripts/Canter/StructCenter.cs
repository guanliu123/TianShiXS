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
    [Header("��ɫ����")] public string name;
    [Header("��ɫ���Ѫ��")] public float MaxHP;
    [Header("��ɫ������ʽ")] public List<BulletType> bulletTypes;
    [Header("��ɫ������")] public float ATK;
    [Header("��ɫ�������")] public float ATKInterval;
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
    [Header("�ӵ�����")] public string bulletName;
    [Header("�ӵ�����ʱ��")] public float existTime;
    [Header("�ӵ��ܷ��ƶ�")] public bool isMovable;
    [Header("�ƶ��ٶ�")] public float moveSpeed;
    [Header("�ƶ�ʱ��")] public float moveTime;
    [Header("ֹͣʱ��")] public float stopTime;
    [Header("�ӵ��ܷ���ת")] public bool isRotatable;
    [Header("��ת�ٶ�")] public float rotateSpeed;
    [Header("��ת���")] public float rotateInterval;
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
public struct LevelData
{
    [Header("�ؿ���������")] public List<CharacterType> enemyTypes;
    [Header("�ؿ��ܲ���")] public int num;
    [Header("���ε��˻�������")] public int baseEnemyNum;
    [Header("������������")] public int riseEnemyNum;
    [Header("�ؿ�boss")] public CharacterType characterType;
}

[Serializable]
public struct LevelDatas
{
    [Header("�ؿ����")] public int id;
    public LevelData levelData;
}

#endregion