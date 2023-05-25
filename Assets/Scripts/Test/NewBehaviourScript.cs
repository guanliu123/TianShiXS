using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class NewBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// 碰撞器（需要挂载）
    /// </summary>
    public GameObject GoDrag;

    /// <summary>
    /// 所有的数据
    /// </summary>
    private List<CircleDragData> dragDataList = new List<CircleDragData>();

    /// <summary>
    /// 所有的物体
    /// </summary>
    private List<GameObject> goList = new List<GameObject>();

    /// <summary>
    /// 当前选择
    /// </summary>
    private int curSelectIndex = 0;

    public int CurSelectIndex
    {
        get
        {
            return curSelectIndex;
        }

        set
        {
            //在这里限制选择的索引在List范围内
            if (value > dragDataList.Count - 1)
            {
                value = 0;
            }
            if (value < 0)
            {
                value = dragDataList.Count;
            }
            curSelectIndex = value;
        }
    }



    private int defaultDepth;


    /// <summary>
    /// 拖拽偏移量
    /// </summary>
    private Vector2 dragOffset;




    private void Awake()
    {
        
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset += eventData.position;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(dragOffset);

        if (dragOffset.x >= 0)
        {
            DragRight();
        }
        else
        {
            DragLeft();
        }

        dragOffset = Vector2.zero;
    }

    private void DragLeft()
    {
        CurSelectIndex++;

        DragEndEffect();
    }

    private void DragRight()
    {
        CurSelectIndex--;

        DragEndEffect();
    }

    /// <summary>
    /// 拖拽后的效果（根据CurSelectIndex来实现）
    /// </summary>
    private void DragEndEffect()
    {
        for (int i = 0; i < goList.Count; i++)
        {
            var go = goList[i];
            //var panel = go.GetComponent<UIPanel>();
            var sp = go.transform.Find("SpCharacter");


            var data = dragDataList[GetCorrectIndex(i)];
            var gotoPos = data.defaultPos;
            var gotoScale = data.defaultScaleValue;
            var gotoDepth = data.panelDepth;
            var gotoColor = new Color(gotoScale, gotoScale, gotoScale);


            var tweenTime = 0.3f;
            //TweenPosition.Begin(go, tweenTime, gotoPos);
            //TweenScale.Begin(go, tweenTime, gotoScale * Vector3.one);
            //TweenColor.Begin(go, tweenTime, gotoColor);
            //设置panel的正确层级，这里使用了一个比较高的6000层级
            //panel.depth = 6000 + gotoDepth;
        }
    }

    private int GetCorrectIndex(int i)
    {
        var value = i - CurSelectIndex;

        if (value < 0)
        {
            value += dragDataList.Count;
        }

        return value;
    }



    public void Init(GameObject go0, GameObject go1, GameObject go2, GameObject go3, GameObject go4)
    {
        CircleDragData data0 = new CircleDragData(0, 1f, 5, go0.transform.localPosition);
        CircleDragData data1 = new CircleDragData(1, 0.85f, 4, go1.transform.localPosition);
        CircleDragData data2 = new CircleDragData(2, 0.55f, 3, go2.transform.localPosition);
        CircleDragData data3 = new CircleDragData(3, 0.55f, 3, go3.transform.localPosition);
        CircleDragData data4 = new CircleDragData(4, 0.85f, 4, go4.transform.localPosition);

        dragDataList.Add(data0);
        dragDataList.Add(data1);
        dragDataList.Add(data2);
        dragDataList.Add(data3);
        dragDataList.Add(data4);

        goList.Add(go0);
        goList.Add(go1);
        goList.Add(go2);
        goList.Add(go3);
        goList.Add(go4);
    }

}


public class CircleDragData
{
    /// <summary>
    /// 索引
    /// </summary>
    public int index;

    /// <summary>
    /// 默认缩放
    /// </summary>
    public float defaultScaleValue;

    /// <summary>
    /// UI层级
    /// </summary>
    public int panelDepth;

    /// <summary>
    /// 默认位置
    /// </summary>
    public Vector3 defaultPos;

    public CircleDragData(int index, float defaultScaleValue, int panelDepth, Vector3 defaultPos)
    {
        this.index = index;
        this.defaultScaleValue = defaultScaleValue;
        this.panelDepth = panelDepth;
        this.defaultPos = defaultPos;
    }
}
