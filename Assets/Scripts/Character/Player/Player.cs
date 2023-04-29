using PlayerStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public static Player _instance = new Player();

    private BulletType nowBullet = BulletType.BaseSword;
    
    // Start is called before the first frame update
    void Awake()
    {
        base.Awake();
        _instance = this;

        this.characterType = GameManager.GetInstance().nowPlayerType;
        characterTag = "Player";
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
        /*GameObject t = PoolManager.GetInstance().GetObj(nowBullet.ToString());
        t.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f, gameObject.transform.position.z);

        IInitBullet initBullet = t.GetComponent<IInitBullet>();
        initBullet.InitInfo(ATK);*/

        BulletManager.GetInstance().ShootBullet(gameObject.transform, nowBullet, ATK);
    }

    public void ChangeBullet(EvolutionType evolutionType)
    {
        switch (evolutionType)
        {
            case EvolutionType.Attck: nowBullet = BulletType.AttackSword; break;
            case EvolutionType.Num: nowBullet = BulletType.MoreSword; break;
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
