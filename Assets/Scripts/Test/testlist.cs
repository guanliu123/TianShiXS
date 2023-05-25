using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// 圆弧列表 实例为圆心
/// </summary>
public class testlist : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <summary>
    /// 子物体列表
    /// </summary>
    List<RectTransform> childList = new List<RectTransform>();

    /// <summary>
    /// 当前显示物体
    /// </summary>
    [SerializeField]
    List<RectTransform> showchildlist = new List<RectTransform>();
    /// <summary>
    /// 未显示物体
    /// </summary>
    List<RectTransform> noshowchildlist = new List<RectTransform>();

    /// <summary>
    /// 右侧待显示物体
    /// </summary>
    Stack<RectTransform> rightchildlist = new Stack<RectTransform>();
    /// <summary>
    /// 左侧待显示物体
    /// </summary>
    Stack<RectTransform> leftchildlist = new Stack<RectTransform>();
    /// <summary>
    /// 排列位置
    /// </summary>
    List<Vector2> poslist = new List<Vector2>();
    /// <summary>
    /// 记录手指触摸位置
    /// </summary>
    Vector2 previousPos;
    /// <summary>
    /// 左侧隐藏物体坐标
    /// </summary>
    public Vector2 leftmask;

    public Vector2 rightmask;

    /// <summary>
    /// 显示数量
    /// </summary>
    [SerializeField]
    int shownumber = 3;
    /// <summary>
    /// 园半径
    /// </summary>
    public float radius = 500f;
    /// <summary>
    /// 滑动时间
    /// </summary>
    public float slidingtime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        leftmask = (transform as RectTransform).anchoredPosition + GetPosXYByAngle(180 / (shownumber - 1) * -1);
        rightmask = (transform as RectTransform).anchoredPosition + GetPosXYByAngle(180 / (shownumber - 1) * shownumber);

        //获取所有物体
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform rect = transform.GetChild(i) as RectTransform;

            if (rect != null)
            {
                if (i < shownumber)
                {
                    showchildlist.Add(rect);
                    //根据角度和显示数量计算当前物体位置
                    Vector2 pos = (transform as RectTransform).anchoredPosition + GetPosXYByAngle(180 / (shownumber - 1) * i);
                    rect.anchoredPosition = pos;
                    //保存位置 滑动时使用
                    poslist.Add(pos);

                }
                else
                {
                    //超出显示数量的物体 保存到右侧待显示列表中等待滑动显示
                    rect.gameObject.SetActive(false);
                    rect.anchoredPosition = rightmask;
                    noshowchildlist.Add(rect);
                }
            }
        }
    }
    /// <summary>
    /// 根据角度拿到相对于圆心的XY坐标
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public Vector2 GetPosXYByAngle(float angle)
    {
        if (angle > 90)
        {
            angle = 180 - angle;
            Vector2 pos = new Vector2();
            pos.y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            pos.x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            return pos;
        }
        else
        {
            Vector2 pos = new Vector2();
            pos.y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            pos.x = -Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            return pos;
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        previousPos = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //手指向右滑动 列表显示左侧待显示物体
        if (eventData.position.x > previousPos.x)
        {
            LeftTurn();
        }
        else
        {
            RightTurn();
        }
    }

    /// <summary>
    /// 显示右侧物体
    /// </summary>
    void RightTurn()
    {
        //右侧是否有待显示物体
        if (noshowchildlist.Count == 0 && rightchildlist.Count == 0)
            return;

        //当前显示第一个物体加入左侧列表
        AddLeftMask(showchildlist[0]);
        //移除当前显示的第一个物体
        showchildlist.RemoveAt(0);

        if (noshowchildlist.Count != 0)
        {
            showchildlist.Add(noshowchildlist[0]);
            noshowchildlist.RemoveAt(0);
        }
        else
        {
            showchildlist.Add(rightchildlist.Pop());
        }
        //更新按钮位置
        for (int i = 0; i < showchildlist.Count; i++)
        {
            showchildlist[i].gameObject.SetActive(true);
            showchildlist[i].DOAnchorPos(poslist[i], slidingtime);
        }



    }
    /// <summary>
    /// 显示左侧物体
    /// </summary>
    void LeftTurn()
    {
        //左侧是否有待显示物体
        if (leftchildlist.Count == 0)
            return;

        //当前显示最后一个物体加入右侧列表
        AddRightMask(showchildlist[showchildlist.Count - 1]);
        //移除当前显示的第一个物体
        showchildlist.RemoveAt(showchildlist.Count - 1);

        showchildlist.Insert(0, leftchildlist.Pop());

        //更新按钮位置
        for (int i = 0; i < showchildlist.Count; i++)
        {
            showchildlist[i].gameObject.SetActive(true);
            showchildlist[i].DOAnchorPos(poslist[i], slidingtime);
        }
    }

    void AddLeftMask(RectTransform item)
    {
        item.DOAnchorPos(leftmask, slidingtime);
        item.gameObject.SetActive(false);
        leftchildlist.Push(item);
    }
    void AddRightMask(RectTransform item)
    {
        item.DOAnchorPos(rightmask, slidingtime);
        item.gameObject.SetActive(false);
        rightchildlist.Push(item);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}

