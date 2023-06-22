using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForstBuff : BuffBase
{
    public ForstBuff()
    {
        buffType = BuffType.Frost;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);


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
        IAttack t = _taker.GetComponent<IAttack>();
        if (t==null) return;

        t.TakeMove(-100, _duration);
    }

    public override void OnEnd(GameObject _taker)
    {
        CharacterBase t = _taker.GetComponent<CharacterBase>();

        t.canActive = true;
        t.animator.speed = 1;
    }
}
