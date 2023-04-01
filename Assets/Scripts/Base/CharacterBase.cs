using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(float damage);
}

public class CharacterBase : MonoBehaviour,IDamage
{
    public CharacterData characterData;

    public CharacterType characterType;

    public ICharacterState currentState;
    public Dictionary<CharacterStateType, ICharacterState> statesDic = new Dictionary<CharacterStateType, ICharacterState>();

    public float maxHP;
    public float nowHP;
    public float ATK;
    public float ATKInterval;

    public Animator animator;

    protected void Awake()
    {
        animator = this.gameObject.GetComponent<Animator>();
    }

    protected void Start()
    {
        characterData = DataManager.GetInstance().AskCharacterData(characterType);
        InitData(characterType);
    }

    protected void OnEnable()//从对象池取出的时候会置为初始状态
    {
        nowHP = maxHP;
        TransitionState(CharacterStateType.Idle);
    }

    protected void InitData(CharacterType thisType)
    {
        maxHP = characterData.MaxHP;
        nowHP = maxHP;
        ATK = characterData.ATK;
        ATKInterval = characterData.ATKInterval;
    }

    protected void Update()
    {
        if (currentState != null) currentState.OnUpdate();
    }

    public void TakeDamage(float damage)
    {
        nowHP -= damage;
        if (nowHP <= 0) DiedEvent();
    }

    public void TransitionState(CharacterStateType characterStateType)
    {
        if (!statesDic.ContainsKey(characterStateType)) return;
        if (currentState != null)
            currentState.OnExit();
        currentState = statesDic[characterStateType];
        currentState.OnEnter();
    }

    public virtual void Attack()
    {
        
    }

    public virtual void DiedEvent()
    {
        PoolManager.GetInstance().PushObj(characterData.name, this.gameObject);
    }
}
