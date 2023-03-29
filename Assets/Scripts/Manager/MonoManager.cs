using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : BaseManager<MonoManager>
{
    //Ϊ���ܹ�ʹ�ò�����MonoController,����������Ҫһ��MonoController����
    private MonoController controller;

    //�����ǵ���ģʽ�����Ǵ������캯��������һЩ��Ҫ�ĳ�ʼ��
    public MonoManager()
    {
        //MonoController�����ǵ���ģʽ���´���һ����Ϸ������ΪMonoController�����monocontroller
        GameObject obj = new GameObject("MonoController");
        //���ؽű�������ô˽ű�������controller
        controller = obj.AddComponent<MonoController>();
    }
    //����controller�����Ӽ����ķ�����ע�⴫��Ĳ���Ϊ�޲��޷���ֵ�ķ���
    public void AddUpdateListener(UnityAction func)
    {
        controller.AddUpdateListener(func);
    }
    //ͬ�ϣ������˷�����Ե�����ʱ�����
    public void AddFixUpdateListener(UnityAction func)
    {
        controller.AddFixedUpdateListener(func);
    }
    //�Ƴ�������Update��ķ���
    public void RemoveUpdeteListener(UnityAction func)
    {
        controller.RemoveUpdateListener(func);
    }
    //ͬ���Ƴ�������FixUpdater�ķ���
    public void RemoveFixUpdateListener(UnityAction func)
    {
        controller.RemoveFiUpdateListener(func);
    }

    //�˴�Ҫ��Ϊ���ֵ��¼�ķ���
    //����Э�̷����������ط�����ʹ��ʱע��ѡ��
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }

    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }

}
