using UnityEngine.Events;
using UnityEngine;

public class MonoController : MonoBehaviour
{

    //设置事件，用此事件监听需要循环更新执行的函数
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;

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

}
