using EnemyStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Turtle : CharacterBase
{
    void Awake()
    {
        base.Awake();
        //characterID = CharacterType.Bat;
        characterID = 2004;
        //AddCharacterEvent(VisibleCheck);

        statesDic.Add(CharacterStateType.Idle, new NormalIdleState(this));
        statesDic.Add(CharacterStateType.Attack, new NormalAttackState(this));

        InitData();

        characterPassiveEvent += CheckDistance;
        characterActiveEvent += Attack;
    }
}
