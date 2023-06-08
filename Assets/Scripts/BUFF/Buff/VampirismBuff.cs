using System.Collections;
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

    public override (int, float) Init()
    {
        if (Random.Range(0, 100) > _probability * 100) return (0, 0);//buff没有触发 
        return (1, _duration);
    }

    public override void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        Debug.Log("吸血buff");
        //_attacker.GetComponent<CharacterBase>().ChangeHealth(_attacker,5f, HPType.Treatment);
    }

    private void Vampirism(GameObject _attacker,GameObject _taker)
    {
        Debug.Log("吸血5%");
    }
}
