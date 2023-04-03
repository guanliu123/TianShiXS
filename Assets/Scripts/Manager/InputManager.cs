using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//继承自BaseManager
public class InputManager: BaseManager<InputManager>
{
    //输入开关
    private bool isStart = false;
    private float timer;

    //构造函数，对设置初始化
    public InputManager()
    {

        MonoManager.GetInstance().AddUpdateListener(MyUpDate);//帧更新，应该设置时间更新
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
        //在其中可设置你需要的输入方式，以下是我自己的定义
        FindMousePoint();//找到鼠标的点，并实现事件
        CheckHMove();//鼠标WS实现上下移动
        CheckVMove();//鼠标AD实现左右移动
        CheckKeyCode(KeyCode.J);//实现按下J的事件监听
  }

    //随时间更新
    void MyFixUpdate()
    {
        if (!isStart)
            return;
        CheckMouseClick(0.12f);
    }



    //检测鼠标输入
    void CheckMouseClick(float t)
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && timer >= t)
        {
            EventCenter.GetInstance().EventTrigger("Fire");
            timer = 0;
        }

    }

    //鼠标输入，得到点击的位置
    void FindMousePoint()
    {

        Vector3 point = Input.mousePosition;
        point.z = -10;
        point = Camera.main.ScreenToWorldPoint(point);

        Debug.Log(point);
        EventCenter.GetInstance().EventTrigger("FindMousePoint", point);
    }
    //检测WS键，执行VMove事件
    public void CheckVMove()
    {
        float v = Input.GetAxisRaw("Vertical");//对于WS

        EventCenter.GetInstance().EventTrigger("VMove", v);
    }
    //检测AD键，执行HMove事件
    public void CheckHMove()
    {
        float h = Input.GetAxisRaw("Horizontal");//对于AD
        EventCenter.GetInstance().EventTrigger("HMove", h);
    }
    //检测键盘输入，根据按压类别启动不同事件
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
