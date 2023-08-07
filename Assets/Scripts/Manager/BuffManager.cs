using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BuffManager : BaseManager<BuffManager>
{
    public static Dictionary<int, BuffData> BuffDic = new Dictionary<int, BuffData>();
    public Dictionary<int, BuffBase> BuffEvent = new Dictionary<int, BuffBase>();
    private Dictionary<int, List<GameObject>> buffList = new Dictionary<int, List<GameObject>>();//用于有持续作用的buff

    static BuffManager()
    {
        BuffDic = BuffDataTool.ReadBuffData();
    }

    public BuffManager()
    {
        InitBuffEvent();
    }

    public void AddToBuffList(int buffID, GameObject character)
    {
        //将传入的物体加入对应的buff列表，如果buffList不存在当前buff则在字典中增加，并且开启当前buff的携程
        if (character == null) return;
        if (!buffList.ContainsKey(buffID))
        {
            buffList.Add(buffID, new List<GameObject>());
            buffList[buffID].Add(character);
            MonoManager.GetInstance().StartCoroutine(BuffEvent[buffID].coroutineType, BuffEvent[buffID].OnSustain(buffList[buffID]));
        }
        else buffList[buffID].Add(character);
    }
    public void RemoveFromBuffList(int buffID, GameObject character)
    {
        //将传入的物体移出对应的buff列表，如果移出后字典的相应列表为空，则将该buff移出字典，并且停止对应协程
        if (!buffList.ContainsKey(buffID) || !buffList[buffID].Contains(character)) return;
        buffList[buffID].Remove(character);
        if (buffList[buffID].Count <= 0)
        {
            MonoManager.GetInstance().KillCoroutine(BuffEvent[buffID].coroutineType);
            buffList.Remove(buffID);
        }
    }

    public void OnBuff(GameObject taker, int buffID)
    {
        string effect = BuffDic[buffID].effectPath;
        //GameManager.GetInstance().GenerateEffect(taker.transform, effect, true, 0.3F);
    }

    private void InitBuffEvent()
    {
        int buffNum = BuffDic.Count;
        int n = 0;

        Type baseType = typeof(BuffBase);
        Assembly assembly = Assembly.GetExecutingAssembly();
        Type[] types = assembly.GetTypes();

        foreach (Type type in types)
        {
            if (type.IsSubclassOf(baseType))
            {
                BuffBase buff = (BuffBase)Activator.CreateInstance(type);
                // 在这里可以对skill进行进一步的操作
                if (!BuffEvent.ContainsKey(buff.buffID)) BuffEvent.Add(buff.buffID, buff);
                n++;
            }
            if (n >= buffNum) break;
        }
    }
}