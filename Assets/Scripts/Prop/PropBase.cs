using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBase : MonoBehaviour
{
    public PropType propType;

    public void Recovery()
    {
        PoolManager.GetInstance().PushObj(propType.ToString(),gameObject);
    }
}
