using Bat;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BulletBase : MonoBehaviour
{
    public BulletData bulletData;
    public int bulletID;
    //public int bulletID;
    public Dictionary<int, int> nowBuffs=new Dictionary<int, int>();
    public GameObject attacker;

    public UnityAction bulletAction;

    protected string targetTag;
    protected string ignoreTag;

    protected float bulletATK;
    private float existTimer = 0;

    public bool isCrit;

    protected void Start()
    {
        bulletAction += RecoveryTimer;
        //bulletAction += AttackCheck;
    }

    public virtual void InitBullet(GameObject _attacker,CharacterTag _tag,BulletData _bulletData,Dictionary<int, int> buffs)
    {
        attacker = _attacker;
        bulletData = _bulletData;
        nowBuffs = buffs;

        ignoreTag = _tag.ToString();
        if (_tag == CharacterTag.Player) targetTag = "Enemy";
        else if (_tag == CharacterTag.Enemy) targetTag = "Player";
        else targetTag = "";
    }

    protected void OnEnable()
    {
        OnEnter();
    }
    protected void Update()
    {
        if (bulletAction != null) bulletAction();
    }

    public virtual void OnEnter()
    {
        if (GameManager.GetInstance().enemyList.Count <= 0)
        {
            Recovery();
            return;
        }

        SpecialEvolution();
    }
    public virtual void OnExit()
    {

    }

    public virtual void Move()
    {
        gameObject.transform.Translate(transform.forward * bulletData.moveSpeed * Time.deltaTime);
    }
    public virtual void Rotat()
    {

    }  

    protected virtual void AttackCheck(GameObject obj)
    {
        IAttack targetIAttck = obj.GetComponent<IAttack>();
        if (targetIAttck == null) return;

        foreach (var item in nowBuffs)
        {
            targetIAttck.TakeBuff(attacker, gameObject, item.Key, item.Value);
            BuffManager.GetInstance().OnBuff(obj, item.Key);
        }
        if (isCrit)
        {
            targetIAttck.ChangeHealth(attacker, -bulletData.ATK *
                (1 + (float)(bulletData.critRate + GameManager.GetInstance().critRate) / 100), HPType.Crit);
            isCrit = false;
        }
        else { targetIAttck.ChangeHealth(attacker, -bulletData.ATK); }
        GameManager.GetInstance().GenerateEffect(obj.transform, bulletData.effectPath);
        AudioManager.GetInstance().PlaySound(bulletData.audioPath,false);
    }
    protected virtual void SpecialEvolution()
    {
        if (!attacker) return;
        var t = attacker.GetComponent<CharacterBase>();
        if (!t || t.characterTag != CharacterTag.Player) return;
    }

    protected void RecoveryTimer()
    {
        existTimer += Time.deltaTime;
        if (existTimer < bulletData.existTime) return;

        Recovery();
    }
    protected void Recovery()
    {
        existTimer = 0;
        OnExit();
        PoolManager.GetInstance().PushObj(bulletID.ToString(), this.gameObject);
    }
}