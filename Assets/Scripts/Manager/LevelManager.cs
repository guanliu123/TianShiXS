using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using UIFrameWork;
using System.Threading.Tasks;

public class LevelManager : BaseManager<LevelManager>
{
    public Dictionary<int, LevelData> levelDatasDic = new Dictionary<int, LevelData>();

    private List<GameObject> nowPlane = new List<GameObject>();

    private bool isSp;//�Ƿ�����˵�ǰԤ�����ɵĵ�ͼģ��
    public bool isChange;//�Ƿ��������ս��ģʽ
    private GameObject checkPoint;

    GameObject boss;

    public int nowLevelNum;
    private LevelData nowLevel;

    public int maxStage { get; private set; }
    public int nowStage { get; private set; }//��ǰ�����׶���
    public int nowWave { get; private set; }//��ǰ����
    private int requireEnemy;//��ǰ����ʣ����Ҫ���ɵĵ�������
    private int requireBOSS;
    //private int requireDoor;//��ǰ����ʣ����Ҫ���ɵ�������

    public float movementRate = 9f;//��ͼ�ƶ�����

    public float[] mapSize;
    public int defaultNum = 13;//��ʼ������ô���ͼ��

    private List<GameObject> exitingSquare = new List<GameObject>();//��ǰ���ϴ��ڵĵ�ͼ��
    private List<GameObject> distanceSquare = new List<GameObject>();//������ҽ�Զ�ķ��飨����ֱ���滻�ȵȣ�

    private float awayDistance = 30f;//�������ĵ������������ôԶʱ�����µĵ�ͼ�飬����ԭ����
    private float closeDistance = 35f;//��ǰ���ĵ����������ô��ʱ���ɵ��ˡ���Զ�������б��Ƴ�

    public int enemyDensity = 5;//�����ܶȣ��򵥶���ΪһƬ������಻�����ɳ�����ô�����
    public List<Vector3> enemyPoints = new List<Vector3>();//�Ѿ������˵��˵ĵ�λ�������ظ���һ���������ɵ���

    public Vector3 specialPoint = new Vector3(0, 2, 20);//����׶����ɹ̶����˵Ĵ���λ��
    public Vector3 bossPoint = new Vector3(0, 2, 25);

    //======================������Դ�ľ��======================//
    List<Task> waitList = new List<Task>();
    private List<GameObject> normalPlanes=new List<GameObject>();
    private List<GameObject> widthPlanes = new List<GameObject>();
    private Dictionary<int,GameObject> enemys = new Dictionary<int, GameObject>();
    private Material skybox;

    public LevelManager()
    {
        levelDatasDic = LevelDataTool.ReadLevelData();

        nowLevel = levelDatasDic[1];
    }

    public void ChangeLevel(int levelNum)
    {
        nowLevelNum = levelNum;
        nowLevel = levelDatasDic[nowLevelNum];
        //nowPlane = nowLevel.normalPlanes;
    }

    public async Task LoadLevelRes()
    {
        waitList.Clear();
        normalPlanes.Clear();
        widthPlanes.Clear();

        waitList.Add(ResourceManager.GetInstance().LoadRes<Material>(nowLevel.skyboxName, result => {
            skybox = result;
        }, ResourceType.Skybox));
        for(int i = 0; i < nowLevel.normalPlanes.Count; i++)
        {
            waitList.Add(ResourceManager.GetInstance().LoadRes<GameObject>(nowLevel.normalPlanes[i], result =>
            {
                normalPlanes.Add(result);
            }, ResourceType.MapGround));
        }
        for (int i = 0; i < nowLevel.widthPlanes.Count; i++)
        {
            waitList.Add(ResourceManager.GetInstance().LoadRes<GameObject>(nowLevel.widthPlanes[i], result =>
            {
                widthPlanes.Add(result);
            }, ResourceType.MapGround));
        }
        foreach(var item in nowLevel.StageDatas)
        {
            foreach(var t in item.BOSSList)
            {
                if (!enemys.ContainsKey(t))
                {
                    enemys.Add(t, new GameObject());
                    LoadEnemy(t);
                }
            }
            foreach(var t in item.WaveEnemyList)
            {
                if (!enemys.ContainsKey(t))
                {
                    enemys.Add(t, new GameObject());
                    LoadEnemy(t);
                }                   
            }
        }

        await Task.WhenAll(waitList);
    }
    private void LoadEnemy(int id)
    {
        waitList.Add(ResourceManager.GetInstance().LoadRes<GameObject>(id.ToString(), result =>
        {
            if (!enemys.ContainsKey(id)) enemys.Add(id, result);
            else enemys[id] = result;
        }, ResourceType.Enemy));
    }

    public void InitLevel()
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
            nowPlane = widthPlanes;
            mapSize = nowLevel.widthSize;
        }
        else
        {
            nowPlane = normalPlanes;
            mapSize = nowLevel.normalSize;
        }

        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
        requireBOSS = nowLevel.StageDatas[nowStage].BOSSList.Count;

        Camera.main.GetComponent<Skybox>().material = skybox;
        for (int i = 0; i < defaultNum; i++)
        {
            if (exitingSquare.Count == 0)
            {
                GameObject t= GameObject.Instantiate(nowPlane[Random.Range(0, nowPlane.Count)],Vector3.zero,Quaternion.identity);
                RandomSet(t);
                exitingSquare.Add(t);
            }
            else
            {
                Vector3 nextSquare = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.y,
                                                 exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSize[0]);
                GameObject t = GameObject.Instantiate(nowPlane[Random.Range(0, nowPlane.Count)], nextSquare, Quaternion.identity);
                RandomSet(t);
                if (nextSquare.z - Vector3.zero.z > closeDistance) distanceSquare.Add(t);
                exitingSquare.Add(t);
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
        Debug.Log("-----------LevelEvent-------");
        SquareMove();
        StageCheck();
    }

    /// <summary>
    /// ���ֵ�ͼ���޹������������ƶ��������һ������ʱ���ɵ��ˣ�ͬʱ�ӡ�Զ�����桱�б��Ƴ�
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

        if (checkPoint.transform.position.z - Player._instance.transform.position.z <= 0.1f)
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
            exitingSquare.RemoveAt(0);
            RecoveryGround(t);

            GameObject t2 = GameObject.Instantiate(nowPlane[Random.Range(0, nowPlane.Count)]);
            RandomSet(t2);
            t2.transform.position = new Vector3(exitingSquare[exitingSquare.Count - 1].transform.position.x, exitingSquare[exitingSquare.Count - 1].transform.position.y,
                    exitingSquare[exitingSquare.Count - 1].transform.position.z + mapSize[0]);
            exitingSquare.Add(t2);
            if (t2.transform.position.z - Vector3.zero.z > closeDistance) distanceSquare.Add(t2);
        }
        if (distanceSquare.Count <= 0) return;
        if (distanceSquare[0].transform.position.z - Vector3.zero.z <= closeDistance)
        {
            EnemyCreate(distanceSquare[0]);
            distanceSquare.RemoveAt(0);
        }
    }
    private void RandomSet(GameObject ground)
    {
        Transform t = ground.transform.Find("Setting");//�ذ��ϵĲ����б�
        if (!t) return;
        HashSet<int> indices = new HashSet<int>();

        int n = Random.Range(0, t.childCount);
        for (int i = 0; i < t.childCount; i++) t.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < n; i++)
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
            nowPlane = widthPlanes;
        }
        else
        {
            nowPlane = normalPlanes;
            BulletManager.GetInstance().ClearExistBullet();
            GameManager.GetInstance().PlayerReset();
            GameManager.GetInstance().LockMove();
        }
        ChangeEnvironment();

        //��һ������ӵ���ĩβ��ͼ��ı�Ե���ж������õ�λ�ã���С��ĳ��ֵ����ʽ������һ���׶�
        checkPoint = new GameObject("CheckPoint");
        Vector3 t1 = new Vector3(distanceSquare[0].transform.position.x,
                              distanceSquare[0].transform.position.y,
                              distanceSquare[0].transform.position.z - mapSize[0] / 2);
        checkPoint.transform.position = t1;
        checkPoint.transform.parent = distanceSquare[0].transform;

        if (requireBOSS > 0)
        {
            BuffDoorCreate(distanceSquare[0]);
        }
    }
    private void ChangeEnvironment()
    {
        for (int i = distanceSquare.Count - 1; i >= 0; i--)
        {            
            GameObject t = distanceSquare[i];
            GameObject newSquare = GameObject.Instantiate(nowPlane[Random.Range(0, nowPlane.Count)]);
            newSquare.transform.position = t.transform.position;
            RandomSet(newSquare);
            distanceSquare[i] = newSquare;
            exitingSquare[(exitingSquare.Count - distanceSquare.Count) + i] = newSquare;

            RecoveryGround(t);
        }
    }

    //�����л���պ�
    public void ChangeStage()
    {
        Debug.Log("-------------ChangeStage----------------");
        isChange = !isChange;
        if (isChange)
        {
            Debug.Log("-------------CameraMove HighPoint begin----------------");
            Debug.Log($"-------------CameraMove HighPoint begin nowLevel:{nowLevel}----------------");
            mapSize = nowLevel.widthSize;
            GameManager.GetInstance().CameraMove(CameraPointType.HighPoint, 1f);
            Debug.Log("-------------CameraMove HighPoint end----------------");
        }
        else
        {
            Debug.Log("-------------CameraMove MainPoint begin----------------");
            Debug.Log($"-------------CameraMove MainPoint begin nowLevel:{nowLevel}----------------");
            mapSize = nowLevel.normalSize;
            GameManager.GetInstance().CameraMove(CameraPointType.MainPoint, 1f);
            GameManager.GetInstance().UnlockMove();
            Debug.Log("-------------CameraMove MainPoint end----------------");
            /*smoke = PoolManager.GetInstance().GetObj("Smoke");
            smoke.transform.position= new Vector3(0, -6, 40);*/
        }
        GameManager.GetInstance().SwitchMode();
    }

    /// <summary>
    /// ����һƬ�����ϵĵ��˺͵��ߵȵȣ�����ֱ�Ķ����
    /// </summary>
    /// <param name="ground"></param>
    void RecoveryGround(GameObject ground)
    {
        Transform enemyList = ground.transform.Find("EnemyList");
        if (enemyList)//���յ���
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

    /// <summary>
    /// ����ˢ��
    /// </summary>
    /// <param name="ground">���ڵذ�</param>
    void EnemyCreate(GameObject ground)
    {
        if (isSp ^ isChange) return;

        if (requireEnemy <= 0)
        {
            if (requireBOSS > 0)
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
            if (!isChange)
            {
                Vector3 _newPoint;
                do
                {
                    _newPoint = InstantRandomPoint(ground);
                } while (enemyPoints.Contains(_newPoint));

                enemyPoints.Add(_newPoint);
                PoolManager.GetInstance().GetObj(
                nowLevel.StageDatas[nowStage].WaveEnemyList[Random.Range(0, nowLevel.StageDatas[nowStage].WaveEnemyList.Count)].ToString(),
                t =>
                {
                    if (t)
                    {
                        t.transform.position = _newPoint;
                        //t.transform.parent = ground.transform;
                        t.transform.parent = enemyList.transform;
                    }
                }, ResourceType.Enemy);

                //Debug.Log("enemypos" + _newPoint);
                enemyList.transform.parent = ground.transform;
            }
            else
            {
                Vector3 _newPoint;
                do
                {
                    _newPoint = InstantRandomPoint();
                } while (enemyPoints.Contains(_newPoint));

                enemyPoints.Add(_newPoint);
                PoolManager.GetInstance().GetObj(
                nowLevel.StageDatas[nowStage].WaveEnemyList[Random.Range(0, nowLevel.StageDatas[nowStage].WaveEnemyList.Count)].ToString(),
                t =>
                {
                    if (t)
                    {
                        t.transform.position = _newPoint;
                    }
                }, ResourceType.Enemy);
            }
            requireEnemy--;
        }
    }

    void BuffDoorCreate(GameObject ground)
    {
        PoolManager.GetInstance().GetObj(MapItemType.BuffDoors.ToString(), t =>
        {
            if (t == null) return;

            t.transform.position = new Vector3(
                ground.transform.position.x,
                ground.transform.position.y,
                ground.transform.position.z - mapSize[0] / 2);
            t.transform.parent = ground.transform;
        }, ResourceType.MapItem);
    }
    void BOSSCreate()
    {
        requireBOSS--;
        PoolManager.GetInstance().GetObj(
            nowLevel.StageDatas[nowStage].BOSSList[Random.Range(0, nowLevel.StageDatas[nowStage].BOSSList.Count)].ToString(),
            t =>
            {
                t.transform.position = new Vector3(bossPoint.x, bossPoint.y + 20f, bossPoint.z);

                t.transform.DOMove(bossPoint, 3f);
                boss = t;
            }, ResourceType.Enemy);
    }

    public void WaveIncrease()
    {
        if (requireEnemy > 0 || requireBOSS > 0) return;

        nowWave++;
        if (nowWave >= nowLevel.StageDatas[nowStage].WaveEnemyNum.Count)
        {
            nowStage++;
            if (nowStage >= nowLevel.StageDatas.Count)
            {
                //������Ϸ               
                PanelManager.Instance.Push(new VictoryPanel());
                return;
            }
            nowWave = 0;
        }
        requireEnemy = nowLevel.StageDatas[nowStage].WaveEnemyNum[nowWave];
        if (nowWave == nowLevel.StageDatas[nowStage].WaveEnemyNum.Count - 1) requireBOSS = nowLevel.StageDatas[nowStage].BOSSList.Count;
        if (nowLevel.StageDatas[nowStage].isSpecial != isChange)
        {
            PrepareChangeStage();
        }
    }

    Vector3 InstantRandomPoint(GameObject parent)
    {
        float[] _znumbers = { -5, -3, 3, 5 };
        float[] _xnumbers = { -2.5f, 0, 2.5f };
        float _randNum;

        _randNum = _znumbers[Random.Range(0, _znumbers.Length)];
        _randNum = Mathf.Clamp(_randNum, -mapSize[1] / 2, mapSize[1] / 2);
        float _z = parent.transform.position.z + _randNum;

        _randNum = _xnumbers[Random.Range(_xnumbers.Length / 2 - 1, _xnumbers.Length / 2 + 2)];
        _randNum = Mathf.Clamp(_randNum, -mapSize[1] / 2, mapSize[1] / 2);
        float _x = parent.transform.position.x + _randNum;

        //float _x = parent.transform.position.x + Random.Range(-mapSize[1] / 2, mapSize[1] / 2);
        //float _z = parent.transform.position.z + Random.Range(-mapSize[0] / 2, mapSize[0] / 2);

        return new Vector3(_x, 0, _z);
    }

    Vector3 InstantRandomPoint()
    {
        float _x = specialPoint.x + Random.Range(-2, 2);
        float _z = specialPoint.z + Random.Range(-2, 2);

        return new Vector3(_x, 0, _z);
    }
}