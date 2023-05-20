using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : CharacterBase
{
    void Awake()
    {
        base.Awake();

        this.characterType = CharacterType.Laser;
        characterTag = "Enemy";
        statesDic.Add(CharacterStateType.Idle, new IdleState(this));
        statesDic.Add(CharacterStateType.Attack, new AttackState(this));

        InitData();
    }
}
