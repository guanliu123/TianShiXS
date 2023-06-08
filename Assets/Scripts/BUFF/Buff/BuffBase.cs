using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase
{
    public BuffType buffType;
    public CoroutineType coroutineType;//给假如存在持续作用效果的buff开的协程类型
    public BuffData buffData;

    public float _duration;
    public float triggerInterval;
    public float _probability;

    public virtual (int, float) Init() {
        return (0, 0);
    }

    public virtual void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker)
    {
    }

    public virtual void OnEnd(GameObject _taker)
    {
        
    }

    public virtual (int, float) OnZero(GameObject _taker,int plies = 1)
    {
        return (0, 0);
    }

    public virtual (int,float) OnSuperpose(GameObject _attacker, GameObject _taker, int plies = 1)
    {
        return (0, 0);
    }

    public IEnumerator OnSustain(List<GameObject> _takers)
    {
        while (_takers.Count > 0)
        {
            for(int i = 0; i < _takers.Count; i++)
            {
                if (_takers[i] == null)
                {
                    _takers.RemoveAt(i);
                    continue;
                }
                OnUpdate(_takers[i]);
            }
            yield return new WaitForSeconds(triggerInterval);
        }
        MonoManager.GetInstance().CoroutineStopped(coroutineType);
    }

    public virtual void OnUpdate(GameObject _taker)
    {
        
    }
}