using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void test(Vector3 vector = default)
    {
        Debug.Log(vector == default);
    }
}
