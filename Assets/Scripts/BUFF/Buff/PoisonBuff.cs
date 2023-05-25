using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBuff : BuffBase
{
    public PoisonBuff()
    {
        buffType = BuffType.Poison;
        coroutineType = CoroutineType.PoisonBuff;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);

        triggerInterval = 1f;
        _probability = buffData.probability;
        _duration = buffData.duration;
    }
    public override (int,float) OnAdd(GameObject _attacker, GameObject bullet, GameObject _taker)
    {
        if (Random.Range(0, 100) > _probability * 100) return (0, 0);//buff没有触发 
        BuffManager.GetInstance().AddToBuffList(buffType, _taker);
        return (BulletManager.GetInstance().BulletBuffs[bullet.GetComponent<BulletBase>().bulletType][BuffType.Poison]
            ,_duration);
    }

    public override void OnUpdate(GameObject _taker)
    {
        IAttack t = _taker.GetComponent<IAttack>();
        if (!_taker) return;
        int n = _taker.GetComponent<CharacterBase>().buffDic[buffType].Item1;
        t.ChangeHealth(-7 * n,HPType.Poison);
    }

    public override (int, float) OnZero(GameObject _taker, int plies = 1)
    {
        return (plies - 1, buffData.duration);
    }

    public override (int, float) OnSuperpose(GameObject _attacker, GameObject _taker, int plies = 1)
    {
        return (1, buffData.duration);
    }

    public override void OnEnd(GameObject _taker)
    {
        BuffManager.GetInstance().RemoveFromBuffList(buffType, _taker);
    }
}
