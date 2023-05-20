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

    private Dictionary<DamageType, Color> colorDic = new Dictionary<DamageType, Color>();

    public void Awake()
    {
        mtext = GetComponent<Text>();
        mainCamera = Camera.main;
        colorDic.Add(DamageType.Default, Color.red);
    }

    public void Init(Transform point,float damage,DamageType damageType=DamageType.Default)
    {
        mtext.text = damage + "";
        mtext.color = colorDic[damageType];

        Vector3 randomOffset = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        mRect.position = RectTransformUtility.WorldToScreenPoint(mainCamera, point.position + randomOffset);

        float posY = transform.position.y + 3f;
        transform.DOMoveY(posY, 1f).OnComplete(() => { PoolManager.GetInstance().PushObj("FloatDamage", gameObject); });
    }
}
