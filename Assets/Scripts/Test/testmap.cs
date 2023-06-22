using LitJson;
using StarkSDKSpace.SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class testmap : MonoBehaviour
{
    Vector3 vector;
    public void Awake()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraManager.GetInstance().CameraShake(0.2f, 0.5f);
        }
    }
}
