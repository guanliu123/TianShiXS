using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
//UI管理器
namespace UIFrameWork
{
    //应该是一个单例，理解为UI的内存池，存储面板信息与对象的映射关系
    public class UIManager : SingletonBase<UIManager>
    {
        private Dictionary<UIType, GameObject> dicUI = new Dictionary<UIType, GameObject>();
        //获取一个面板
        public GameObject GetSingleUI(UIType uIType)
        {
            if (uIType == null)
                return null;
            GameObject parent = GameObject.Find("Canvas/SafeAreaRect");
            if (!parent)
            {
                Debug.LogError("无Canvas对象，请查询是否存在所有UI的根");
                return null;
            }
            //如果内存池中存在该UI面板
            if (dicUI.ContainsKey(uIType))
                return dicUI[uIType];
            //如果不存在，从预设中加载
            GameObject uiPrefab = Resources.Load<GameObject>(uIType.Path);

            GameObject uiInstance = null;
            if (uiPrefab != null)
            {
                uiInstance = GameObject.Instantiate(uiPrefab, parent.transform);
                uiInstance.name = uIType.Name;
                dicUI.Add(uIType, uiInstance);
            }
            else
            {
                Debug.LogError($"在路径:{uIType.Path}中没有找到名为{uIType.Name}的预设，请查询");
            }
            return uiInstance;
        }

        public void GetSingleUI(UIType uIType, Action<GameObject> cb)
        {
            Debug.Log($"GetSingleUI {uIType.Path} begin");

            if (uIType == null)
                return;
            GameObject parent = GameObject.Find("Canvas/SafeAreaRect");
            if (!parent)
            {
                Debug.LogError("无Canvas对象，请查询是否存在所有UI的根");
                return;
            }
            //如果内存池中存在该UI面板
            if (dicUI.ContainsKey(uIType))
            {
                Debug.Log($"GetSingleUI {uIType.Path} dicUI find!");
                cb.Invoke(dicUI[uIType]);
                return;
            }
            foreach (Transform item in parent.transform)
            {
                if (item.name == uIType.Name)
                {
                    Debug.Log($"GetSingleUI {uIType.Path} parent.transform find!");
                    cb.Invoke(item.gameObject);
                    return;
                }
            }

            var uiPrefab = Addressables.LoadAssetAsync<GameObject>("Assets/Resources_Move/" + uIType.Path + ".prefab");
            uiPrefab.Completed += (handle) =>
            {
                Debug.Log($"GetSingleUI {uIType.Path} LoadAssetAsync Completed!");

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log($"GetSingleUI {uIType.Path} LoadAssetAsync Succeeded!");

                    GameObject result = handle.Result;
                    GameObject uiInstance = GameObject.Instantiate(result, parent.transform);
                    cb.Invoke(uiInstance);
                    uiInstance.name = uIType.Name;
                    dicUI.Add(uIType, uiInstance);
                }
                else
                {
                    Debug.LogError($"在路径:{uIType.Path}中没有找到名为{uIType.Name}的预设，请查询");
                }
            };
        }

        //销毁一个面板
        public void DestroyUI(UIType uIType)
        {
            if (uIType == null)
                return;
            if (dicUI.ContainsKey(uIType))
            {
                GameObject.Destroy(dicUI[uIType]);
                dicUI.Remove(uIType);
            }
        }

    }
}