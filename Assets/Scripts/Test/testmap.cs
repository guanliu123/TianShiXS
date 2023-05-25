using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class testmap : MonoBehaviour
{
    Dictionary<int, float> t = new Dictionary<int, float>();

    public void Awake()
    {
        t.Add(1, 2);
        t.Add(2, 2);
    }

    // Update is called once per frame
    public void Update()
    {

    }
    public void test()
    {
        for(int i = 0; i < t.Count; i++)
        {
            float a = t.ElementAt(i).Value - Time.deltaTime;

            if (a <= 0)
            {
                t.Remove(t.ElementAt(i).Key);
                continue;
            }

            t[t.ElementAt(i).Key] = a;         
        }
    }
}
