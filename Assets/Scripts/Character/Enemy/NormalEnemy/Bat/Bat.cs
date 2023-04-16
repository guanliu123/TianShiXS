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
            statesDic.Add(CharacterStateType.Idle, new NormalIdleState(this));
            statesDic.Add(CharacterStateType.PrepareAttack, new NormalPrepareAttackState(this));
            statesDic.Add(CharacterStateType.Attack, new NormalAttackState(this));
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }

        public override void Attack()
        {
            BulletManager.GetInstance().ShootBullet(gameObject.transform,
                characterData.bulletTypes[Random.Range(0, characterData.bulletTypes.Count)],
                characterData.ATK);
        }
    }

    #region 蝙蝠的行动状态类
   
#endregion  
}
