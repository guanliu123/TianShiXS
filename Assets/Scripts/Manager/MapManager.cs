using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MapManager : BaseManager<MapManager>
{
    private List<GameObject> mapSquares = ResourceManager.GetInstance().Loads<GameObject>(ResourceType.MapSquare);
    private LevelData nowLevel;

    public float movementRate = 5f;

    private float[] mapSquareSize = new float[2] {10, 20 };//����
    public int defaultNum = 5;//��ʼ������ô���ͼ��
    private List<GameObject> exitingSquare=new List<GameObject>();//��ǰ���ϴ��ڵĵ�ͼ��˳��
    public float awayDistance=15f;//�������ĵ������������ôԶʱ����ͼ���ƶ�����ǰ�ˣ����Ҽ������ĩ��

    public int enemyDensity = 5;//�����ܶȣ��򵥶���ΪһƬ������಻�����ɳ�����ô�����
    public List<Vector3> enemyPoints = new List<Vector3>();//�Ѿ������˵��˵ĵ�λ�������ظ���һ���������ɵ���

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
    /// ���ֵ�ͼ���޹�������λ�����ĵ�ͼ�ƶ�����ǰ��ʱ����Ϊ�������µĵ��棬ִ�е��˺͵�������
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
    /// ��һƬ������������ɺ���
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
    /// ��һƬ������������ɼӳ�buff����
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
