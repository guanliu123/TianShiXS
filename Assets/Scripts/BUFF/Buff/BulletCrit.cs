using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCrit : BuffBase
{
    public override int buffID { get; set; } = 4001;

    //每种子弹对应的下次暴击几率和暴击倍率
    private Dictionary<int, (float, float)> critDic = new Dictionary<int, (float, float)>();
    public float critGrowthRate;

    public BulletCrit()
    {
        //buffID = BuffType.Crit;
        //buffData = DataManager.GetInstance().AskBuffDate(buffType);
        buffData = BuffManager.BuffDic[buffID];
        critGrowthRate = 0.05f;
        _probability = buffData.probability;
        _duration = buffData.duration;
    }

    public override (int, float) Init()
    {
        if (Random.Range(0, 100) > _probability * 100) return (0, 0);//buff没有触发 

        return (1, _duration);
    }

    public override void  OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
        int t = Random.Range(0, 100);
        int _id = _bullet.GetComponent<BulletBase>().bulletID;
        if (!critDic.ContainsKey(_id)) critDic.Add(_id, 
            (BulletManager.GetInstance().BulletDic[_id].crit, BulletManager.GetInstance().BulletDic[_id].critRate));

        if (t < (critDic[_id].Item1+GameManager.GetInstance().critProbability)*100)
        {
            //如果触发暴击了就重置几率
            _bullet.GetComponent<BulletBase>().isCrit = true;
            critDic[_id] = (BulletManager.GetInstance().BulletDic[_id].crit, BulletManager.GetInstance().BulletDic[_id].critRate);
        }
        else
        {
            critDic[_id] = (critDic[_id].Item1 +critGrowthRate, critDic[_id].Item2);
        }       
    }
}
