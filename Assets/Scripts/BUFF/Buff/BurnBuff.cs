﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BurnBuff : BuffBase
{
    public BurnBuff()
    {
        buffType = BuffType.Burn;
        coroutineType = CoroutineType.BurnBuff;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);

        triggerInterval = 0.5f;
    }
    public override void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        BuffManager.GetInstance().AddToBuffList(buffType,_taker);
    }

    public override void OnUpdate(GameObject _taker)
    {
        IAttack t = _taker.GetComponent<IAttack>();
        if (!_taker) return;
        //int n = _taker.GetComponent<CharacterBase>().buffDic[buffType].Item1;
        t.TakeDamage(5);
    }

    public override void OnEnd(GameObject _taker)
    {
        BuffManager.GetInstance().RemoveFromBuffList(buffType, _taker);
    }
}