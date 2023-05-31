﻿using DG.Tweening;
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
        cameraPos.Add(CameraPointType.MainPoint, new Vector3(0, 3.77f, -3f));
        cameraRot.Add(CameraPointType.MainPoint, new Vector3(25f, 0, 0));
        cameraPos.Add(CameraPointType.HighPoint, new Vector3(0, 16.78f, -9.18f));
        cameraRot.Add(CameraPointType.HighPoint, new Vector3(45f, 0, 0));
        cameraPos.Add(CameraPointType.OrginPoint, new Vector3(2.16f,4.55f,-7.1f));
        cameraRot.Add(CameraPointType.OrginPoint, new Vector3(10.02f,-14.32f,0));

        //MonoManager.GetInstance().AddUpdateListener(CameraEvent);    
    }

    public void StartCameraEvent()
    {
        MonoManager.GetInstance().AddUpdateListener(CameraEvent);
    }
    /*public void StopCameraEvent()
    {
        MonoManager.GetInstance().RemoveUpdeteListener(CameraEvent);
    }*/

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
