//ʹ��where�Է���T����Լ��
using System.Diagnostics;

public class BaseManager<T> where T : new()
{
    private static T instance;
    //��̬����,��T��ʵ��instance

    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
    //����������ʵ��instance
}
