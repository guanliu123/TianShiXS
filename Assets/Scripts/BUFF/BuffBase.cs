using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    void OnAdd();
    void OnSustain();

    void OnSuperpose();
    void OnEnd();
}

public class BuffBase : IBuff
{
    
    public GameObject attacker;//施加buff的人
    public GameObject manager;//buff拥有者
    public BuffType buffType;//当前buff的类型
    public int stacks;//buff的层数（比如毒buff，层数影响持续伤害值）
    public float duration;//持续时间若为0则buff为瞬间结算型
    
    public BuffBase(GameObject _attacker,GameObject _manager,int _stacks)
    {
        attacker = _attacker;
        manager = _manager;
        stacks = _stacks;
    }

    public virtual void OnAdd()
    {
        
    }

    public virtual void OnEnd()
    {
       
    }

    public void OnSuperpose()
    {
        
    }

    public virtual void OnSustain()
    {
        
    }
}