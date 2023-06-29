using EnemyStates;
using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : CharacterBase
{
    void Awake()
    {
        base.Awake();

        //this.characterID = CharacterType.Laser;
        characterID = 2002;
        characterTag = CharacterTag.Enemy;

        statesDic.Add(CharacterStateType.Idle, new LaserIdleState(this));
        statesDic.Add(CharacterStateType.Attack, new LaserAttackState(this));

        InitData();

        characterPassiveEvent += CheckDistance;
        characterActiveEvent += Attack;     
    }
}
