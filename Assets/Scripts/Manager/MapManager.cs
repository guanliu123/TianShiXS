using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapManager : BaseManager<MapManager>
{
    private List<GameObject> mapSquares = ResourceManager.GetInstance().Loads<GameObject>(ResourceType.MapSquare);
    private LevelData nowLevel;

    public float movementRate = 5f;

    private float[] mapSquareSize = new float[2] {10, 20 };//宽，长
    public int defaultNum = 5;//初始生成这么多地图块
    private List<GameObject> exitingSquare=new List<GameObject>();//当前场上存在的地图块顺序
    public float awayDistance=15f;//当最后面的地面块距离玩家这么远时将地图块移动到最前端，并且加入队列末端

    public int enemyDensity = 5;//敌人密度，简单定义为一片地面最多不能生成超过这么多敌人
    public List<Vector3> enemyPoints = new List<Vector3>();//已经生成了敌人的点位，不能重复在一个点上生成敌人

    public MapManager()
    {
        if (mapSquares.Count <=0) return;

        nowLevel = DataManager.GetInstance().AskLevelData(0);
        
    }
    private void InitMapSquare()
    {
        for (int i = 0; i < defaultNum; i++)
        {
            if (exitingSquare.Count == 0)
            {
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
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevel = DataManager.GetInstance().AskLevelData(levelNum-1);
    }

    public void StartMapCreate()
    {
        InitMapSquare();
        MonoManager.GetInstance().AddUpdateListener(MapRunning);
    }

    private void MapRunning()
    {
        MapMove();
    }

    /// <summary>
    /// 保持地图无限滚动。当位于最后的地图移动到最前面时，视为生成了新的地面，执行敌人和道具生成
    /// </summary>
    void MapMove()
    {
        foreach(var item in exitingSquare)
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
    /// 回收一片地面上的敌人和道具等等，加入分别的对象池
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
    /// 在一片地面上随机生成和门
    /// </summary>
    /// <param name="ground"></param>
    void ItemCreate(GameObject ground)
    {
        GameObject enemyList = new GameObject("EnemyList");
        int n = Random.Range(0, enemyDensity + 1);

        for(int i = 0; i < n; i++)
        {
            Vector3 _newPoint = InstantRandomPoint(ground);
            while(enemyPoints.Contains(_newPoint)) _newPoint = InstantRandomPoint(ground);

            GameObject t = PoolManager.GetInstance().GetObj(NameCenter.BatName);
            if (t == null) return;
            t.transform.position = _newPoint;
            t.transform.parent = enemyList.transform;
        }

        enemyList.transform.parent = ground.transform;
    }

    /// <summary>
    /// 在一片地面上随机生成加成buff的门
    /// </summary>
    /// <param name="ground"></param>
    void BuffdoorCreate(GameObject ground)
    {

    }

    Vector3 InstantRandomPoint(GameObject parent)
    {
        float _x = parent.transform.position.x + Random.Range(-mapSquareSize[0] / 2, mapSquareSize[0] / 2);
        float _z = parent.transform.position.z + Random.Range(-mapSquareSize[1] / 2, mapSquareSize[1] / 2);

        return new Vector3(_x, 1, _z);  
    }
}
