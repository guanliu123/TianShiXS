/*using UnityEngine;

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
        isDragging = false;
    }
}*/

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isDragging = false; // 是否正在拖动角色
    private bool isHorizontalMode = true; // 是否处于水平模式
    public bool canMove = true;

    private Vector3 touchOrigin; // 手指按下的屏幕位置
    private Vector3 touchPosition;//手指最新按下的位置
    private float moveSpeed = 10f;

    private void Update()
    {
        if (!canMove) return;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 检测手指是否按在角色物体上
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                //手指按在角色上或者按在屏幕1/3以下的位置
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject||touch.position.y<Screen.height/3)
                {
                    isDragging = true;
                    touchOrigin = touch.position;
                    touchPosition = touch.position;
                    //offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, 10.0f));
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // 根据操作模式拖动角色
                //Vector3 newPosition = Camera.main.ScreenToWorldPoint(touchOrigin) + offset;
                touchPosition = touch.position;
                Vector3 dragDir = (touchPosition - touchOrigin).normalized;

                if (isHorizontalMode)
                {
                    dragDir.y = 0;
                    dragDir.z = 0;
                }
                else
                {
                    dragDir.z = dragDir.y;
                    dragDir.y = 0;
                }

                
                transform.Translate(dragDir *moveSpeed*Time.deltaTime);
                Debug.Log(dragDir);
                touchOrigin = touchPosition;
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
        isDragging = false;
    }
}