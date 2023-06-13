using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isDragging = false; // 是否正在拖动角色
    private bool isHorizontalMode = true; // 是否处于水平模式
    public bool canMove = true;

    private Vector3 touchOrigin; // 手指按下的屏幕位置
    private Vector3 touchPosition;//手指最新按下的位置
    private float moveSpeed = 5f;

   /* private void Update()
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
                if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject || touch.position.y < Screen.height / 3)
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
                Vector3 dragVec = touchPosition - touchOrigin;

                if (isHorizontalMode)
                {
                    dragVec.y = 0;
                    dragVec.z = 0;
                }
                else
                {
                    dragVec.z = dragVec.y;
                    dragVec.y = 0;
                }


                //transform.Translate(dragVec * moveSpeed * Time.deltaTime);
                transform.position = dragVec;
                touchOrigin = touchPosition;
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                isDragging = false;
            }
        }
    }*/

    //#if UNITY_STANDALONE_WIN
    private bool isMouseDown = false;
    private void Update()
    {
        if (!canMove) return;
        Debug.Log(Input.GetAxis("Mouse X"));
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }
        if (isMouseDown)
        {
            if (isHorizontalMode)
            {
                Vector3 offset = new Vector3(transform.position.x + Input.GetAxis("Mouse X")*0.3f, 1, -1);
                //offset.x = Mathf.Clamp(offset.x, -4.8f, 4.8f);

                transform.position = offset;
            }
            else
            {
                Vector3 offset = new Vector3(transform.position.x + Input.GetAxis("Mouse X") * 0.3f, 1, transform.position.z+Input.GetAxis("Mouse Y")*0.3f);
                //offset.x = Mathf.Clamp(offset.x, -4.8f, 4.8f);

                transform.position = offset;
            }
            
        }

    }
    //#endif
    // 切换操作模式
    public void SwitchMode()
    {
        isHorizontalMode = !isHorizontalMode;
        //isDragging = false;
        isMouseDown = false;
    }
}