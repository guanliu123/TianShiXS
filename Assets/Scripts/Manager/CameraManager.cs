using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : BaseManager<CameraManager>
{
    private Camera mainCamera;

    private Dictionary<CameraPointType, Vector3> cameraPos = new Dictionary<CameraPointType, Vector3>();
    private Dictionary<CameraPointType, Vector3> cameraRot = new Dictionary<CameraPointType, Vector3>();

    public CameraManager(){
        mainCamera = Camera.main;
        cameraPos.Add(CameraPointType.MainPoint, new Vector3(0, 4.3f, -7.15f));
        cameraRot.Add(CameraPointType.MainPoint, new Vector3(11.75f, 0, 0));
        cameraPos.Add(CameraPointType.HighPoint, new Vector3(0, 16.78f, -9.18f));
        cameraRot.Add(CameraPointType.HighPoint, new Vector3(45f, 0, 0));

        MonoManager.GetInstance().AddUpdateListener(CameraEvent);    
    }

    private void CameraEvent()
    {
        CameraFollow();
    }

    public void CameraMove(CameraPointType cameraPointType, float moveTime)
    {
        mainCamera.transform.DOMove(cameraPos[cameraPointType], moveTime);
        mainCamera.transform.DORotate(cameraRot[cameraPointType], moveTime);
    }

    private void CameraFollow()
    {
        Transform playerTra = Player._instance.gameObject.transform;

        Vector3 newPoint = new Vector3(playerTra.position.x / 1.5f, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = newPoint;
    }
}
