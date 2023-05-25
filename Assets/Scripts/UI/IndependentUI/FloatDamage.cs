using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class FloatDamage : MonoBehaviour
{
    private Text mtext;
    private RectTransform mRect;
    private Camera mainCamera;

    private Dictionary<HPType, Color> colorDic = new Dictionary<HPType, Color>();

    public void Awake()
    {
        mtext = GetComponent<Text>();
        mRect = GetComponent<RectTransform>();
        mainCamera = Camera.main;

        colorDic.Add(HPType.Default, new Color(255,0,0));
        colorDic.Add(HPType.Treatment, new Color(0, 255, 0));
        colorDic.Add(HPType.Burn, new Color(255,117,0));
        colorDic.Add(HPType.Poison, new Color(17, 140, 100));
        colorDic.Add(HPType.Crit, new Color(255,215,0));
    }

    public void Init(Transform point,float damage,HPType hpType=HPType.Default)
    {
        if(damage<0) mtext.text = "" + damage;
        else mtext.text = "+" + damage;

        mtext.color = colorDic[hpType];

        Vector3 randomOffset = new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f, 0.5f), 0);
        mRect.position = RectTransformUtility.WorldToScreenPoint(mainCamera, point.position + randomOffset);

        float posY = transform.position.y + 1f;
        transform.DOMoveY(posY, 1f).OnComplete(() => { PoolManager.GetInstance().PushObj("FloatDamage", gameObject); });
    }
}
