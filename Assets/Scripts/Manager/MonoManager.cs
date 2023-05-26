using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;

public class MonoManager : BaseManager<MonoManager>
{
    //为了能够使用并管理MonoController,我们首先需要一个MonoController对象
    private MonoController controller;

    //由于是单例模式，我们创建构造函数来进行一些必要的初始化
    public MonoManager()
    {
        //MonoController并不是单例模式，新创建一个游戏物体名为MonoController，添加monocontroller
        GameObject obj = new GameObject("MonoController");
        //挂载脚本，并获得此脚本的引用controller
        controller = obj.AddComponent<MonoController>();
    }
    //调用controller里的添加监听的方法，注意传入的参数为无参无返回值的方法
    public void AddUpdateListener(UnityAction func)
    {
        controller.AddUpdateListener(func);
    }
    //同上，不过此方法针对的是随时间更新
    public void AddFixUpdateListener(UnityAction func)
    {
        controller.AddFixedUpdateListener(func);
    }
    //移除监听在Update里的方法
    public void RemoveUpdeteListener(UnityAction func)
    {
        controller.RemoveUpdateListener(func);
    }
    //同上移除监听在FixUpdater的方法
    public void RemoveFixUpdateListener(UnityAction func)
    {
        controller.RemoveFiUpdateListener(func);
    }

    public void ClearActions()
    {
        controller.ClearActions();
    }
    //开启协程方法及其重载方法，使用时注意选择
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
    ///     通过调用此类中方法开启的协程必须在结束时写上这个或者kill方法并且传入id
    /// </summary>
    /// <param name="id">此协程的id</param>
    public void CoroutineStopped(CoroutineType coroutineType)
    {
        controller.CoroutineStopped((int)coroutineType);
    }
    public void KillAllCoroutines()
    {
        controller.KillAllCoroutines();
    }
}
