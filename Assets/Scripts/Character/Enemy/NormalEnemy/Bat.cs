using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyStates;

namespace Bat {
    public class Bat : CharacterBase
    {
        // Start is called before the first frame update
        void Awake()
        {
            base.Awake();
            characterType = CharacterType.Bat;
            characterTag = "Enemy";
            //AddCharacterEvent(VisibleCheck);
            
            statesDic.Add(CharacterStateType.Idle, new NormalIdleState(this));
            statesDic.Add(CharacterStateType.Attack, new NormalAttackState(this));

            InitData();

            characterPassiveEvent += CheckDistance;
            characterActiveEvent += Attack;
        }
    }

}
