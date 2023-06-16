using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using UnityEditor.Experimental.GraphView;

//���Ը�Ϊ��ID�洢�������
public class PoolData
{

    //������п����ж����ͬ��������壬Ϊ����������Ҫ����һ��������
    public GameObject fatherObj;
    //ʹ��List��ʽ�ṹ���洢����
    public List<GameObject> poolList;
    private GameObject poolObj;

    //���캯��������PoolData��һЩ��ʼ��
    public PoolData(GameObject obj, GameObject _poolObj,bool resetFather=false)
    {
        fatherObj = obj;
        poolObj = _poolObj;
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>();
        PushObj(obj);

    }
    //������push�������
    public void PushObj(GameObject obj)
    {
        //�洢�����ø����塢����
        poolList.Add(obj);
        obj.SetActive(false);
        if (fatherObj == null)
        {
            fatherObj = obj;
            fatherObj.transform.parent = poolObj.transform;
        }
        obj.transform.SetParent(fatherObj.transform);       
    }
    //������ӻ����ȡ��
    public GameObject GetObj()
    {
        if (poolList.Count <= 0) return null;

        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        if (!obj) return obj;
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}

//�Ի���ؽ��й���ĵ���ģʽ��
public class PoolManager : BaseManager<PoolManager>
{
    public bool isActive = true;
    //ʹ���ֵ�洢����
    private Dictionary<string, PoolData> poolDic
        = new Dictionary<string, PoolData>();
    //����صĸ�����
    private GameObject poolObj;

    public override void Reset()
    {
        Clear();
    }

    /// <summary>
    /// �Ӷ������ȡ����
    /// </summary>
    /// <param name="poolName">�������ڶ�������ƣ�������ȡ�����������ƣ�</param>
    /// <returns></returns>
    public GameObject GetObj(string poolName)
    {
        if (poolName == "RotateBullet") Debug.Log("ȡ����ת�ӵ�");
        if (!isActive) return null;
        if (poolDic.ContainsKey(poolName) && poolDic[poolName].poolList.Count > 0)
        {
            GameObject t = poolDic[poolName].GetObj();
            if (t == null) poolDic.Remove(poolName);
            else return t;
        }

        return GameObject.Instantiate(ResourceManager.GetInstance().LoadByName<GameObject>(poolName));
    }

    /*public GameObject GetBullet(string bulletName, GameObject _attacker, CharacterTag _tag, BulletData _bulletData, Dictionary<BuffType, int> buffs)
    {
        if (!isActive) return null;
        if (poolDic.ContainsKey(bulletName) && poolDic[bulletName].poolList.Count > 0)
        {
            GameObject t = poolDic[bulletName].GetObj();
            if (t == null) poolDic.Remove(bulletName);
            else
            {
                BulletBase bb = t.GetComponent<BulletBase>();
                bb.InitBullet(_attacker, _tag, _bulletData, buffs);
                return t;
            }
        }

        return ExtensionMethod.InstantiateBullet(ResourceManager.GetInstance().LoadByName<GameObject>(bulletName),
            _attacker, _tag, _bulletData, buffs);
    }*/

    //�ӻ������ȡ��
    public void GetObj(string objName, UnityAction<GameObject> callback)
    {
        //��������д��ڣ�ȡ��
        if (poolDic.ContainsKey(objName) && poolDic[objName].poolList.Count > 0)
            callback(poolDic[objName].GetObj());
        //����������ʹ��ResMgr��̬����
        else
        {
            //ע�����ɵ������this.name��ʱ������objName������objName��clone��������ʹ��this.name����PushObj�Ļ������޷�ʹ��objName���ֵ��н���Get
            ResourceManager.GetInstance().LoadAsync<GameObject>(objName, callback);

        }
    }

    //ʹ��ʱע��objName��ʹ�ö�̬���س����������this.name����ԭ������ϣ�clone)��������ʱʹ��this.name����PushObj����ʱ��ʵ���Ǵ�������һ�����ӣ�����ʹ��ʱ�Ƽ�ֱ��ʹ��"objName"�ķ�ʽ������this.name�ķ�ʽ
    /// <summary>
    /// �������������
    /// </summary>
    /// <param name="poolName">��Ҫ����ĳ�������</param>
    /// <param name="obj">��Ʒ����</param>
    public void PushObj(string poolName, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");//ʵ�������˺������ڻ���ص�����ȫ��Ϊ
        if (poolDic.ContainsKey(poolName))//�����������Ѿ����������ͣ��������������
            poolDic[poolName].PushObj(obj);
        else//���������û�д������壬��������ֵ�
            poolDic.Add(poolName, new PoolData(obj, poolObj));
        //���õĽṹ��PoolData �࣬���溬����ʽ�ṹPoolList 
    }
    //���ĳ������
    public void Clear(string poolName)
    {
        if (poolDic.ContainsKey(poolName))
        {
            for(int i = 0; i < poolDic[poolName].poolList.Count; i++)
            {
                GameObject.Destroy(poolDic[poolName].poolList[i]);
            }
            GameObject.Destroy(poolDic[poolName].fatherObj);
            poolDic.Remove(poolName);
        }
    }
    //��ջ����
    public void Clear()
    {
        //isActive = false;
        for(int i = 0; i < poolDic.Count; i++)
        {
            for(int j = 0; j < poolDic.ElementAt(i).Value.poolList.Count; j++)
            {
                GameObject.Destroy(poolDic.ElementAt(i).Value.poolList[j]);
            }
            GameObject.Destroy(poolDic.ElementAt(i).Value.fatherObj);
        }

        poolDic.Clear();
        GameObject.Destroy(poolObj);
        poolObj = null;
    }
}

/*public static class ExtensionMethod
{
    public static GameObject InstantiateBullet(UnityEngine.Object original, 
        GameObject _attacker, CharacterTag _tag, BulletData _bulletData, Dictionary<BuffType, int> buffs)
    {
        GameObject bullet = Object.Instantiate(original) as GameObject;
        BulletBase bb = bullet.GetComponent<BulletBase>();
        bb.InitBullet(_attacker, _tag, _bulletData, buffs);
        return bullet;
    }
}*/