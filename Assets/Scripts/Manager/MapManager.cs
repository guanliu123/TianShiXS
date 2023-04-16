using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MapManager : BaseManager<MapManager>
{
    private List<GameObject> mapSquares=new List<GameObject>();

    private bool isStart;

    private LevelData nowLevel;

    private int nowWave;//��ǰ����
    private int requireEnemy;//��ǰ����ʣ����Ҫ���ɵĵ�������
    private int requireDoor;//��ǰ����ʣ����Ҫ���ɵ�������

    public float movementRate = 5f;

    private float[] mapSquareSize = new float[2] { 10, 20 };//����
    public int defaultNum = 7;//��ʼ������ô���ͼ��
    private List<GameObject> exitingSquare = new List<GameObject>();//��ǰ���ϴ��ڵĵ�ͼ��
    public float awayDistance = 15f;//�������ĵ������������ôԶʱ����ͼ���ƶ�����ǰ�ˣ����Ҽ������ĩ��

    public int enemyDensity = 5;//�����ܶȣ��򵥶���ΪһƬ������಻�����ɳ�����ô�����
    public List<Vector3> enemyPoints = new List<Vector3>();//�Ѿ������˵��˵ĵ�λ�������ظ���һ���������ɵ���
    public int doorDensity = 1;//buff���ܶ�
    public Vector3 bossPoint=new Vector3(0,0,35);

    private Vector3 cameraPoint = new Vector3(0, 13.41f, -7.37f);
    private Vector3 cameraRotation = new Vector3(40, 0, 0);

    public MapManager()
    {
        nowLevel = DataManager.GetInstance().AskLevelData(0);
        mapSquares = nowLevel.planes;
    }

    public void StartGame()
    {
        isStart = true;
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

        player.AddComponent<Taoist_priest>();
        player.AddComponent<TestController>();

        Camera mainCamera = Camera.main;
        mainCamera.transform.DOMove(cameraPoint, 1f);
        mainCamera.transform.DORotate(cameraRotation, 1f);
    }
    private void InitMap()
    {
        //Debug.Log(mapSquares);
        exitingSquare.Clear();
        for (int i = 0; i < defaultNum; i++)
        {
            if (exitingSquare.Count == 0)
            {
                //Debug.Log(mapSquares);
                //Debug.Log(GameObject.Instantiate(mapSquares[Random.Range(0, mapSquares.Count)]));
                exitingSquare.Add(GameObject.Instantiate(mapSquares[Random.Range(0, mapSquares.Count)], Vector3.zero, Quaternion.identity));
            }
            else
            {
                Vector3 nextSquare = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.y,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSquareSize[1]);
                exitingSquare.Add(GameObject.Instantiate(mapSquares[Random.Range(0, mapSquares.Count)], nextSquare, Quaternion.identity));
            }
        }

        nowWave = 1;
        requireEnemy = nowLevel.baseEnemyNum;
        requireDoor = nowLevel.baseBuffDoorNum;
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevel = DataManager.GetInstance().AskLevelData(levelNum - 1);
        mapSquares = nowLevel.planes;
    }

    public void StartMapCreate()
    {   
        InitMap(); 
        MonoManager.GetInstance().AddUpdateListener(MapMove);
    }

    /// <summary>
    /// ���ֵ�ͼ���޹�������λ�����ĵ�ͼ�ƶ�����ǰ��ʱ����Ϊ�������µĵ��棬ִ�е��˺͵�������
    /// </summary>
    void MapMove()
    {
        foreach (var item in exitingSquare)
        {
            item.transform.Translate(Vector3.back * movementRate * Time.deltaTime);
        }
        if (Vector3.zero.z - exitingSquare[0].transform.position.z >= awayDistance)
        {
            GameObject t = exitingSquare[0];
            exitingSquare.RemoveAt(0);
            RetrieveItem(t);
            t.transform.position = new Vector3(t.transform.position.x, t.transform.position.y,
                exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSquareSize[1]);
            exitingSquare.Add(t);
            ItemCreate(t);
        }
    }

    /// <summary>
    /// ����һƬ�����ϵĵ��˺͵��ߵȵȣ�����ֱ�Ķ����
    /// </summary>
    /// <param name="ground"></param>
    void RetrieveItem(GameObject ground)
    {
        
        Transform enemyList = ground.transform.Find("EnemyList");

        if (enemyList != null)
        {
            for (int i = 0; enemyList != null && i < enemyList.childCount; i++)
            {
                var child = enemyList.GetChild(i).gameObject;
                PoolManager.GetInstance().PushObj("Enemy", child);
            }

            GameObject.Destroy(enemyList.gameObject);
        }

    }

    /// <summary>
    /// ��һƬ������������ɵ��˺���
    /// </summary>
    /// <param name="ground"></param>
    void ItemCreate(GameObject ground)
    {
        if (!isStart) return;
        EnemyCreate(ground);
        if (requireEnemy <= 0) WaveIncrease();
        if (nowWave == nowLevel.waveNum + 2)
        {
            BuffDoorCreate(ground);
        }
        else if (nowWave == nowLevel.waveNum + 7)
        {
            BOSSCreate();
        }
    }

    void EnemyCreate(GameObject ground)
    {
        GameObject enemyList = new GameObject("EnemyList");
        if (requireEnemy <= 0) return;
        int n = Random.Range(1, Mathf.Min(requireEnemy, enemyDensity)+1);

        for (int i = 0; i < n; i++)
        {
            Vector3 _newPoint = InstantRandomPoint(ground);
            GameObject t = PoolManager.GetInstance().GetObj(nowLevel.enemyTypes[Random.Range(0,nowLevel.enemyTypes.Count)].ToString());
            if (t == null) return;

            while (enemyPoints.Contains(_newPoint)) _newPoint = InstantRandomPoint(ground);
      
            t.transform.position = _newPoint;
            t.transform.parent = enemyList.transform;

            requireEnemy--;
        }

        enemyList.transform.parent = ground.transform;
    }

    /*void BuffDoorCreate(GameObject ground)
    {
        GameObject buffDoorList = new GameObject("BuffDoorList");
        if (requireDoor <= 0) return;
        int n = Random.Range(0, 2);

        GameObject t = PoolManager.GetInstance().GetObj(NameCenter.BuffDoorName);
        if (t == null) return;
        Vector3 _newPoint = InstantRandomPoint(ground);
        t.transform.position = new Vector3(ground.transform.position.x,ground.transform.position.y,_newPoint.z);
        t.transform.parent = buffDoorList.transform;

        requireDoor--;

        buffDoorList.transform.parent = ground.transform;
    }*/

    void BuffDoorCreate(GameObject ground)
    {
        if (requireDoor <= 0) return;

        GameObject t = PoolManager.GetInstance().GetObj(BuffDoorType.BuffDoors.ToString());
        if (t == null) return;

        t.transform.position = new Vector3(
            ground.transform.position.x,
            ground.transform.position.y,
            ground.transform.position.z - mapSquareSize[1] / 2);
        t.transform.parent = ground.transform;

        requireDoor--;
    }
    void BOSSCreate()
    {
        GameObject t = PoolManager.GetInstance().GetObj(nowLevel.bossType.ToString());

        t.transform.position = new Vector3(bossPoint.x,bossPoint.y+20f,bossPoint.z);

        t.transform.DOMove(bossPoint, 3f);
    }

    void WaveIncrease()
    {
        nowWave++;
        if (nowWave > nowLevel.waveNum) return;
        requireEnemy = nowLevel.baseEnemyNum + nowWave * nowLevel.riseEnemyNum;
        requireDoor = nowLevel.baseBuffDoorNum + nowWave * nowLevel.riseBuffDoorNum;
    }

    Vector3 InstantRandomPoint(GameObject parent)
    {
        float _x = parent.transform.position.x + Random.Range(-mapSquareSize[0] / 2, mapSquareSize[0] / 2);
        float _z = parent.transform.position.z + Random.Range(-mapSquareSize[1] / 2, mapSquareSize[1] / 2);

        return new Vector3(_x, 1, _z);
    }
}
