using PlayerStates;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EvolutionType
{ 
    Attck,//增加子弹攻击力
    Num,//增加子弹数量
}

public class BuffDoor : MonoBehaviour
{
    private EvolutionType evolutionType;

    private void OnEnable()
    {
        evolutionType = (EvolutionType)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EvolutionType)).Length);//随机取一种进化类型
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Taoist_priest>() != null)
        {
            Taoist_priest._instance.ChangeBullet(evolutionType);
            PoolManager.GetInstance().PushObj(BuffDoorType.BuffDoors.ToString(), gameObject.transform.parent.gameObject);
        }
    }
}
