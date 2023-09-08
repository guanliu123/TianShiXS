//单例模式继承自BaseManager，使用时可以通过GetInstance调用
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
    //同步加载
    //使用是要注意给出T的类型，如ResMagr.GetInstance().Load<GameObject>()
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
            Debug.Log("加载资源时传入的资源类型不正确！");
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
            Debug.Log($"加载{objName}资源失败！ path:{path}");
        }
    }

    //同步加载
    //使用是要注意给出T的类型，如ResMagr.GetInstance().Load<GameObject>()
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
            Debug.Log("加载资源时传入的资源类型不正确！");
            faild();
            yield break;
        }

        var handle = Addressables.LoadAssetAsync<T>(path);
        while (handle.Status == AsyncOperationStatus.None)
        {
            yield return new WaitForSeconds(0.5f);
        }
        // 加载完成回调
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
        else Debug.Log("加载资源失败！");
        //return null;
    }

    public IEnumerator LoadRes<T>(string path, Object temp) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        if (!handle.IsDone) yield return handle;
        // 加载完成回调
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

    //异步加载，异步加载使用起来在观感上更加顺滑，适用于较大的资源
    //异步加载使用协程
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        //由于是单例模式，要使用协程需要用到MonoMgr
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync<T>(name, callback));
    }


    private IEnumerator ReallyLoadAsync<T>(string _name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(_name);
        yield return r;//直到系统读取完，r没读取到就是null状态
        if (r.asset is GameObject)
        {
            T t = GameObject.Instantiate(r.asset) as T;
            t.name = _name;//去点(clone)字段
            callback(t);//callback方法作用很大，例如获得其组件脚本，改变其状态        
        }

        else
            r.asset.name = _name;
        callback(r.asset as T);
    }
}