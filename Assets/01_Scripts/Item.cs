using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemIndex;
    [SerializeField] private TextMeshProUGUI dataText;

    public void SetView(int index, int data)
    {
        itemIndex.text = index.ToString();
        dataText.text = $"Data:{data.ToString()}";
    }
}
