using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    //void OnNewly();//当该buff新增到子弹附带的效果列表中时
    void OnAdd(GameObject _attacker, GameObject _bullet, GameObject _taker);
    void OnUpdate(GameObject _taker);//持续的时候

    (int, float) OnSuperpose(GameObject _attacker, GameObject _taker, int plies = 1);//进化的时候

    (int, float) OnZero(GameObject _taker, int plies=1);//当某一层buff归零的时候，可能会进行掉层等操作而不是直接移除buff

    IEnumerator OnSustain(List<GameObject> _taker);//给有持续作用buff用的，一般是存在一大堆人会被作用到，为了节省性能才有这个方法，所以传入list

    void OnEnd(GameObject _taker);
}

public class BuffBase : IBuff
{
    public BuffType buffType;
    public CoroutineType coroutineType;//给假如存在持续作用效果的buff开的协程类型
    public BuffData buffData;
    protected float triggerInterval;

    /*public virtual void OnNewly()
    {

    }*/

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
        return (plies, 0);
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