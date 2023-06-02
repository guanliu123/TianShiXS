using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UIFrameWork;

public class list : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public GameObject GoDrag;
    public RolePanel _panel;

    /// <summary>
    /// 所有的数据
    /// </summary>
    private List<CircleDragData> dragDataList = new List<CircleDragData>();

    /// <summary>
    /// 角色数据和goList下标严格对应
    /// </summary>   
    public List<CharacterDatas> playerList = new List<CharacterDatas>();
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
        public Vector3 defaultScaleValue;

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool isActive;

        /// <summary>
        /// 默认位置
        /// </summary>
        public Vector3 defaultPos;

        public CircleDragData(int index, Vector3 defaultScaleValue, bool isActive, Vector3 defaultPos)
        {
            this.index = index;
            this.defaultScaleValue = defaultScaleValue;
            this.isActive = isActive;
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
            CircleDragData data = new CircleDragData(i, goList[i].transform.localScale, goList[i].gameObject.activeSelf, goList[i].transform.localPosition);
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
            DragRight();
        }
        else
        {
            DragLeft();
        }
        dragOffset = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void DragLeft()
    {
        CurSelectIndex++;
        Debug.Log("left" + CurSelectIndex);
        DragEndEffect();
    }

    public void DragRight()
    {
        CurSelectIndex--;
        Debug.Log("right" + CurSelectIndex);
        DragEndEffect();
    }

    private void updatePanel()
    {
        
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
            bool gotoActive = data.isActive;
            //var gotoColor = new Color(gotoScale, gotoScale, gotoScale);

            go.transform.DOLocalMove(gotoPos, 0.3f);
            go.transform.DOScale(gotoScale, 0.3f);
            //System.Threading.Thread.Sleep(300);
            go.SetActive(gotoActive);
        }
    }

    private void SetGotoActive()
    {

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
