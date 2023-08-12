using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : BaseManager<CameraManager>
{
    private Camera mainCamera;
    //private Vector3 originPos;

    private Dictionary<CameraPointType, Vector3> cameraPos = new Dictionary<CameraPointType, Vector3>();
    private Dictionary<CameraPointType, Vector3> cameraRot = new Dictionary<CameraPointType, Vector3>();

    CameraShaker cameraShaker;

    public CameraManager(){
        mainCamera = Camera.main;
        cameraPos.Add(CameraPointType.MainPoint, new Vector3(0, 3.77f, -3f));
        cameraRot.Add(CameraPointType.MainPoint, new Vector3(25f, 0, 0));
        cameraPos.Add(CameraPointType.HighPoint, new Vector3(0, 16.78f, -9.18f));
        cameraRot.Add(CameraPointType.HighPoint, new Vector3(45f, 0, 0));
        cameraPos.Add(CameraPointType.OrginPoint, new Vector3(2.16f,4.55f,-7.1f));
        cameraRot.Add(CameraPointType.OrginPoint, new Vector3(10.02f,-14.32f,0));

        cameraShaker = mainCamera.gameObject.GetComponent<CameraShaker>();
        if (!cameraShaker) cameraShaker = mainCamera.gameObject.AddComponent<CameraShaker>();
        //MonoManager.GetInstance().AddUpdateListener(CameraEvent);    
    }

    public void StartCameraEvent()
    {
        mainCamera = Camera.main;
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

        Vector3 newPoint = new Vector3(playerTra.position.x / 1.4f, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = newPoint;
    }
    
    public void CameraShake(float duration,float strength)
    {
        cameraShaker.Shake(strength, duration);
        //MonoManager.GetInstance().StartCoroutine(Shake(duration,strength));
    }
    /*private IEnumerator Shake(float duration,float strength)
    {
        Vector3 originPos = mainCamera.transform.position;
        float elapsed = 0.0f;//摇晃进行时间
        strength *= 0.05F;
        while (elapsed < duration)
        {
            float x = Random.Range(-2f, 2f) * strength;//x轴随机抖动幅度
            float y = Random.Range(-2f, 2f) * strength;//y轴随机抖动幅度

            Vector3 t = new Vector3(originPos.x+x, originPos.y+y, originPos.z);
            //mainCamera.transform.localPosition = t;


            elapsed += Time.deltaTime;

            yield return new WaitForSeconds(Time.deltaTime);
        }
        mainCamera.transform.localPosition = originPos;//再次复原
    }*/
}
