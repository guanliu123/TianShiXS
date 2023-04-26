using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamage
{
    void TakeDamage(float damage,Transform damagePoint);
    void ShowDamage(float damage,Transform point);
}

public class CharacterBase : MonoBehaviour, IDamage
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
    public HPSlider hpSlider;

    protected event UnityAction characterEvent;

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
        if (characterType != CharacterType.Player) GameManager.GetInstance().EnemyIncrease();
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
        if(characterEvent!=null) characterEvent();
    }

    public void TakeDamage(float damage,Transform damagePoint)
    {
        nowHP -= damage;
        if (hpSlider) hpSlider.UpdateHPSlider(maxHP, nowHP);
        ShowDamage(damage, damagePoint);
        if (nowHP <= 0) DiedEvent();
    }
    public void ShowDamage(float damage,Transform point)
    {
        GameObject obj = PoolManager.GetInstance().GetObj("DamageText");

        obj.transform.position = point.position;
        obj.GetComponent<TextMesh>().text = damage + "";

        Vector3 cameraPoint = new Vector3(Screen.width / 2, 0, Screen.height / 2);
        obj.transform.LookAt(Camera.main.ScreenToWorldPoint(cameraPoint));

        float posY = obj.transform.position.y + 1f;
        obj.transform.DOMoveY(posY, 1f).OnComplete(() => { PoolManager.GetInstance().PushObj("DamageText", obj); });
    }

    public void TransitionState(CharacterStateType characterStateType)
    {
        if (!statesDic.ContainsKey(characterStateType)) return;
        if (currentState != null)
            currentState.OnExit();
        currentState = statesDic[characterStateType];
        currentState.OnEnter();
    }

    public void Recovery()
    {
        if (!gameObject.GetComponentInChildren<SkinnedMeshRenderer>().isVisible)
        {
            PoolManager.GetInstance().PushObj(characterData.name, this.gameObject);
        }
    }

    public virtual void Attack()
    {
        
    }

    public virtual void DiedEvent()
    {
        PoolManager.GetInstance().PushObj(characterData.name, this.gameObject);
        if (characterType != CharacterType.Player)
        {
            GameManager.GetInstance().ChangeEnergy(characterData.energy);//改成读入数据中的增长能量值 
            FallMoney();
        }
    }

    private void OnDisable()
    {
        GameManager.GetInstance().EnemyDecrease();
    }

    public void FallMoney()
    {
        GameManager.GetInstance().ChangeMoney(characterData.money);
        //生成金币的方式
    }

    public void SetHP()
    {
        throw new System.NotImplementedException();
    }
}
