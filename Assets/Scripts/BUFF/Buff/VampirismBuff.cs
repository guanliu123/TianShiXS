using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirismBuff : BuffBase
{
    public VampirismBuff()
    {
        buffType = BuffType.Vampirism;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);
    }
    public override void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        _attacker.GetComponent<CharacterBase>().AddHP(5f);
    }
}
