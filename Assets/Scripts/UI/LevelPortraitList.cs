using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using UIFrameWork;

/// <summary>
/// 角色立绘列表
/// </summary>
public class LevelPortraitList : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public LevelPanel _panel;

    /// <summary>
    /// 销毁列表
    /// </summary>
    private List<GameObject> destroyList = new List<GameObject>();

    // 占位符
    private Transform leftTemp;
    private Transform rightTemp;
    private Transform midTemp;

    //当前立绘对象
    private GameObject currentPortrait;
    //预载立绘对象
    private GameObject preparePortrait;
    //预销毁立绘对象
    private GameObject expirePortrait;

    /// <summary>
    /// 当前选择
    /// </summary>
    private int curSelectIndex = 0;

    //滑动起始点位
    private Vector2 beginDragPos;
    //滑动结束点位
    private Vector2 endDragPos;
    //滑动距离
    private float offfsetx;

    //private int lastIndex;

    public int CurSelectIndex
    {
        get
        {
            return curSelectIndex;
        }

        set
        {
            //在这里限制选择的索引在List范围内
            if (value > _panel.levelNum - 1)
            {
                value = _panel.levelNum - 1;
            }
            if (value < 0)
            {
                value = 0;
            }
            curSelectIndex = value;
        }
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        leftTemp = GameObject.Find("Left_Temp").transform;
        rightTemp = GameObject.Find("Right_Temp").transform;
        midTemp = GameObject.Find("Mid_Temp").transform;
        PoolManager.GetInstance().GetObj("LevelList", t =>
        {
            currentPortrait = t;
            currentPortrait.transform.parent = transform;
            currentPortrait.transform.localScale = new Vector3(1, 1, 1);
            currentPortrait.transform.localPosition = midTemp.localPosition;
            _panel.UpdateLevelList(0, currentPortrait);
        }, ResourceType.UI);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //dragOffset = eventData.position;
        //屏幕坐标转UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, eventData.position, eventData.pressEventCamera, out beginDragPos);
        //lastIndex = CurSelectIndex;
    }

    //private bool isDrag = false

    public void OnDrag(PointerEventData eventData)
    {
        //屏幕坐标转UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.transform as RectTransform, eventData.position, eventData.pressEventCamera, out endDragPos);
        offfsetx = beginDragPos.x - endDragPos.x;
        //isDrag = true;
        //Draging();
    }
    public void OnDestroy()
    {
        PoolManager.GetInstance().Clear("LevelList");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        //DragReset();
        if (offfsetx < -200)
        {
            DragRight();
        }
        else if (offfsetx > 200)
        {
            DragLeft();
        }
        beginDragPos = Vector2.zero;
    }

    /// <summary>
    /// 向左拖拽，index+1
    /// </summary>
    public void DragLeft()
    {
        if (CurSelectIndex + 1 > _panel.levelNum / 3) return;
        AudioManager.GetInstance().PlaySound("PageturnButton");
        CurSelectIndex++;
        DragEndEffect(true);
    }
    /// <summary>
    /// 向右拖拽，index-1
    /// </summary>
    public void DragRight()
    {
        if (CurSelectIndex - 1 < 0) return;
        AudioManager.GetInstance().PlaySound("PageturnButton");
        CurSelectIndex--;
        DragEndEffect(false);
    }

    public void LeftButton_Click()
    {
        DragRight();
    }
    public void RightButton_Click()
    {
        DragLeft();
    }
    /// <summary>
    /// 滑动动画
    /// </summary>
    /// <param name="flag">true向左，false向右</param>
    private void DragEndEffect(bool flag)
    {
        InsPortrait(flag, CurSelectIndex);
        preparePortrait.transform.DOLocalMove(midTemp.localPosition, 0.3f);
        if (flag)
        {
            currentPortrait.transform.DOLocalMove(leftTemp.localPosition, 0.3f);
        }
        else
        {
            currentPortrait.transform.DOLocalMove(rightTemp.localPosition, 0.3f);
        }

        expirePortrait = currentPortrait;
        destroyList.Add(expirePortrait);
        currentPortrait = preparePortrait;
    }

    /// <summary>
    /// 生成立绘
    /// </summary>
    /// <param name="flag">true在右边生成，false在左边生成</param>
    /// <param name="i"></param>
    private void InsPortrait(bool flag, int i)
    {
        PoolManager.GetInstance().GetObj("LevelList", t => {
            preparePortrait = t;
            _panel.UpdateLevelList(i, preparePortrait);
            preparePortrait.transform.SetParent(transform);
            preparePortrait.transform.localScale = new Vector3(1, 1, 1);
            if (flag)
            {
                preparePortrait.transform.localPosition = rightTemp.localPosition;
            }
            else
            {
                preparePortrait.transform.localPosition = leftTemp.localPosition;
            }
        }, ResourceType.UI);
    }
    private void FixedUpdate()
    {
        /*for (int i = 0; i < destroyList.Count; i++) 
        {
            if (destroyList[i].transform.localPosition.x <= -999 || destroyList[i].transform.localPosition.x >= 999)
            {
                PoolManager.GetInstance().PushObj("LevelList", destroyList[i]);
                destroyList.Remove(destroyList[i]);
            }
        }*/
    }
}