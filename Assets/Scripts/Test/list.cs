using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class list : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// 所有的数据
    /// </summary>
    private List<CircleDragData> dragDataList = new List<CircleDragData>();

    /// <summary>
    /// 所有的物体
    /// </summary>
    public List<GameObject> goList = new List<GameObject>();

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
                value = dragDataList.Count - 1;
            }
            curSelectIndex = value;
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

    private Vector2 dragOffset;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        for(int i=0;i<goList.Count;i++)
        {
            CircleDragData data = new CircleDragData(0, 1f, 5, goList[i].transform.localPosition);
            dragDataList.Add(data);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        

        if (eventData.position.x > dragOffset.x)
        {
            DragLeft();
        }
        else
        {
            DragRight();
        }

        dragOffset = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    private void DragLeft()
    {
        CurSelectIndex++;
        Debug.Log("left" + CurSelectIndex);
        DragEndEffect();
    }

    private void DragRight()
    {
        CurSelectIndex--;
        Debug.Log("right" + CurSelectIndex);
        DragEndEffect();
    }

    private void DragEndEffect()
    {
        //Debug.Log(CurSelectIndex);
        for (int i = 0; i < goList.Count; i++)
        {
            var go = goList[i];
            var data = dragDataList[GetCorrectIndex(i)];
            var gotoPos = data.defaultPos;
            var gotoScale = data.defaultScaleValue;
            var gotoDepth = data.panelDepth;
            var gotoColor = new Color(gotoScale, gotoScale, gotoScale);

            var tweenTime = 0.3f;


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
}
