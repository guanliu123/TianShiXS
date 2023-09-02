using EnemyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : CharacterBase
{
    void Awake()
    {
        base.Awake();

        //this.characterID = CharacterType.Laser;
        characterID = 2005;
        characterTag = CharacterTag.Enemy;

        statesDic.Add(CharacterStateType.Idle, new LaserIdleState(this));
        statesDic.Add(CharacterStateType.Attack, new LaserAttackState(this));

        InitData();

        characterPassiveEvent += CheckDistance;
        characterActiveEvent += Attack;
    }
}
