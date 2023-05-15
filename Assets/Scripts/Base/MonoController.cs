using UnityEngine.Events;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MonoController : MonoBehaviour
{
    //�����¼����ô��¼�������Ҫѭ������ִ�еĺ���
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;

    public Dictionary<int, Coroutine> CoroutinesDic = new Dictionary<int, Coroutine>();
    public List<Coroutine> CoroutinesList = new List<Coroutine>();

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

    /// <summary>
    ///     �����ʹ�õ�Э��
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
    ///     ����Э��
    /// </summary>
    /// <param name="id">Э��id</param>
    /// <param name="enumerator">����Э�̷���</param>
    /// <param name="restart">���Э�̴����Ƿ����¿���</param>
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
    ///     ������ռ��id��Э��ʹ�ã����Ƽ�������ȷ��Э�̲����ظ����������ܹ��Լ�ֹͣ
    /// </summary>
    /// <param name="enumerator">��Ҫ������Э��</param>
    /// �����Э���Ѿ����ڣ��Ƿ�ֹͣ
    /// <returns></returns>
    public Coroutine Create(IEnumerator enumerator)
    {
        var coroutine = StartCoroutine(enumerator);
        CoroutinesList.Add(coroutine);
        return coroutine;
    }

    /// <summary>
    ///     ����id�ر�Э��
    /// </summary>
    /// <param name="id">��Ҫ�رյ�Э��id</param>
    public void Kill(int id)
    {
        if (CoroutinesDic.ContainsKey(id))
        {
            StopCoroutine(CoroutinesDic[id]);
            CoroutinesDic.Remove(id);
        }
    }

    /// <summary>
    ///     ͨ�����ô����з���������Э�̱����ڽ���ʱд���������kill�������Ҵ���id
    /// </summary>
    /// <param name="id">��Э�̵�id</param>
    public void CoroutineStopped(int id)
    {
        if (CoroutinesDic.ContainsKey(id)) CoroutinesDic.Remove(id);
    }
}
