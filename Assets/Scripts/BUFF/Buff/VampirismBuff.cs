﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirismBuff : BuffBase
{
    public VampirismBuff()
    {
        buffType = BuffType.Vampirism;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);

        _probability = buffData.probability;
        _duration = buffData.duration;
    }
    public override (int,float) OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        if (Random.Range(0, 100) > _probability * 100) return (0, 0);//buff没有触发 

        _attacker.GetComponent<CharacterBase>().ChangeHealth(5f, HPType.Treatment);
        return (1, _duration);
    }
}
