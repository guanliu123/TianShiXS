using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmap : MonoBehaviour
{
    public GameObject t;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MapManager.GetInstance().BeginMapCreate();
        }
        if (Input.GetKeyDown(KeyCode.K)) GameObject.Instantiate(t, Vector3.zero, Quaternion.identity);
    }
}
