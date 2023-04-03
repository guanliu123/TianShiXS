using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using static UnityEngine.GraphicsBuffer;

namespace Bat {
    public class Bat : CharacterBase
    {
        // Start is called before the first frame update
        void Awake()
        {
            base.Awake();
            characterType = CharacterType.Bat;
            statesDic.Add(CharacterStateType.Idle, new IdleState(this));
            statesDic.Add(CharacterStateType.PrepareAttack, new PrepareAttackState(this));
            statesDic.Add(CharacterStateType.Attack, new AttackState(this));
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }
    }

    #region 蝙蝠的行动状态类
    public class IdleState : ICharacterState
    {
        CharacterBase character;
        float distance;

        public IdleState(CharacterBase character)
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
            if (distance < 30 && distance > 0.0001f)
            {
                character.TransitionState(CharacterStateType.PrepareAttack);
            }
        }
    }
    public class PrepareAttackState : ICharacterState
    {
        CharacterBase character;

        float timer = 0f;

        bool isRotate = true;
        float angleSpeed = 1f;

        public PrepareAttackState(CharacterBase character)
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

    public class AttackState : ICharacterState
    {
        CharacterBase character;

        public AttackState(CharacterBase character)
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
#endregion  
}
