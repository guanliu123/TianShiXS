using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MonoController : MonoBehaviour
{
    //设置事件，用此事件监听需要循环更新执行的函数
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;

    public Dictionary<int, Coroutine> CoroutinesDic = new Dictionary<int, Coroutine>();
    public List<Coroutine> CoroutinesList = new List<Coroutine>();

    //此物体不可移除
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    //如果此事件不为空，则在此类的Update里调用
    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }
    //随时间更行的调用
    private void FixedUpdate()
    {
        if (fixedUpdateEvent != null)
            fixedUpdateEvent();
    }

    //对方法添加监听
    public void AddUpdateListener(UnityAction func)
    {
        updateEvent += func;
    }

    //移除
    public void RemoveUpdateListener(UnityAction func)
    {
        updateEvent -= func;
    }

    public void AddFixedUpdateListener(UnityAction func)
    {
        fixedUpdateEvent += func;
    }

    public void RemoveFiUpdateListener(UnityAction func)
    {
        fixedUpdateEvent -= func;
    }

    /// <summary>
    ///     清除不使用的协程
    /// </summary>
    public void ClearUnUsedCoroutines()
    {
        for (var i = CoroutinesList.Count - 1; i > 0; i--)
        {
            if (CoroutinesList[i] == null)
            {
                CoroutinesList.RemoveAt(i);
            }
        }
    }


    /// <summary>
    ///     开启协程
    /// </summary>
    /// <param name="id">协程id</param>
    /// <param name="enumerator">具体协程方法</param>
    /// <param name="restart">如果协程存在是否重新开启</param>
    /// <returns></returns>
    public Coroutine Create(int id, IEnumerator enumerator, bool restart = false)
    {
        if (CoroutinesDic.ContainsKey(id) && !restart) return null;
        if (CoroutinesDic.ContainsKey(id) && restart) Kill(id);

        var coroutine = StartCoroutine(enumerator);
        CoroutinesDic.Add(id, coroutine);
        return coroutine;
    }

    /// <summary>
    ///     给不好占用id的协程使用，不推荐，必须确保协程不会重复开启并且能够自己停止
    /// </summary>
    /// <param name="enumerator">需要开启的协程</param>
    /// 如果该协程已经存在，是否停止
    /// <returns></returns>
    public Coroutine Create(IEnumerator enumerator)
    {
        var coroutine = StartCoroutine(enumerator);
        CoroutinesList.Add(coroutine);
        return coroutine;
    }

    /// <summary>
    ///     根据id关闭协程
    /// </summary>
    /// <param name="id">需要关闭的协程id</param>
    public void Kill(int id)
    {
        if (CoroutinesDic.ContainsKey(id))
        {
            StopCoroutine(CoroutinesDic[id]);
            CoroutinesDic.Remove(id);
        }
    }

    /// <summary>
    ///     通过调用此类中方法开启的协程必须在结束时写上这个或者kill方法并且传入id
    /// </summary>
    /// <param name="id">此协程的id</param>
    public void CoroutineStopped(int id)
    {
        if (CoroutinesDic.ContainsKey(id)) CoroutinesDic.Remove(id);
    }
}
