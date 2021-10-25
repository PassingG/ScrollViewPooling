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

    [SerializeField] private GameObject verticalRect;
    [SerializeField] private GameObject horizontalRect;
    
    [Range(1,1000)]
    public int itemCount = 100;

    private int[,] datas;
    private Item[][] itemObjects;

    private int curCategoryIndex;

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
        MakeData();
        InitScrollviewPooling();

        pooling.Initialize(itemCount, curCategoryIndex);
        pooling.InitView();
    }

    private void MakeData()
    {
        int prefabLength = pooling.Prefabs.Length;

        datas = new int[prefabLength,itemCount];
        for(int i=0;i<prefabLength;i++)
        {
            for(int j=0;j<itemCount;j++)
            {
                datas[i,j] = UnityEngine.Random.Range(0,100);
            }
        }
    }

    public void InitScrollviewPooling()
    {
        curCategoryIndex = 0;

        int EnumLength = Enum.GetValues(typeof(TestEnum)).Length;
        itemObjects = new Item[EnumLength][];

        for (int i = 0; i < EnumLength; i++)
        {
            pooling.Initialize(itemCount, i);

            GetScrollViewObject(i);
        }

        pooling.OnUpdateItem += UpdateItem;
    }

    private void GetScrollViewObject(int i)
    {
        GameObject[] objectTmp = pooling.GetGameObjects(i);
        int objectLength = objectTmp.Length;

        itemObjects[i] = new Item[objectLength];
        for (int j = 0; j < objectLength; j++)
        {
            itemObjects[i][j] = objectTmp[j].GetComponent<Item>();
        }
    }

    private void UpdateItem(int dataIndex, int objectIndex)
    {
        itemObjects[curCategoryIndex][objectIndex].SetView(dataIndex+1, datas[curCategoryIndex,dataIndex]);
    }

    public void SelectCategory(int index)
    {
        curCategoryIndex = index;

        // 오브젝트 풀링 갯수가 늘어났는지 체크하여 새로 오브젝트 받아와야함
        bool isChange = pooling.Initialize(itemCount, curCategoryIndex);

        if (isChange)
        {
            GameObject[] objectTmp = pooling.GetGameObjects(0);
            int objectLength = objectTmp.Length;

            GetScrollViewObject(curCategoryIndex);
        }

        pooling.InitView();
    }
}
