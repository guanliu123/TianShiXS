using LitJson;
using StarkSDKSpace.SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class testmap : MonoBehaviour
{
    Vector3 vector;
    event UnityAction t;
    private void Awake()
    {
        t += Print1;
        t += Print2;
        t += Print2;
    }
    private void Update()
    {
        t();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            t -= Print2;
        }
    }
    private void Print1()
    {
        print(1);
    }
    private void Print2()
    {
        print(2);
    }
}
