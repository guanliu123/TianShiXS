using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class LevelManager : BaseManager<LevelManager>
{
    private List<GameObject> nowSquares = new List<GameObject>();
    private bool isSp;//是否更换了当前预备生成的地图模板

    private bool isChange;//是否进入宽地面战斗模式
    private GameObject checkPoint;

    GameObject boss;
    List<GameObject> existEnemy=new List<GameObject>();

    private LevelData nowLevel;

    public int nowStage { get; private set; }//当前所处阶段数
    public int nowWave { get; private set; }//当前波次
    private int requireEnemy;//当前波次剩余需要生成的敌人数量
    private int requireBOSS;
    //private int requireDoor;//当前波次剩余需要生成的门数量

    public float movementRate = 9f;//地图移动速率

    private float[,] mapSquareSize = new float[2, 2] { { 10, 20 }, { 15, 20 } };//宽，长
    public int defaultNum = 5;//初始生成这么多地图块

    private List<GameObject> exitingSquare = new List<GameObject>();//当前场上存在的地图块
    public float awayDistance = 15f;//当最后面的地面块距离玩家这么远时生成新的地图块，回收原来的

    public int enemyDensity = 5;//敌人密度，简单定义为一片地面最多不能生成超过这么多敌人
    public List<Vector3> enemyPoints = new List<Vector3>();//已经生成了敌人的点位，不能重复在一个点上生成敌人

    public Vector3 specialPoint=new Vector3(0,2,20);//特殊阶段生成固定敌人的大致位置
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
    /// 保持地图无限滚动。当位于最后的地图移动到最前面时，视为生成了新的地面，执行敌人和道具生成
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

        //将一个检查点加到最末尾地图快的边缘，判断玩家与该点位置，若小于某个值，正式进入下一个阶段
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
    /// 回收一片地面上的敌人和道具等等，加入分别的对象池
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
    /// 敌人和门的生成
    /// </summary>
    /// <param name="ground">当前地面</param>
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