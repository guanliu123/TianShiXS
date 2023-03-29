using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Taoist_priest : CharacterBase
    {
        public static Taoist_priest _instance = new Taoist_priest();

        // Start is called before the first frame update
        void Awake()
        {
            base.Awake();
            _instance = this;
           
            this.characterType = CharacterType.Player;
            statesDic.Add(CharacterStateType.Idle, new IdleState(this));
            statesDic.Add(CharacterStateType.Attack, new AttackState(this));
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
            PoolManager.GetInstance().GetObj("Bullet", "Sword").transform.position=this.gameObject.transform.position;
        }
    }

    #region 角色的行动类

    public class IdleState : ICharacterState
    {
        CharacterBase character;
        float timer;

        public IdleState(CharacterBase character)
        {
            this.character = character;
        }

        public void OnEnter()
        {
            //character.animator.Play("Idle");
        }

        public void OnExit()
        {
            timer = 0;
        }

        public void OnUpdate()
        {
            timer += Time.deltaTime;
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
            //character.animator.Play("Attack");
            character.Attack();
            character.TransitionState(CharacterStateType.Idle);
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
