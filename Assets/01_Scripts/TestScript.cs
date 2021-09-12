using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiseUtility.ScrollViewPooling;

public class TestScript : MonoBehaviour
{
    public static TestScript Instance;

    [SerializeField] ScrollViewPooling pooling;
    
    [Range(1,200)]
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
        RandomDatas(itemCount);

        pooling.OnUpdateItem += UpdateItem;
        GameObject[] objectTmp = pooling.Initialize(itemCount, width);

        for (int i = 0; i < objectTmp.Length; i++)
        {
            items.Add(objectTmp[i].GetComponent<Item>());
        }
        pooling.InitView();
    }

    private void UpdateItem(int dataIndex, int objectIndex)
    {
        int[] dataTmp = new int[datas.GetLength(1)];
        for (int i = 0; i < datas.GetLength(1); i++)
        {
            dataTmp[i] = datas[dataIndex,i];
        }

        items[objectIndex].SetView(dataTmp);
    }
    
    private void RandomDatas(int itemCount)
    {
        datas = new int[itemCount,images.Length];

        for (int i = 0; i < itemCount; i++)
        {
            for (int j = 0; j < images.Length; j++)
            {
                datas[i,j] = Random.Range(0,images.Length);
            }
        }
    }
}
