using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isDragging = false; // 是否正在拖动角色
    private bool isHorizontalMode = true; // 是否处于水平模式
    private Vector2 touchStartPosition; // 触摸起始位置
    private Vector3 objectStartPosition; // 物体起始位置

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 检测手指是否按在角色物体上
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    touchStartPosition = touch.position;
                    objectStartPosition = transform.position;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // 拖动角色
                Vector2 delta = touch.position - touchStartPosition;

                if (isHorizontalMode)
                {
                    // 水平模式下只能在x轴上移动
                    transform.position = new Vector3(objectStartPosition.x + delta.x / Screen.width, objectStartPosition.y, objectStartPosition.z);
                }
                else
                {
                    // 垂直模式下只能在xoz平面上移动
                    transform.position = new Vector3(objectStartPosition.x + delta.x / Screen.width, objectStartPosition.y, objectStartPosition.z + delta.y / Screen.height);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
    }

    // 切换操作模式
    public void SwitchMode()
    {
        isHorizontalMode = !isHorizontalMode;
    }
    /*public Transform currTouchObj;
    private Camera mainCamera;
    private float moveSpeed;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

private void Update()
{
    CtrlTouchObjMove();    
}

private void CtrlTouchObjMove()
{
    if (Input.touchCount == 1)
    {
        Touch firstTouch = Input.GetTouch(0);
        if (firstTouch.phase == TouchPhase.Began)
        {
            Ray ray = mainCamera.ScreenPointToRay(firstTouch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //获取当前触摸到的物体
                if (hit.collider.gameObject != this.gameObject) return;
                currTouchObj = hit.collider.transform;
            }
        }

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            if (currTouchObj)
            {
                Vector3 touchDeltaPos = Input.GetTouch(0).deltaPosition;
                currTouchObj.Translate(touchDeltaPos.x, touchDeltaPos.y, 0, Space.World);
            }
        }

        currTouchObj = null;
    }
}*/

}
