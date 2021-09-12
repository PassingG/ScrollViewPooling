using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Image[] items;

    public void SetView(int[] index)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].sprite = TestScript.Instance.images[index[i]];
        }
    }
}
