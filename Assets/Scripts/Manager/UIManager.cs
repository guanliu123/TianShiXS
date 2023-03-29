//设置枚举，表示层级
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
}
//继承自BaseManager,单例模式随时调用
public class UIManager : BaseManager<UIManager>
{	//用字典存储给UI,方便管理
    public Dictionary<string, BasePanel> panelDic
        = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;

    //构造函数，用于初始化
    public UIManager()
    {	//利用ResMgr加载画布
        GameObject obj = ResourceManager.GetInstance().LoadByPath<GameObject>("UI/Canvas");
        Transform canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);//UI不随场景切换销毁

        //加上bot，mid，top层
        bot = canvas.Find("bot");
        mid = canvas.Find("mid");
        top = canvas.Find("top");
        //UI系统没有EventSystem无法启动
        obj = ResourceManager.GetInstance().LoadByPath<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }
    //将此Panel移动到Top层，并且callback委托可传可不传函数
    public void ShowPanel<T>(string panelName,
        E_UI_Layer layer = E_UI_Layer.Top,
        UnityAction<T> callback = null) where T : BasePanel
    {
        //若字典中有注册过此面板
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if (callback != null)
                callback(panelDic[panelName] as T);
            return;
        }
        //若字典中未注册过此面板，从Resource文件夹中加载
        //使用了lambda表达式
        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //把它作为Canvas的子对象
            //并且设置它的相对位置
            //找到父对象
            Transform father = bot;
            switch (layer)
            {
                case E_UI_Layer.Mid:
                    father = mid;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);

            //设置相对位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的脚本（继承自BasePanel）
            T panel = obj.GetComponent<T>();

            //执行外面想要做的事情
            if (callback != null)
            {
                callback(panel);
            }

            //在字典中添加此面板
            panelDic.Add(panelName, panel);
        });


    }

    public void HidePanel(string panelName)
    {
        if (panelDic[panelName])
        {
            panelDic[panelName].HideMe();
            //个人感觉没必要
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
}
