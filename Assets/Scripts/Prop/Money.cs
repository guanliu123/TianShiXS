﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : PropBase
{
    private void Start()
    {
        propType = PropType.Money;
    }
    // Update is called once per frame
    void Update()
    {
        if (Player._instance != null) gameObject.transform.DOMove(Player._instance.gameObject.transform.position, 1f);
        else Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.GetInstance().ChangeMoney(1);
            Recovery();
        }
    }
}
