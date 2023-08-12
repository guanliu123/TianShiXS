using System;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isHorizontalMode = true; // 是否处于水平模式
    public bool canMove = true;
    private bool isDragging = false;

    private float nowSpeed = 3.6f;
    private float normalSpeed = 3.6f;
    private float fightSpeed = 5f;
    float minNum = -500f;
    float maxNum = 500f;

    private Vector3 offset;

    private void OnEnable()
    {
        Input.ResetInputAxes();
        isHorizontalMode = true;
        canMove = true;
        isDragging = false;
        offset = Vector3.up+Vector3.back;
    }

    private void Update()
    {
        if (!canMove)
        {
            offset = transform.position;
            return;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 检测手指是否按在角色物体上
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                //手指按在角色上或者按在屏幕1/3以下的位置
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject || touch.position.y < Screen.height / 3)
                {
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
            else if (touch.phase==TouchPhase.Moved && isDragging)
            {
                if (isHorizontalMode)
                {
                    float scaledX = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minNum, maxNum, touch.deltaPosition.x));
                    offset = new Vector3(transform.position.x - scaledX * nowSpeed, 1, -1);
                    offset.x = Mathf.Clamp(offset.x, -LevelManager.GetInstance().mapSize[1] - 0.2f, LevelManager.GetInstance().mapSize[1] / 2 - 0.2f);

                   // transform.position = offset;
                }
                else
                {
                    float scaledX = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minNum, maxNum, touch.deltaPosition.x));
                    //float scaledY = Mathf.Lerp(-1f, 1f, Mathf.InverseLerp(minNum, maxNum, touch.deltaPosition.y));
                    offset = new Vector3(transform.position.x - scaledX * nowSpeed, 1, transform.position.z /*- scaledY * nowSpeed*/);
                    offset.x = Mathf.Clamp(offset.x, -LevelManager.GetInstance().mapSize[1] / 2 - 0.2f, LevelManager.GetInstance().mapSize[1] / 2 - 0.2f);
                    offset.z = Mathf.Clamp(-offset.z, -LevelManager.GetInstance().mapSize[0] / 3 - 3f, LevelManager.GetInstance().mapSize[0] / 3 - 5f);

                    //transform.position = offset;
                }
                
            }
        }
    }
    private void FixedUpdate()
    {
        if (!canMove) return;
        transform.position = offset;
    }

    // 切换操作模式
    public void SwitchMode()
    {
        offset = transform.position;
        isHorizontalMode = !isHorizontalMode;
        if (isHorizontalMode) nowSpeed = normalSpeed;
        else nowSpeed = fightSpeed;
        isDragging = false;
    }
}