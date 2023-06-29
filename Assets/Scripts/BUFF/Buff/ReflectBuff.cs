using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBuff : BuffBase
{
    public ReflectBuff()
    {
       // buffID = BuffType.Reflect;
        //buffData = DataManager.GetInstance().AskBuffDate(buffType);
        buffData = BuffManager.BuffDic[buffID];
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
        var t = _taker.GetComponent<CharacterBase>();
        if (t) t.attackedEvent += ReflectDamage;
    }  
    public override void OnEnd(GameObject _taker)
    {
        var t = _taker.GetComponent<CharacterBase>();
        if (t) t.attackedEvent -= ReflectDamage;
    }

    public void ReflectDamage(GameObject _attacker, GameObject _taker,float variation, HPType hpType)
    {
        if (hpType != HPType.Treatment)
        {
            var t = _attacker.GetComponent<CharacterBase>();
            if (t) t.ChangeHealth(null,(int)variation*0.25f);
        }
    }
}
