using UnityEngine.Events;
using UnityEngine;

public class MonoController : MonoBehaviour
{

    //�����¼����ô��¼�������Ҫѭ������ִ�еĺ���
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;

    //�����岻���Ƴ�
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    //������¼���Ϊ�գ����ڴ����Update�����
    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }
    //��ʱ����еĵ���
    private void FixedUpdate()
    {
        if (fixedUpdateEvent != null)
            fixedUpdateEvent();
    }

    //�Է�����Ӽ���
    public void AddUpdateListener(UnityAction func)
    {
        updateEvent += func;
    }

    //�Ƴ�
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
