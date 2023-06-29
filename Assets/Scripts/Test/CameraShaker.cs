using UnityEngine;
using System.Collections;
public class CameraShaker : MonoBehaviour
{
    // 震动标志位
    private bool isshakeCamera = false;
    // 震动幅度
    public float shakeLevel;
    // 震动时间
    public float ShakeTime;
    // 震动的帧率
    public float shakeFps = 45f;

    private float shakeFrameTime;
    private float timer = 0.0f;
    private float frameTime = 0.0f;
    private float shakeDelta = 0.005f;
    private Camera selfCamera;
    private Rect DefaultRect;
    private Rect changeRect;

    void Awake()
    {
        selfCamera = GetComponent<Camera>();
        DefaultRect = selfCamera.rect;
        shakeFrameTime = 1f / shakeFps;
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (isshakeCamera)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Reset();
                    selfCamera.rect = DefaultRect;
                }
                else
                {
                    frameTime += Time.deltaTime;
                    if (frameTime >= shakeFrameTime)
                    {
                        frameTime -= shakeFrameTime;
                        changeRect.x = DefaultRect.x + shakeDelta * (shakeLevel * (Random.value - 0.5f));
                        changeRect.y = DefaultRect.y + shakeDelta * (shakeLevel * (Random.value - 0.5f));

                        selfCamera.rect = changeRect;
                    }
                }
            }
        }
    }

    void Reset()
    {
        isshakeCamera = false;

        timer = ShakeTime;
        frameTime = shakeFrameTime;
        changeRect = DefaultRect;
    }

    public void Shake(float _shakeLevel,float _shakeTime)
    {
        shakeLevel = _shakeLevel;
        ShakeTime = _shakeTime;
        isshakeCamera = true;
    }
}