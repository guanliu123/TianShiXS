using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
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

    public void ClearActions()
    {
        controller.ClearActions();
    }
    //����Э�̷����������ط�����ʹ��ʱע��ѡ��
    public Coroutine StartCoroutine(CoroutineType coroutineType, IEnumerator enumerator, bool restart = false)
    {
        int id = (int)coroutineType;
        //return controller.StartCoroutine(routine);
        return controller.Create(id, enumerator, restart);
    }

    public Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return controller.Create(enumerator);
    }

    public void KillCoroutine(CoroutineType coroutineType)
    {
        controller.Kill((int)coroutineType);
    }

    /*public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }*/
    /// <summary>
    ///     ͨ�����ô����з���������Э�̱����ڽ���ʱд���������kill�������Ҵ���id
    /// </summary>
    /// <param name="id">��Э�̵�id</param>
    public void CoroutineStopped(CoroutineType coroutineType)
    {
        controller.CoroutineStopped((int)coroutineType);
    }
    public void KillAllCoroutines()
    {
        controller.KillAllCoroutines();
    }
}
