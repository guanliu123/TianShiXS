using System.Collections;
using System.Collections.Generic;
using UIFrameWork;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public static LoadScene Instance { get; private set; }

    public Slider slider;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    private void Start()
    {
        
    }

    public void SetPercent(float _percent)
    {
        slider.value = _percent;
        return;
    }
}
    

