//����ģʽ�̳���BaseManager��ʹ��ʱ����ͨ��GetInstance����
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
    //ͬ������
    //ʹ����Ҫע�����T�����ͣ���ResMagr.GetInstance().Load<GameObject>()
    public T LoadByName<T>(string objName) where T : Object
    {
        
        T res = Resources.Load<T>(DataManager.GetInstance().AskAPath(objName));
        return res;
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
