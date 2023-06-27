using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTimer : MonoBehaviour
{
    private float timer;
    private float recoveryTime;
    private string effectName;

    public void Init(string _effectName,float time)
    {
        effectName = _effectName;
        recoveryTime = time;
        timer = 0;
    }


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer < recoveryTime) return;
        Recovery();
    }

    private void Recovery()
    {
        PoolManager.GetInstance().PushObj(effectName, gameObject);
    }
}
