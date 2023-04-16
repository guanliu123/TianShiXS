using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyStates {
    public class NormalIdleState : ICharacterState
    {
        CharacterBase character;
        float distance;

        public NormalIdleState(CharacterBase character)
        {
            this.character = character;
        }

        public void OnEnter()
        {
            character.animator.Play("Idle");
        }

        public void OnExit()
        {

        }

        public void OnUpdate()
        {
            distance = (character.gameObject.transform.position - Taoist_priest._instance.gameObject.transform.position).magnitude;
            if (distance < 30)
            {
                character.TransitionState(CharacterStateType.PrepareAttack);
            }
        }
    }
    public class NormalPrepareAttackState : ICharacterState
    {
        CharacterBase character;

        float timer = 0f;

        bool isRotate = true;
        float angleSpeed = 1f;

        public NormalPrepareAttackState(CharacterBase character)
        {
            this.character = character;
        }
        public void OnEnter()
        {
            character.animator.Play("Idle");
        }

        public void OnExit()
        {
            timer = 0;
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
            character.gameObject.transform.LookAt(Taoist_priest._instance.transform);
            if (timer > character.ATKInterval)
            {
                character.TransitionState(CharacterStateType.Attack);
            }
        }
    }

    public class NormalAttackState : ICharacterState
    {
        CharacterBase character;

        public NormalAttackState(CharacterBase character)
        {
            this.character = character;
        }
        public void OnEnter()
        {
            character.animator.Play("Attack");
            character.Attack();
            character.TransitionState(CharacterStateType.PrepareAttack);
        }

        public void OnExit()
        {
        }

        public void OnUpdate()
        {
        }
    }

}
