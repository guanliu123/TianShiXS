using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public static Player _instance = new Player();

    //private BulletType nowBullet = BulletType.BaseSword;
    
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        _instance = this;

        this.characterType = GameManager.GetInstance().nowPlayerType;
        characterTag = "Player";
        statesDic.Add(CharacterStateType.Idle, new IdleState(this));
        statesDic.Add(CharacterStateType.Attack, new AttackState(this));

        characterActiveEvent += Attack;
        InitData();
    }

    public void InitPlayer()
    {
        this.characterType = GameManager.GetInstance().nowPlayerType;
        characterTag = "Player";
        InitData();
    }
    public void ClearPlayer()
    {
        nowBullet.Clear();
        bulletTimer.Clear();
    }

    public override void Attack()
    {
        /*GameObject t = PoolManager.GetInstance().GetObj(nowBullet.ToString());
        t.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f, gameObject.transform.position.z);

        IInitBullet initBullet = t.GetComponent<IInitBullet>();
        initBullet.InitInfo(ATK);*/

        //BulletManager.GetInstance().ShootBullet(gameObject.transform, nowBullet, ATK);
        base.Attack();
    }

    public void BulletEvolute(BuffType bulletEvolutionType)
    {
        foreach (var item in nowBullet)
        {
            BulletManager.GetInstance().BulletEvolute(bulletEvolutionType, item.Key);
        }
    }
}
namespace PlayerStates
{
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
            //character.AddCharacterEvent(character.Attack);
        }

        public void OnExit()
        {
            timer = 0;
        }

        public void OnUpdate()
        {
            /*timer += Time.deltaTime;
            if (timer > character.ATKInterval)
            {
                character.TransitionState(CharacterStateType.Attack);
            }*/
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
