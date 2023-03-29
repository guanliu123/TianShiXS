using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�̳���BaseManager
public class InputManager: BaseManager<InputManager>
{
    //���뿪��
    private bool isStart = false;
    private float timer;

    //���캯���������ó�ʼ��
    public InputManager()
    {

        MonoManager.GetInstance().AddUpdateListener(MyUpDate);//֡���£�Ӧ������ʱ�����
        MonoManager.GetInstance().AddFixUpdateListener(MyFixUpdate);
    }

    public void StartOrEndCheck(bool isOpen)
    {
        isStart = isOpen;
    }

    private void MyUpDate()
    {
        if (!isStart)
            return;
        //�����п���������Ҫ�����뷽ʽ�����������Լ��Ķ���
        FindMousePoint();//�ҵ����ĵ㣬��ʵ���¼�
        CheckHMove();//���WSʵ�������ƶ�
        CheckVMove();//���ADʵ�������ƶ�
        CheckKeyCode(KeyCode.J);//ʵ�ְ���J���¼�����
  }

    //��ʱ�����
    void MyFixUpdate()
    {
        if (!isStart)
            return;
        CheckMouseClick(0.12f);
    }



    //����������
    void CheckMouseClick(float t)
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= t)
        {
            EventCenter.GetInstance().EventTrigger("Fire");
            timer = 0;
        }

    }

    //������룬�õ������λ��
    void FindMousePoint()
    {

        Vector3 point = Input.mousePosition;
        point.z = -10;
        point = Camera.main.ScreenToWorldPoint(point);

        Debug.Log(point);
        EventCenter.GetInstance().EventTrigger("FindMousePoint", point);
    }
    //���WS����ִ��VMove�¼�
    public void CheckVMove()
    {
        float v = Input.GetAxisRaw("Vertical");//����WS

        EventCenter.GetInstance().EventTrigger("VMove", v);
    }
    //���AD����ִ��HMove�¼�
    public void CheckHMove()
    {
        float h = Input.GetAxisRaw("Horizontal");//����AD
        EventCenter.GetInstance().EventTrigger("HMove", h);
    }
    //���������룬���ݰ�ѹ���������ͬ�¼�
    public void CheckKeyCode(KeyCode key)
    {
        if (Input.GetKeyDown(key))
            EventCenter.GetInstance().EventTrigger("KeyIsDown", key);
        if (Input.GetKey(key))
            EventCenter.GetInstance().EventTrigger("KeyAlwaysDown", key);
        if (Input.GetKeyUp(key))
            EventCenter.GetInstance().EventTrigger("KeyIsOpen", key);
    }

}
