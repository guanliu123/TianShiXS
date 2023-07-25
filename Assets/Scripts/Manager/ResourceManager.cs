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
        //resourcepathSO = Resources.Load<ResourcepathSO>("ScriptableObject/ResourcepathSO");

        /*List<ResourceDatas> t = resourcepathSO.resourcePaths;
        foreach (var item in t)
        {
            objPathDic.Add(item.resourceType, new Dictionary<string, string>());
            foreach (var item1 in item.resourceNPs)
            {
                objPathDic[item.resourceType].Add(item1.name, item1.path);
            }
        }*/
        pathDic = ResourceDataTool.ReadResourceData();
    }
    //ͬ������
    //ʹ����Ҫע�����T�����ͣ���ResMagr.GetInstance().Load<GameObject>()
    //UnityAction<T> callback, 
    public async void LoadRes<T>(string objName, UnityAction<T> callback, ResourceType resourceType = ResourceType.Null, string suffix = "") where T : Object
    {
        if (pathDic.ContainsKey(resourceType))
        {
            //res = Resources.Load<T>(pathDic[resourceType]+objName);
            string path = "Assets/Resources_Move/";
            if (resourceType != ResourceType.Null) path += pathDic[resourceType].Item1 + objName + pathDic[resourceType].Item2;
            else path += objName + suffix;

            var handle = Addressables.LoadAssetAsync<T>(path);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded) callback(handle.Result);
            else Debug.Log($"����{objName}��Դʧ�ܣ�");

            //LoadRes<T>(path,callback);
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
        //���res��һ��GameObject
        /*if (res is GameObject)
        {
            T t = GameObject.Instantiate(res);
            t.name = objName;
            return t;

        }*/
        //else
        return res;
    }

    public T Load<T>(string instantName, string objName) where T : Object
    {
        T res = Resources.Load<T>(objName);
        //���res��һ��GameObject
        /*if (res is GameObject)
        {
            T t = GameObject.Instantiate(res);
            t.name = instantName;
            return t;
        }*/
        //else //else���ʾ����TextAsset��AudioClip
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