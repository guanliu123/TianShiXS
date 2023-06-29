using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : BuffBase
{
    public ShieldBuff()
    {
        //buffID = BuffType.Shield;
        coroutineType = CoroutineType.ShieldBuff;
        //buffData = DataManager.GetInstance().AskBuffDate(buffType);
        buffData = BuffManager.BuffDic[buffID];
        triggerInterval = 5f;
        _probability = buffData.probability;
        _duration = buffData.duration;
    }
    public override (int, float) Init()
    {
        if (Random.Range(0, 100) > _probability * 100) return (0, 0);//buff没有触发 
        return (1, _duration);
    }

    public override void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        BuffManager.GetInstance().AddToBuffList(buffID, _taker);
    }

    public override void OnUpdate(GameObject _taker)
    {
        //Debug.Log("附加护盾");
        GameObject t = PoolManager.GetInstance().GetObj(PropType.Shield.ToString(), ResourceType.Prop);

        if (!t) return;
        ObjTimer timer = t.GetComponent<ObjTimer>();
        if (!timer) timer = t.AddComponent<ObjTimer>();
        timer.Init(PropType.Shield.ToString(), 2.5f);
    }
}
