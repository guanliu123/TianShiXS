using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuffManager : BaseManager<BuffManager>
{
    //public Dictionary<BuffType, IBuff> Buffs;
    private Dictionary<BuffType, List<IBuff>> buffList;//用于有持续作用的buff

    public BuffManager()
    {
        //Buffs.Add(BuffType.Burn, new BurnBuff());

    }

    public void AddToBuffList(BuffType buffType)
    {
        //将传入的物体加入对应的buff列表，如果buffList不存在当前buff则在字典中增加，并且开启当前buff的携程
    }
    public void RemoveFromBuffList(BuffType buffType)
    {
        //将传入的物体移出对应的buff列表，如果移出后字典的相应列表为空，则将该buff移出字典，并且停止对应协程
    }
}
