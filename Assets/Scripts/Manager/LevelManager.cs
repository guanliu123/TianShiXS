using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UIFrameWork;

public class LevelManager : BaseManager<LevelManager>
{
    private List<GameObject> nowSquares = new List<GameObject>();

    private bool isSp;//是否更换了当前预备生成的地图模板
    public bool isChange;//是否进入宽地面战斗模式
    private GameObject checkPoint;

    GameObject boss;

    private LevelData nowLevel;

    public int maxStage{ get; private set; }
    public int nowStage { get; private set; }//当前所处阶段数
    public int nowWave { get; private set; }//当前波次
    private int requireEnemy;//当前波次剩余需要生成的敌人数量
    private int requireBOSS;
    //private int requireDoor;//当前波次剩余需要生成的门数量

    public float movementRate = 9f;//地图移动速率

    public float[] mapSize;
    public int defaultNum = 13;//初始生成这么多地图块

    private List<GameObject> exitingSquare = new List<GameObject>();//当前场上存在的地图块
    private List<GameObject> distanceSquare = new List<GameObject>();//距离玩家较远的方块（可以直接替换等等）

    private float awayDistance = 30f;//当最后面的地面块距离玩家这么远时生成新的地图块，回收原来的
    private float closeDistance = 35f;//当前方的地面离玩家这么近时生成敌人、从远处地面列表移除

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

    private void InitLevel()
    {
        exitingSquare.Clear();
        distanceSquare.Clear();

        nowStage = 0;
        maxStage = nowLevel.StageDatas.Count;
        nowWave = 0;
        isChange = false;
        isSp = nowLevel.StageDatas[nowStage].isSpecial;
        if (isSp)
        {
            nowSquares = nowLevel.widthPlanes;
            mapSize = nowLevel.widthSize;
        }
        else
        {
            nowSquares = nowLevel.normalPlanes;
            mapSize = nowLevel.normalSize;
        }

        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
        requireBOSS = nowLevel.StageDatas[nowStage].BOSSType.Length;
        
        
        Camera.main.GetComponent<Skybox>().material = nowLevel.skybox;

        for (int i = 0; i < defaultNum; i++)
        {
            if (exitingSquare.Count == 0)
            {
                GameObject t = nowSquares[Random.Range(0, nowSquares.Count)];
                RandomSet(t);

                exitingSquare.Add(GameObject.Instantiate(t, Vector3.zero, Quaternion.identity));
            }
            else
            {
                Vector3 nextSquare = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.y,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSize[0]);
                GameObject square = GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)], nextSquare, Quaternion.identity);

                RandomSet(square);

                if (nextSquare.z - Vector3.zero.z > closeDistance) distanceSquare.Add(square);
                exitingSquare.Add(square);
            }
        }

        
    }

    public void Start()
    {
        InitLevel();       
        MonoManager.GetInstance().AddUpdateListener(LevelEvent);
    }

    public override void Reset()
    {
        for (int i = 0; i < exitingSquare.Count; i++)
        {
            GameObject.Destroy(exitingSquare[i]);
        }
        exitingSquare.Clear();

        GameObject.Destroy(boss);
    }

    void LevelEvent()
    {
        SquareMove();
        StageCheck();
    }

    /// <summary>
    /// 保持地图无限滚动。当地面移动到离玩家一定距离时生成敌人，同时从“远处地面”列表移除
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
        if (Vector3.zero.z - exitingSquare[0].transform.position.z >= awayDistance)
        {
            GameObject t = exitingSquare[0];
            GameObject t2 = GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)]);

            RandomSet(t2);           

            exitingSquare.RemoveAt(0);
            RecoveryGround(t);
            //PoolManager.GetInstance().PushObj("Ground",t);

            t2.transform.position = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x, exitingSquare[exitingSquare.Count - 1].transform.position.y,
                exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSize[0]);
            exitingSquare.Add(t2);
            if (t2.transform.position.z - Vector3.zero.z > closeDistance) distanceSquare.Add(t2);
        }
        if (distanceSquare[0].transform.position.z - Vector3.zero.z <= closeDistance)
        {
            EnemyCreate(distanceSquare[0]);
            distanceSquare.RemoveAt(0);
        }
    }
    private void RandomSet(GameObject ground)
    {
        Transform t = ground.transform.Find("Setting");//地板上的布景列表
        if (!t) return;
        HashSet<int> indices = new HashSet<int>();

        int n = Random.Range(0, t.childCount);
        for (int i = 0; i < t.childCount; i++) t.GetChild(i).gameObject.SetActive(false);
        for(int i = 0; i < n; i++)
        {
            int r;
            do
            {
                r = Random.Range(0, t.childCount);
            } while (indices.Contains(r));
            t.GetChild(r).gameObject.SetActive(true);
        }
    }

    public void PrepareChangeStage()
    {
        isSp = !isSp;
        if (isSp)
        {
            nowSquares = nowLevel.widthPlanes;
        }
        else
        {
            nowSquares = nowLevel.normalPlanes;
            GameManager.GetInstance().PlayerReset();
            GameManager.GetInstance().LockMove();
        }
        ChangeEnvironment();     

        //将一个检查点加到最末尾地图快的边缘，判断玩家与该点位置，若小于某个值，正式进入下一个阶段
        checkPoint = new GameObject("CheckPoint");
        Vector3 t = new Vector3(distanceSquare[0].transform.position.x,
                              distanceSquare[0].transform.position.y,
                              distanceSquare[0].transform.position.z - mapSize[0] / 2);
        checkPoint.transform.position = t;
        checkPoint.transform.parent = distanceSquare[0].transform;

        if (requireBOSS > 0)
        {
            BuffDoorCreate(distanceSquare[0]);
        }
    }
    private void ChangeEnvironment()
    {
        for(int i = distanceSquare.Count-1; i >= 0; i--)
        {
            GameObject newSquare = GameObject.Instantiate(nowSquares[Random.Range(0, nowSquares.Count)]);
            GameObject t = distanceSquare[i];
            newSquare.transform.position = t.transform.position;
            RandomSet(newSquare);
            distanceSquare[i] = newSquare;
            exitingSquare[(exitingSquare.Count - distanceSquare.Count) + i] = newSquare;

            RecoveryGround(t);
        }
    }

    //加上切换天空盒
    public void ChangeStage()
    {
        isChange = !isChange;
        if (isChange)
        {
            mapSize = nowLevel.widthSize;
            GameManager.GetInstance().CameraMove(CameraPointType.HighPoint, 1f);
        }
        else
        {
            GameManager.GetInstance().PlayerReset();
            mapSize = nowLevel.normalSize;
            GameManager.GetInstance().CameraMove(CameraPointType.MainPoint, 1f);

            /*smoke = PoolManager.GetInstance().GetObj("Smoke");
            smoke.transform.position= new Vector3(0, -6, 40);*/
        }
        GameManager.GetInstance().SwitchMode();
        GameManager.GetInstance().UnlockMove();
    }

    /// <summary>
    /// 回收一片地面上的敌人和道具等等，加入分别的对象池
    /// </summary>
    /// <param name="ground"></param>
    void RecoveryGround(GameObject ground)
    {
        Transform enemyList = ground.transform.Find("EnemyList");
        if (enemyList)//回收敌人
        {
            enemyList.transform.SetParent(null);
            for (int i = 0; enemyList != null && i < enemyList.childCount; i++)
            {
                var child = enemyList.GetChild(i).gameObject;
                child.GetComponent<CharacterBase>().Recovery();
            }
            GameObject.Destroy(enemyList.gameObject);
        }

        GameObject.Destroy(ground);
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
                Vector3 _newPoint;
                do
                {
                    _newPoint = InstantRandomPoint(ground);
                }while (enemyPoints.Contains(_newPoint));

                t.transform.position = _newPoint;
                //t.transform.parent = ground.transform;
                t.transform.parent = enemyList.transform;

                enemyList.transform.parent = ground.transform;
            }
            else
            {
                Vector3 _newPoint;
                do
                {
                    _newPoint = InstantRandomPoint();
                } while (enemyPoints.Contains(_newPoint));

                t.transform.position = _newPoint;
            }
            requireEnemy--;
        }
    }

    void BuffDoorCreate(GameObject ground)
    {
        GameObject t = PoolManager.GetInstance().GetObj(BuffDoorType.BuffDoors.ToString());
        if (t == null) return;

        t.transform.position = new Vector3(
            ground.transform.position.x,
            ground.transform.position.y,
            ground.transform.position.z - mapSize[0] / 2);
        t.transform.parent = ground.transform;
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
            if (nowStage >= nowLevel.StageDatas.Count)
            {
                //结束游戏               
                PanelManager.Instance.Push(new SuccessPanel());
                return;
            }         
            nowWave = 0;
        }
        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
        requireBOSS = nowLevel.StageDatas[nowStage].BOSSType.Length;
        if (nowLevel.StageDatas[nowStage].isSpecial != isChange)
        {
            PrepareChangeStage();
        }
    }

    Vector3 InstantRandomPoint(GameObject parent)
    {
        float _x = parent.transform.position.x + Random.Range(-mapSize[1] / 2, mapSize[1] / 2);
        float _z = parent.transform.position.z + Random.Range(-mapSize[0] / 2, mapSize[0] / 2);

        return new Vector3(_x, 0, _z);
    }

    Vector3 InstantRandomPoint()
    {
        float _x = specialPoint.x + Random.Range(-2, 2);
        float _z= specialPoint.z+ Random.Range(-2, 2);

        return new Vector3(_x, 0, _z);
    }
}