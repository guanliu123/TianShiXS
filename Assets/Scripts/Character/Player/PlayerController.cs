/*using System.Collections;
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
                    //transform.position = new Vector3(objectStartPosition.x + delta.x / Screen.width, objectStartPosition.y, objectStartPosition.z);
			transform.position = new Vector3(objectStartPosition.x + delta.x, objectStartPosition.y, objectStartPosition.z);
                }
                else
                {
                    // 垂直模式下只能在xoz平面上移动
                    //transform.position = new Vector3(objectStartPosition.x + delta.x / Screen.width, objectStartPosition.y, objectStartPosition.z + delta.y / Screen.height);
			transform.position = new Vector3(objectStartPosition.x + delta.x, objectStartPosition.y, objectStartPosition.z + delta.y);
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
   
}
*/

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isDragging = false; // 是否正在拖动角色
    private bool isHorizontalMode = true; // 是否处于水平模式

    private Vector3 touchPosition; // 手指按下的屏幕位置
    private Vector3 offset; // 角色物体与手指按下位置的偏移量

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 检测手指是否按在角色物体上
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
                {
                    isDragging = true;
                    touchPosition = touch.position;
                    offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10.0f));
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // 根据操作模式拖动角色
                Vector3 newPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10.0f)) + offset;

                if (isHorizontalMode)
                {
                    newPosition.y = transform.position.y;
                    newPosition.z = transform.position.z;
                }
                else
                {
                    newPosition.y = transform.position.y;
                }

                transform.position = newPosition;
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
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
}