﻿using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMouse : CharacterBase
{
    void Awake()
    {
        base.Awake();

        //this.characterID = CharacterType.BigMouse;
        characterTag = CharacterTag.Enemy;
        statesDic.Add(CharacterStateType.Idle, new IdleState(this));
        statesDic.Add(CharacterStateType.Attack, new AttackState(this));

        InitData();
    }
}
