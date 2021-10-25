using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.ScrollViewPooling;

public enum TestEnum
{
    Normal,
    Good,
    Best,
}
public class TestScript : MonoBehaviour
{
    public static TestScript Instance;

    [SerializeField] ScrollViewPooling pooling;
    
    [Range(1,1000)]
    public int itemCount = 100;

    public float width = 200f;

    public Sprite[] images;

    private int[,] datas;

    private List<Item> items = new List<Item>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        InitScrollviewPooling();
    }

    public void InitScrollviewPooling()
    {
        int EnumLength = Enum.GetValues(typeof(TestEnum)).Length;

        for (int i = 0; i < Enum.GetValues(typeof(TestEnum)).Length; i++)
        {
            pooling.Initialize(itemCount, i);

        }

        pooling.OnUpdateItem += UpdateItem;
    }

    private void UpdateItem(int dataIndex, int objectIndex)
    {
        int[] dataTmp = new int[datas.GetLength(1)];
        for (int i = 0; i < datas.GetLength(1); i++)
        {
            dataTmp[i] = datas[dataIndex,i];
        }
    }
}
