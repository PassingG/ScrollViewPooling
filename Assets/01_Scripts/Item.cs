using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Text itemIndex;
    [SerializeField] private Image itemIcon;

    [SerializeField] private Sprite[] items;

    public void SetView(int index, int itemGrade)
    {
        itemIndex.text = itemIndex.ToString();  
        itemIcon.sprite = items[itemGrade];
    }
}
