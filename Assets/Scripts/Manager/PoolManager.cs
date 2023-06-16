using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using UnityEditor.Experimental.GraphView;

//可以改为用ID存储到缓存池
public class PoolData
{

    //缓存池中可能有多个不同种类的物体，为方便管理故需要设置一个父物体
    public GameObject fatherObj;
    //使用List链式结构来存储物体
    public List<GameObject> poolList;
    private GameObject poolObj;

    //构造函数，进行PoolData的一些初始化
    public PoolData(GameObject obj, GameObject _poolObj,bool resetFather=false)
    {
        fatherObj = obj;
        poolObj = _poolObj;
        fatherObj.transform.parent = poolObj.transform;
        poolList = new List<GameObject>();
        PushObj(obj);

    }
    //将物体push进缓存池
    public void PushObj(GameObject obj)
    {
        //存储、设置父物体、隐藏
        poolList.Add(obj);
        obj.SetActive(false);
        if (fatherObj == null)
        {
            fatherObj = obj;
            fatherObj.transform.parent = poolObj.transform;
        }
        obj.transform.SetParent(fatherObj.transform);       
    }
    //将物体从缓存池取出
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

//对缓存池进行管理的单例模式类
public class PoolManager : BaseManager<PoolManager>
{
    public bool isActive = true;
    //使用字典存储数据
    private Dictionary<string, PoolData> poolDic
        = new Dictionary<string, PoolData>();
    //缓存池的父物体
    private GameObject poolObj;

    public override void Reset()
    {
        Clear();
    }

    /// <summary>
    /// 从对象池中取物体
    /// </summary>
    /// <param name="poolName">物体所在对象池名称（就是想取出的物体名称）</param>
    /// <returns></returns>
    public GameObject GetObj(string poolName)
    {
        if (poolName == "RotateBullet") Debug.Log("取出旋转子弹");
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

    //从缓存池中取出
    public void GetObj(string objName, UnityAction<GameObject> callback)
    {
        //若缓存池中存在，取出
        if (poolDic.ContainsKey(objName) && poolDic[objName].poolList.Count > 0)
            callback(poolDic[objName].GetObj());
        //若不存在则使用ResMgr动态加载
        else
        {
            //注意生成的物体的this.name此时并不是objName，而是objName（clone），若是使用this.name进行PushObj的话，则无法使用objName从字典中进行Get
            ResourceManager.GetInstance().LoadAsync<GameObject>(objName, callback);

        }
    }

    //使用时注意objName，使用动态加载出来的物体的this.name会在原名后加上（clone)字样，此时使用this.name进行PushObj操作时，实际是创建了另一个池子，所以使用时推荐直接使用"objName"的方式而不是this.name的方式
    /// <summary>
    /// 将物体加入对象池
    /// </summary>
    /// <param name="poolName">想要加入的池子类型</param>
    /// <param name="obj">物品本身</param>
    public void PushObj(string poolName, GameObject obj)
    {
        if (poolObj == null)
            poolObj = new GameObject("Pool");//实例化，此后所以在缓存池的物体全部为
        if (poolDic.ContainsKey(poolName))//如果缓存池中已经存在其类型，则将物体加入其中
            poolDic[poolName].PushObj(obj);
        else//若缓存池中没有此类物体，则添加至字典
            poolDic.Add(poolName, new PoolData(obj, poolObj));
        //采用的结构是PoolData 类，里面含有链式结构PoolList 
    }
    //清空某个池子
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
    //清空缓存池
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