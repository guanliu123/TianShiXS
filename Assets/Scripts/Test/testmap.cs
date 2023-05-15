using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmap : MonoBehaviour
{
    public int nowLevel;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var step in Enum.GetValues(typeof(BulletType)))
            {
                
            }
        }
    }
}
