using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class LevelManager : BaseManager<LevelManager>
{
    private List<GameObject> nowSquares = new List<GameObject>();
    private bool isSp;//�Ƿ�����˵�ǰԤ�����ɵĵ�ͼģ��

    private bool isChange;//�Ƿ��������ս��ģʽ
    private GameObject checkPoint;

    GameObject boss;
    List<GameObject> existEnemy=new List<GameObject>();

    private LevelData nowLevel;

    public int nowStage { get; private set; }//��ǰ�����׶���
    public int nowWave { get; private set; }//��ǰ����
    private int requireEnemy;//��ǰ����ʣ����Ҫ���ɵĵ�������
    private int requireBOSS;
    //private int requireDoor;//��ǰ����ʣ����Ҫ���ɵ�������

    public float movementRate = 9f;//��ͼ�ƶ�����

    private float[,] mapSquareSize = new float[2, 2] { { 10, 20 }, { 15, 20 } };//����
    public int defaultNum = 5;//��ʼ������ô���ͼ��

    private List<GameObject> exitingSquare = new List<GameObject>();//��ǰ���ϴ��ڵĵ�ͼ��
    public float awayDistance = 15f;//�������ĵ������������ôԶʱ�����µĵ�ͼ�飬����ԭ����

    public int enemyDensity = 5;//�����ܶȣ��򵥶���ΪһƬ������಻�����ɳ�����ô�����
    public List<Vector3> enemyPoints = new List<Vector3>();//�Ѿ������˵��˵ĵ�λ�������ظ���һ���������ɵ���

    public Vector3 specialPoint=new Vector3(0,2,20);//����׶����ɹ̶����˵Ĵ���λ��
    public Vector3 bossPoint = new Vector3(0, 2, 25);
    

    public LevelManager()
    {
        nowLevel = DataManager.GetInstance().AskLevelData(0);
        nowSquares = nowLevel.normalPlanes;
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevel = DataManager.GetInstance().AskLevelData(levelNum - 1);
        nowSquares = nowLevel.normalPlanes;
    }

    private void InitMap()
    {
        exitingSquare.Clear();
        nowSquares = nowLevel.normalPlanes;

        for (int i = 0; i < defaultNum; i++)
        {
            if (exitingSquare.Count == 0)
            {
                exitingSquare.Add(GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)], Vector3.zero, Quaternion.identity));
            }
            else
            {
                Vector3 nextSquare = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.y,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSquareSize[0, 1]);
                exitingSquare.Add(GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)], nextSquare, Quaternion.identity));
            }
        }

        nowStage = 0;
        nowWave = 0;
        isChange = false;
        isSp = false;
        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
    }

    public void Start()
    {
        InitMap();
        MonoManager.GetInstance().AddUpdateListener(LevelEvent);
    }

    public void Stop()
    {
        MonoManager.GetInstance().RemoveUpdeteListener(LevelEvent);
        for(int i = 0; i < exitingSquare.Count; i++)
        {
            RetrieveItem(exitingSquare[i]);
            GameObject.Destroy(exitingSquare[i]);
        }
        exitingSquare.Clear();

        GameObject.Destroy(boss);
        for(int i = 0; i < existEnemy.Count; i++)
        {
            GameObject.Destroy(existEnemy[i]);
        }

        PoolManager.GetInstance().Clear();
        exitingSquare.Clear();
    }

    void LevelEvent()
    {
        SquareMove();
        StageCheck();
    }
    /// <summary>
    /// ���ֵ�ͼ���޹�������λ�����ĵ�ͼ�ƶ�����ǰ��ʱ����Ϊ�������µĵ��棬ִ�е��˺͵�������
    /// </summary>
    void SquareMove()
    {
        if (exitingSquare.Count == 0) return;
        foreach (var item in exitingSquare)
        {
            item.transform.Translate(Vector3.back * movementRate * Time.deltaTime);
        }
        CheckSquare();
    }

    void StageCheck()
    {
        if (checkPoint == null) return;

        if(checkPoint.transform.position.z - Player._instance.transform.position.z <= 0.1f)
        {
            ChangeStage();

            GameObject.Destroy(checkPoint);
            checkPoint = null;
        }
    }

    void CheckSquare()
    {
        if (nowSquares.Count == 0) return;
        if (Vector3.zero.z - exitingSquare[0].transform.position.z >= awayDistance)
        {
            GameObject t = exitingSquare[0];
            GameObject t2 = GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)]);

            exitingSquare.RemoveAt(0);
            RetrieveItem(t);
            GameObject.Destroy(t);

            t2.transform.position = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x, exitingSquare[exitingSquare.Count - 1].transform.position.y,
                exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSquareSize[0, 1]);
            exitingSquare.Add(t2);
            ItemCreate(t2);
        }
    }

    public void PrepareChangeStage()
    {
        //if (isSp == nowLevel.StageDatas[nowStage].isSpecial) return;

        isSp = !isSp;
        if (isSp)
        {
            nowSquares = nowLevel.widthPlanes;
        }
        else
        {
            nowSquares = nowLevel.normalPlanes;
        }

        //��һ������ӵ���ĩβ��ͼ��ı�Ե���ж������õ�λ�ã���С��ĳ��ֵ����ʽ������һ���׶�
        checkPoint = new GameObject("CheckPoint");
        Vector3 t = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x,
                              exitingSquare[exitingSquare.Count - 1].transform.position.y,
                              exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSquareSize[0, 1] / 2);
        checkPoint.transform.position = t;
        checkPoint.transform.parent = exitingSquare[exitingSquare.Count - 1].transform;

        if (requireBOSS > 0)
        {
            BuffDoorCreate(exitingSquare[exitingSquare.Count - 1]);
        }
    }

    public void ChangeStage()
    {
        isChange = !isChange;
        if (isChange)
        {
            GameManager.GetInstance().CameraMove(CameraPointType.HighPoint, 1f);
        }
        else
        {
            
            GameManager.GetInstance().CameraMove(CameraPointType.MainPoint, 1f);
        }
    }

    /// <summary>
    /// ����һƬ�����ϵĵ��˺͵��ߵȵȣ�����ֱ�Ķ����
    /// </summary>
    /// <param name="ground"></param>
    void RetrieveItem(GameObject ground)
    {
        Transform enemyList = ground.transform.Find("EnemyList");
        if (!enemyList) return;

        GameObject.Destroy(enemyList.gameObject);
        /*Transform enemyList = ground.transform.Find("EnemyList");

        if (enemyList != null)
        {
            for (int i = 0; enemyList != null && i < enemyList.childCount; i++)
            {
                var child = enemyList.GetChild(i).gameObject;
                PoolManager.GetInstance().PushObj(child.GetComponent<CharacterBase>().characterType.ToString(), child);
            }

            GameObject.Destroy(enemyList.gameObject);
        }*/

    }

    /// <summary>
    /// ���˺��ŵ�����
    /// </summary>
    /// <param name="ground">��ǰ����</param>
    void ItemCreate(GameObject ground)
    {
        EnemyCreate(ground);
        /*if (requireEnemy <= 0) WaveIncrease();
        if (nowWave == nowLevel.waveNum + 2)
        {
            BuffDoorCreate(ground);
        }
        else if (nowWave == nowLevel.waveNum + 7)
        {
            BOSSCreate();
        }*/
    }

    void EnemyCreate(GameObject ground)
    {
        if (isSp ^ isChange) return;
        if (requireEnemy <= 0)
        {
            if (requireBOSS>0)
            {
                BOSSCreate();
            }
            return;
        }
        
        enemyPoints.Clear();
        GameObject enemyList = new GameObject("EnemyList");
        int n = Random.Range(1, Mathf.Min(requireEnemy, enemyDensity) + 1);

        for (int i = 0; i < n; i++)
        {   
            GameObject t = PoolManager.GetInstance().GetObj(
                nowLevel.StageDatas[nowStage].WaveEnemyType[Random.Range(0, nowLevel.StageDatas[nowStage].WaveEnemyType.Length)].ToString());
            if (t == null) return;

            if (!isChange)
            {
                Vector3 _newPoint = InstantRandomPoint(ground);
                while (enemyPoints.Contains(_newPoint)) _newPoint = InstantRandomPoint(ground);

                t.transform.position = _newPoint;
                t.transform.parent = enemyList.transform;

                enemyList.transform.parent = ground.transform;
            }
            else
            {
                Vector3 _newPoint = InstantRandomPoint();
                while (enemyPoints.Contains(_newPoint)) _newPoint = InstantRandomPoint();

                t.transform.position = _newPoint;
                existEnemy.Add(t);
            }
            requireEnemy--;
        }
    }

    void BuffDoorCreate(GameObject ground)
    {
        //if (requireDoor <= 0) return;

        GameObject t = PoolManager.GetInstance().GetObj(BuffDoorType.BuffDoors.ToString());
        if (t == null) return;

        t.transform.position = new Vector3(
            ground.transform.position.x,
            ground.transform.position.y,
            ground.transform.position.z - mapSquareSize[0, 1] / 2);
        t.transform.parent = ground.transform;

        //requireDoor--;
    }
    void BOSSCreate()
    {
        requireBOSS--;
        GameObject t = PoolManager.GetInstance().GetObj(
            nowLevel.StageDatas[nowStage].BOSSType[Random.Range(0, nowLevel.StageDatas[nowStage].BOSSType.Length)].ToString());

        t.transform.position = new Vector3(bossPoint.x, bossPoint.y + 20f, bossPoint.z);

        t.transform.DOMove(bossPoint, 3f);
        boss = t;
    }

    public void WaveIncrease()
    {
        if (requireEnemy > 0|| requireBOSS>0) return;

        nowWave++;
        if (nowWave >= nowLevel.StageDatas[nowStage].WaveEnemyNum.Length)
        {
            nowStage++;
            if (nowStage >=nowLevel.StageDatas.Count) return;
            nowWave = 0;
        }
        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
        requireBOSS = nowLevel.StageDatas[nowStage].BOSSType.Length;
        if (nowLevel.StageDatas[nowStage].isSpecial) PrepareChangeStage();
    }

    Vector3 InstantRandomPoint(GameObject parent)
    {
        float _x = parent.transform.position.x + Random.Range(-mapSquareSize[0, 0] / 2, mapSquareSize[0, 0] / 2);
        float _z = parent.transform.position.z + Random.Range(-mapSquareSize[0, 1] / 2, mapSquareSize[0, 1] / 2);

        return new Vector3(_x, 1, _z);
    }

    Vector3 InstantRandomPoint()
    {
        float _x = specialPoint.x + Random.Range(-2, 2);
        float _z= specialPoint.z+ Random.Range(-2, 2);

        return new Vector3(_x, 1, _z);
    }
}