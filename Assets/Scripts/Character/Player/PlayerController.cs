using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
        public Transform currTouchObj;
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
        }
    }

}
