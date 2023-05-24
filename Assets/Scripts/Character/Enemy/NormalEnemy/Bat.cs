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

            characterEvent += CheckDistancce;
            characterEvent += Attack;
        }


        public void CheckDistancce()
        {
            if ((Player._instance.transform.position - transform.position).z > 1.5f) Recovery();
        }
    }

}