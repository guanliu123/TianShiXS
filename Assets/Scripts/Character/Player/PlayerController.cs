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

    private Vector3 touchOrigin; // 手指按下的屏幕位置
    private Vector3 touchPosition;//手指最新按下的位置

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
                    Debug.Log(1);
                    dragDir.y = 0;
                }

                
                transform.Translate(dragDir * 10f*Time.deltaTime);
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

/*using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 定义手指按住屏幕时的位置和角色物体
    private Vector2 fingerDown;
    private GameObject character;

    // 定义水平和平面移动时的速度以及移动的最大距离
    public float horizontalSpeed = 5f;
    public float planeSpeed = 10f;
    public float maxPlaneDistance = 10f;

    // 定义当前的操作模式
    private bool isHorizontalMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        // 获取角色物体
        character = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 当玩家手指按下屏幕时
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // 获取手指按下的位置
            fingerDown = Input.GetTouch(0).position;

            // 检测是否按住角色物体
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(fingerDown);
            if (fingerDown.y<Screen.height/3||Physics.Raycast(ray, out hit) && hit.transform.gameObject == character)
            {
                // 将操作模式切换为水平移动
                isHorizontalMoving = true;
            }
        }

        // 根据操作模式进行移动
        if (isHorizontalMoving)
        {
            // 控制角色左右移动
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // 获取手指移动的距离
                Vector2 delta = Input.GetTouch(0).deltaPosition;

                // 计算移动的距离
                float deltaX = delta.x * Time.deltaTime * horizontalSpeed;

                // 限制移动范围在水平方向上
                float newX = Mathf.Clamp(character.transform.position.x + deltaX, -maxPlaneDistance, maxPlaneDistance);

                // 移动角色
                character.transform.position = new Vector3(newX, character.transform.position.y, character.transform.position.z);
            }
        }
        else
        {
            // 控制角色平面移动
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                // 获取手指移动的距离
                Vector2 delta = Input.GetTouch(0).deltaPosition;

                // 计算移动的距离
                float deltaX = delta.x * Time.deltaTime * planeSpeed;
                float deltaZ = delta.y * Time.deltaTime * planeSpeed;

                // 限制移动范围在水平面上
                float newZ = Mathf.Clamp(character.transform.position.z + deltaZ, -maxPlaneDistance, maxPlaneDistance);
                float newX = Mathf.Clamp(character.transform.position.x + deltaX, -maxPlaneDistance, maxPlaneDistance);

                // 移动角色
                character.transform.position = new Vector3(newX, character.transform.position.y, newZ);
            }
        }

        // 当玩家手指离开屏幕时，重置操作模式
        if (Input.touchCount == 0)
        {
            isHorizontalMoving = false;
        }
    }

    // 提供函数以供其他脚本调用以在这两种操作模式间切换
    public void SwitchMode()
    {
        isHorizontalMoving = !isHorizontalMoving;
    }
}*/