//����ģʽ�̳���BaseManager��ʹ��ʱ����ͨ��GetInstance����
using System.Xml.Linq;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEngine.AddressableAssets;
using System;
using Object = UnityEngine.Object;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using UnityEditor;

public class ResourceManager : SingletonBase<ResourceManager>
{
    //private Dictionary<ResourceType, Dictionary<string, string>> objPathDic = new Dictionary<ResourceType, Dictionary<string, string>>();

    private Dictionary<ResourceType, (string, string)> pathDic = new Dictionary<ResourceType, (string, string)>();
    public ResourceManager()
    {
        pathDic = ResourceDataTool.ReadResourceData();
    }
    //ͬ������
    //ʹ����Ҫע�����T�����ͣ���ResMagr.GetInstance().Load<GameObject>()
    //UnityAction<T> callback, 
    public async Task LoadRes<T>(string objName, UnityAction<T> callback, UnityAction faild, ResourceType resourceType, string suffix = "") where T : Object
    {
        string path = "Assets/Resources_Move/";
        if (resourceType == ResourceType.Null)
        {
            path += objName + suffix;
        }
        else if (pathDic.ContainsKey(resourceType))
        {
            //res = Resources.Load<T>(pathDic[resourceType]+objName);
            path += pathDic[resourceType].Item1 + objName + pathDic[resourceType].Item2;           
            //LoadRes<T>(path,callback);
        }
        else
        {
            Debug.Log("������Դʱ�������Դ���Ͳ���ȷ��");
            return;
        }
        var handle = Addressables.LoadAssetAsync<T>(path);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            callback(handle.Result);
        }
        else
        {
            faild();
            Debug.Log($"����{objName}��Դʧ�ܣ� path:{path}");
        }
    }

    //ͬ������
    //ʹ����Ҫע�����T�����ͣ���ResMagr.GetInstance().Load<GameObject>()
    //UnityAction<T> callback, 
    public IEnumerator LoadResYield<T>(string objName, UnityAction<T> callback, UnityAction faild, ResourceType resourceType, string suffix = "") where T : Object
    {
        string path = "Assets/Resources_Move/";
        if (resourceType == ResourceType.Null)
        {
            path += objName + suffix;
        }
        else if (pathDic.ContainsKey(resourceType))
        {
            //res = Resources.Load<T>(pathDic[resourceType]+objName);
            path += pathDic[resourceType].Item1 + objName + pathDic[resourceType].Item2;
            //LoadRes<T>(path,callback);
        }
        else
        {
            Debug.Log("������Դʱ�������Դ���Ͳ���ȷ��");
            faild();
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<T>(path);
        while (handle.Status == AsyncOperationStatus.None)
        {
            yield return new WaitForSeconds(0.5f);
        }
        // ������ɻص�
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            callback(handle.Result);
        }
        else
        {
            faild();
        }
    }

    public async void LoadRes<T>(string path, UnityAction<T> callback) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded) callback(handle.Result);
        else Debug.Log("������Դʧ�ܣ�");
        //return null;
    }

    public IEnumerator LoadRes<T>(string path, Object temp) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        if (!handle.IsDone) yield return handle;
        // ������ɻص�
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            temp = handle.Result;
        }
    }

    public T LoadByPath<T>(string objPath) where T : Object
    {
        T res = Resources.Load<T>(objPath);
        return res;
    }

    //�첽���أ��첽����ʹ�������ڹ۸��ϸ���˳���������ڽϴ����Դ
    //�첽����ʹ��Э��
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //�����ǵ���ģʽ��Ҫʹ��Э����Ҫ�õ�MonoMgr
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback));
    }


    private IEnumerator ReallyLoadAsync<T>(string _name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(_name);
        yield return r;//ֱ��ϵͳ��ȡ�꣬rû��ȡ������null״̬
        if (r.asset is GameObject)
        {
            T t = GameObject.Instantiate(r.asset) as T;
            t.name = _name;//ȥ��(clone)�ֶ�
            callback(t);//callback�������úܴ�������������ű����ı���״̬        
        }

        else
            r.asset.name = _name;
        callback(r.asset as T);
    }
}