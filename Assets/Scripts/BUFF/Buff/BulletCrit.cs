using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCrit : BuffBase
{
    //每种子弹对应的下次暴击几率和暴击倍率
    private Dictionary<BulletType, (float, float)> critDic = new Dictionary<BulletType, (float, float)>();
    public float critGrowthRate;

    public BulletCrit()
    {
        buffType = BuffType.Crit;
        buffData = DataManager.GetInstance().AskBuffDate(buffType);

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
        BulletType _type = _bullet.GetComponent<BulletBase>().bulletType;
        if (!critDic.ContainsKey(_type)) critDic.Add(_type, 
            (BulletManager.GetInstance().BulletDic[_type].crit, BulletManager.GetInstance().BulletDic[_type].critRate));

        if (t < (critDic[_type].Item1+GameManager.GetInstance().critProbability)*100)
        {
            //如果触发暴击了就重置几率
            _bullet.GetComponent<BulletBase>().isCrit = true;
            critDic[_type] = (BulletManager.GetInstance().BulletDic[_type].crit, BulletManager.GetInstance().BulletDic[_type].critRate);
        }
        else
        {
            critDic[_type] = (critDic[_type].Item1 +critGrowthRate, critDic[_type].Item2);
        }       
    }
}
