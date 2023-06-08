using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : BuffBase
{
    public ShieldBuff()
    {
        buffType = BuffType.Shield;
        coroutineType = CoroutineType.ShieldBuff;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);

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
        BuffManager.GetInstance().AddToBuffList(buffType, _taker);
    }

    public override void OnUpdate(GameObject _taker)
    {
        Debug.Log("附加护盾");
    }
}
