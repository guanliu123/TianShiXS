using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixiu :CharacterBase
{
    void Awake()
    {
        base.Awake();

        this.characterID = 2006;
        characterTag = CharacterTag.Enemy;
        statesDic.Add(CharacterStateType.Idle, new IdleState(this));
        statesDic.Add(CharacterStateType.Attack, new AttackState(this));

        InitData();
    }
}
