using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    //ʹ���ֵ�洢UI���������ϵĿؼ�
    public Dictionary<string, List<UIBehaviour>> controlDic
       = new Dictionary<string, List<UIBehaviour>>();
    private void Awake()//��Start֮ǰ�͵õ�����
    {
        //��Ŀؼ����������ֵ��У��ɸ����Լ�UI���صĿؼ�����
        FinChildControl<Button>();
        FinChildControl<Image>();
        FinChildControl<Scrollbar>();
        FinChildControl<Text>();
    }
    //������ΪcontrolName��UI���ϵ�T�ؼ�����û���򷵻�null
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            for (int i = 0; i < controlDic[controlName].Count; i++)
            {
                if (controlDic[controlName][i] is T)//ʹ��is�ж��Ƿ�ΪT�ؼ�
                    return controlDic[controlName][i] as T;
            }
        }

        return null;
    }

    //�ҵ����弰���������UI�ؼ��������ֵ���
    private void FinChildControl<T>() where T : UIBehaviour
    {
        T[] controls = this.GetComponentsInChildren<T>();//ע���õ���GetComents,���ص���һ��
        string objName;//ʹ�����ֶԿؼ����������
        for (int i = 0; i < controls.Length; i++)
        {
            objName = controls[i].gameObject.name;
            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);//ע��controlDic�д洢������Ϊ<string, List<UIBehaviour>,�������ǵ�ͬһ��UI�����пؼ��������������
            else
                controlDic.Add(objName, new List<UIBehaviour>() { controls[i] });//���UI���ֵ���
        }
    }

    //�麯�������ڼ̳������Լ�����
    public virtual void ShowMe()
    {

    }
    //ͬ��
    public virtual void HideMe()
    {

    }

}
