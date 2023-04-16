using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    Slider hp;
    public Camera cameraToLookAt;

    // Start is called before the first frame update
    private void Awake()
    {
        hp = this.gameObject.GetComponent<Slider>();
        gameObject.transform.parent.parent.GetComponent<CharacterBase>().hpSlider = this;
        cameraToLookAt = Camera.main;
    }
    private void OnEnable()
    {
        hp.value = 1;
    }

    private void Update()
    {
        //使用  Vector3.ProjectOnPlane （投影向量，投影平面法向量）用于计算某个向量在某个平面上的投影向量  
        Vector3 lookPoint = Vector3.ProjectOnPlane(gameObject.transform.transform.position - Camera.main.transform.position, Camera.main.transform.forward);
        gameObject.transform.LookAt(Camera.main.transform.position + lookPoint);
    }

    public void UpdateHPSlider(float maxHP,float nowHP)
    {
        hp.value = nowHP / maxHP;
    }
}
