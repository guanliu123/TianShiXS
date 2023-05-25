using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            distance = (character.gameObject.transform.position - Player._instance.gameObject.transform.position).magnitude;
            if (distance < 30)
            {
                character.gameObject.transform.LookAt(Player._instance.transform);
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
            character.TransitionState(CharacterStateType.Idle);
        }

        public void OnExit()
        {
        }

        public void OnUpdate()
        {
        }
    }
    public class LaserIdleState : ICharacterState
    {
        CharacterBase character;
        bool isMove;
        float moveSpeed = 2f;
        float rotateSpeed = 1f;
        Vector3 target;

        public LaserIdleState(CharacterBase character)
        {
            this.character = character;
        }

        public void OnEnter()
        {
            isMove = false;
        }

        public void OnExit()
        {
            
        }

        public void OnUpdate()
        {
            Vector3 direction = Player._instance.gameObject.transform.position - character.transform.position;
            direction.y = 0; // 只在水平面上旋转

            // 计算旋转角度
            Quaternion rotation = Quaternion.LookRotation(direction);

            // 旋转物体
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rotation, Time.deltaTime * rotateSpeed);

            if (!isMove)
            {
                float randomDistance;
                do
                {
                    randomDistance = Random.Range(-5, 5) + character.transform.position.x;

                } while (Mathf.Abs(randomDistance) >= 4.5f);
                target = new Vector3(randomDistance, character.transform.position.y, character.transform.position.z);

                isMove = true;
            }
            else
            {
                character.transform.position = Vector3.MoveTowards(character.transform.position, target, moveSpeed * Time.deltaTime);
                if ((character.transform.position - target).magnitude < 0.1)
                {
                    /*pauseTimer += Time.deltaTime;
                    if (pauseTimer >= pauseTime)
                    {*/
                        isMove = false;
/*                        pauseTimer = 0f;
                    }*/
                }
            }
        }
    }

    public class LaserAttackState : ICharacterState
    {
        CharacterBase character;
        float timer;
        

        public LaserAttackState(CharacterBase character)
        {
            this.character = character;
        }

        public void OnEnter()
        {
            timer = 2f;
        }

        public void OnExit()
        {

        }

        public void OnUpdate()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                character.TransitionState(CharacterStateType.Idle);
            }
        }
    }
}
