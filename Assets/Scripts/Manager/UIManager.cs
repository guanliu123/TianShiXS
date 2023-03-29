//����ö�٣���ʾ�㼶
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum E_UI_Layer
{
    Bot,
    Mid,
    Top,
}
//�̳���BaseManager,����ģʽ��ʱ����
public class UIManager : BaseManager<UIManager>
{	//���ֵ�洢��UI,�������
    public Dictionary<string, BasePanel> panelDic
        = new Dictionary<string, BasePanel>();

    private Transform bot;
    private Transform mid;
    private Transform top;

    //���캯�������ڳ�ʼ��
    public UIManager()
    {	//����ResMgr���ػ���
        GameObject obj = ResourceManager.GetInstance().LoadByPath<GameObject>("UI/Canvas");
        Transform canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);//UI���泡���л�����

        //����bot��mid��top��
        bot = canvas.Find("bot");
        mid = canvas.Find("mid");
        top = canvas.Find("top");
        //UIϵͳû��EventSystem�޷�����
        obj = ResourceManager.GetInstance().LoadByPath<GameObject>("UI/EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }
    //����Panel�ƶ���Top�㣬����callbackί�пɴ��ɲ�������
    public void ShowPanel<T>(string panelName,
        E_UI_Layer layer = E_UI_Layer.Top,
        UnityAction<T> callback = null) where T : BasePanel
    {
        //���ֵ�����ע��������
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            if (callback != null)
                callback(panelDic[panelName] as T);
            return;
        }
        //���ֵ���δע�������壬��Resource�ļ����м���
        //ʹ����lambda���ʽ
        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>
        {
            //������ΪCanvas���Ӷ���
            //���������������λ��
            //�ҵ�������
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
            //���ø�����
            obj.transform.SetParent(father);

            //�������λ�úʹ�С
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //�õ�Ԥ�������ϵĽű����̳���BasePanel��
            T panel = obj.GetComponent<T>();

            //ִ��������Ҫ��������
            if (callback != null)
            {
                callback(panel);
            }

            //���ֵ�����Ӵ����
            panelDic.Add(panelName, panel);
        });


    }

    public void HidePanel(string panelName)
    {
        if (panelDic[panelName])
        {
            panelDic[panelName].HideMe();
            //���˸о�û��Ҫ
            panelDic[panelName].HideMe();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
}
