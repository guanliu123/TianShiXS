﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuffManager : BaseManager<BuffManager>
{
    public Dictionary<BuffType, BuffBase> Buffs=new Dictionary<BuffType, BuffBase>();
    private Dictionary<BuffType, List<GameObject>> buffList=new Dictionary<BuffType, List<GameObject>>();//用于有持续作用的buff



    public BuffManager()
    {
        Buffs.Add(BuffType.Burn, new BurnBuff());
        Buffs.Add(BuffType.Poison, new PoisonBuff());
        Buffs.Add(BuffType.Vampirism, new VampirismBuff());
        Buffs.Add(BuffType.Frost, new BuffBase());//后续加了冰冻buff脚本要换上去
        Buffs.Add(BuffType.Crit, new BulletCrit());
        Buffs.Add(BuffType.Multiply, new BulletMultiply());
    }

    public void AddToBuffList(BuffType buffType, GameObject character)
    {
        //将传入的物体加入对应的buff列表，如果buffList不存在当前buff则在字典中增加，并且开启当前buff的携程
        if (character == null) return;
        if (!buffList.ContainsKey(buffType))
        {
            buffList.Add(buffType, new List<GameObject>());
            buffList[buffType].Add(character);
            MonoManager.GetInstance().StartCoroutine(Buffs[buffType].coroutineType, Buffs[buffType].OnSustain(buffList[buffType]));
        }
        else buffList[buffType].Add(character);
    }
    public void RemoveFromBuffList(BuffType buffType, GameObject character)
    {
        //将传入的物体移出对应的buff列表，如果移出后字典的相应列表为空，则将该buff移出字典，并且停止对应协程
        buffList[buffType].Remove(character);
        if (buffList[buffType].Count <= 0)
        {
            MonoManager.GetInstance().KillCoroutine(Buffs[buffType].coroutineType);
            buffList.Remove(buffType);
        }
    }
}