//使用where对泛型T进行约束
using System.Diagnostics;

public class BaseManager<T> where T : new()
{
    private static T instance;
    //静态函数,建T的实体instance

    public static T GetInstance()
    {
        if (instance == null)
            instance = new T();
        return instance;
    }
    //返回这个类的实体instance
}
