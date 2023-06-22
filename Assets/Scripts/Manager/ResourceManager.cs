//单例模式继承自BaseManager，使用时可以通过GetInstance调用
using System.Xml.Linq;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceManager : BaseManager<ResourceManager>
{
    private ResourcepathSO resourcepathSO;
    private Dictionary<ResourceType, Dictionary<string, string>> objPathDic = new Dictionary<ResourceType, Dictionary<string, string>>();
    public ResourceManager()
    {
        /*resourcepathSO = Resources.Load<ResourcepathSO>("ScriptableObject/ResourcepathSO");

        List<ResourceDatas> t = resourcepathSO.resourcePaths;
        foreach (var item in t)
        {
            objPathDic.Add(item.resourceType, new Dictionary<string, string>());
            foreach (var item1 in item.resourceNPs)
            {
                objPathDic[item.resourceType].Add(item1.name, item1.path);
            }
        }*/
    }
    //同步加载
    //使用是要注意给出T的类型，如ResMagr.GetInstance().Load<GameObject>()
    public T LoadByName<T>(string objName) where T : Object
    {
        
        T res = Resources.Load<T>(DataManager.GetInstance().AskAPath(objName));
        return res;
    }
    public T LoadByPath<T>(string objPath) where T : Object
    {
        T res = Resources.Load<T>(objPath);
        //如果res是一个GameObject
        /*if (res is GameObject)
        {
            T t = GameObject.Instantiate(res);
            t.name = objName;
            return t;

        }*/
        //else
            return res;
    }

    public List<T> Loads<T>(ResourceType resourceType) where T : Object
    {
        List<string> targetPaths = DataManager.GetInstance().AskPaths(resourceType);
        List<T> res = new List<T>();

        if (targetPaths == null) return null;
        else
        {
            foreach(var item in targetPaths)
            {
                res.Add(Resources.Load<T>(item));
            }
            return res;
        }
    }

    public T Load<T>(string instantName,string objName) where T : Object
    {
        T res = Resources.Load<T>(objName);
        //如果res是一个GameObject
        /*if (res is GameObject)
        {
            T t = GameObject.Instantiate(res);
            t.name = instantName;
            return t;
        }*/
        //else //else情况示例：TextAsset、AudioClip
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
