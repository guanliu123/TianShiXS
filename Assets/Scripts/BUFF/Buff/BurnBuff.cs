using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurnBuff : BuffBase
{
    public BurnBuff()
    {
        buffType = BuffType.Burn;
        coroutineType = CoroutineType.BurnBuff;
        //buffData = DataManager.GetInstance().AskBuffDate(buffType);
        buffData = BuffManager.BuffDic[buffType];

        triggerInterval = 0.5f;
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
        BuffManager.GetInstance().AddToBuffList(buffType,_taker);
    }

    public override void OnUpdate(GameObject _taker)
    {
        IAttack t = _taker.GetComponent<IAttack>();
        if (!_taker) return;

        t.ChangeHealth(null,-5f,HPType.Burn);
    }

    public override void OnEnd(GameObject _taker)
    {
        BuffManager.GetInstance().RemoveFromBuffList(buffType, _taker);
    }
}
