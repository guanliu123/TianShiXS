using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Item
    {
        public string name;
        public int weight;

        public Item(string name, int weight)
        {
            this.name = name;
            this.weight = weight;
        }
    }
public class LotterySystem : MonoBehaviour
{
    private int[] weight;

    private List<Item> items;
    void Start()
    {
        items = new List<Item>();

        items.Add(new Item("1", 50));
        items.Add(new Item("2", 25));
        items.Add(new Item("3", 15));
        items.Add(new Item("4", 10));

        weight = new int[items.Count];
        for (int i=0;i<items.Count;i++)
        {
            weight[i] = items[i].weight;
        }

        //ExtractItems();
    }



    public void ExtractItems()
    {
        Debug.Log(GetRandPersonalityType(weight));
    }

    /// <summary>
    /// 概率随机
    /// </summary>
    /// <param name="array">权重数组</param>
    /// <param name="_total">权重总和（默认100）</param>
    /// <returns>返回数组下标</returns>
    private int GetRandPersonalityType(int[] array, int _total = 100)
    {
        int rand = Random.Range(1, _total + 1);
        int tmp = 0;

        for (int i = 0; i < array.Length; i++)
        {
            tmp += array[i];
            if (rand < tmp)
            {
                return i;
            }
        }
        return 0;
    }

}
