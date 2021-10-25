using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.ScrollViewPooling
{
    // Initialize
    public partial class ScrollViewPooling
    {
        // private void CreateIcons()
        // {
        //     switch (ScrollType)
        //     {
        //         case EScrollType.Vertical:
        //             CreateIconsVertical();
        //             break;
        //         case EScrollType.Horizontal:
        //             CreateIconsHorizontal();
        //             break;
        //     }
        // }
        // private void CreateIconsVertical()
        // {
        //     GameObject topIcons = Instantiate(updateIconPrefab, Vector3.zero, Quaternion.identity);
        //     topIcons.transform.SetParent(scrollRect.viewport.transform);
        //     StartPullIcon = topIcons.GetComponent<Image>();

        //     RectTransform rect = StartPullIcon.GetComponent<RectTransform>();
        //     rect.pivot = new Vector2(0.5f, 1f);
        //     rect.anchorMin = new Vector2(0f, 1f);
        //     rect.anchorMax = Vector2.one;
        //     rect.offsetMax = Vector2.zero;
        //     rect.offsetMin = new Vector2(0f, -UpdateIconOffest);
        //     rect.anchoredPosition3D = Vector3.zero;

        //     topIcons.SetActive(false);

        //     GameObject bottomIcons = Instantiate(updateIconPrefab, Vector3.zero, Quaternion.identity);
        //     bottomIcons.transform.SetParent(scrollRect.viewport.transform);
        //     EndPullIcon = bottomIcons.GetComponent<Image>();
        //     EndPullIcon.transform.position = Vector3.zero;

        //     rect = EndPullIcon.GetComponent<RectTransform>();
        //     rect.pivot = new Vector2(0.5f, 0f);
        //     rect.anchorMin = Vector2.zero;
        //     rect.anchorMax = new Vector2(1f, 0f);
        //     rect.offsetMax = new Vector2(0f, UpdateIconOffest);
        //     rect.offsetMin = Vector2.zero;
        //     rect.anchoredPosition3D = Vector3.zero;

        //     bottomIcons.SetActive(false);
        // }
        // private void CreateIconsHorizontal()
        // {
        //     GameObject leftIcons = new GameObject("LeftIcons");
        //     leftIcons.transform.SetParent(scrollRect.viewport.transform);
        //     StartPullIcon = leftIcons.AddComponent<Image>();

        //     RectTransform rect = StartPullIcon.GetComponent<RectTransform>();
        //     rect.pivot = new Vector2(0f, 0.5f);
        //     rect.anchorMin = Vector2.zero;
        //     rect.anchorMax = new Vector2(0f, 1f);
        //     rect.offsetMax = Vector2.zero;
        //     rect.offsetMin = new Vector2(-UpdateIconOffest * 2, 0f);
        //     rect.anchoredPosition3D = Vector3.zero;

        //     leftIcons.SetActive(false);

        //     GameObject rightIcons = new GameObject("RightIcons");
        //     rightIcons.transform.SetParent(scrollRect.viewport.transform);
        //     EndPullIcon = rightIcons.AddComponent<Image>();
        //     EndPullIcon.transform.position = Vector3.zero;

        //     rect = EndPullIcon.GetComponent<RectTransform>();
        //     rect.pivot = new Vector2(1f, 0.5f);
        //     rect.anchorMin = new Vector2(1f, 0f);
        //     rect.anchorMax = Vector3.one;
        //     rect.offsetMax = new Vector2(UpdateIconOffest * 2, 0f);
        //     rect.offsetMin = Vector2.zero;
        //     rect.anchoredPosition3D = Vector3.zero;

        //     rightIcons.SetActive(false);
        // }

        private bool InitVertical()
        {
            bool isMakeObject = CreateItems();
            previousScrollIndex = 0;

            float contentHeight = CalculateContentSize();
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);

            Vector2 pos = content.anchoredPosition;
            pos.y = 0f;
            content.anchoredPosition = pos;

            Vector2 size = Vector2.zero;

            for (int i = 0; i < itemObjectCache[curPrefabIndex].Count; i++)
            {
                if(i + 1 > itemCountCache)
                {
                    continue;
                }

                pos = itemRectCache[i].anchoredPosition;
                pos.y = itemPositionCache[i];
                pos.x = 0f;

                itemRectCache[i].anchoredPosition = pos;
                size = itemRectCache[i].sizeDelta;
                size.y = itemHeightCache;
                itemRectCache[i].sizeDelta = size;
            }

            return isMakeObject;
        }
        private bool InitHorizontal()
        {
            bool isMakeObject = CreateItems();
            previousScrollIndex = 0;

            float contentWidth = CalculateContentSize();
            content.sizeDelta = new Vector2(contentWidth, content.sizeDelta.y);

            Vector2 pos = content.anchoredPosition;
            pos.x = 0f;
            content.anchoredPosition = pos;

            Vector2 size = Vector2.zero;

            for (int i = 0; i < itemObjectCache[curPrefabIndex].Count; i++)
            {
                if(i + 1 > itemCountCache)
                {
                    continue;
                }

                pos = itemRectCache[i].anchoredPosition;
                pos.x = itemPositionCache[i];
                pos.y = 0f;

                itemRectCache[i].anchoredPosition = pos;
                size = itemRectCache[i].sizeDelta;
                size.x = itemWidthCache;
                itemRectCache[i].sizeDelta = size;
            }

            return isMakeObject;
        }

        private bool CreateItems()
        {
            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    return CreateItemsVertical();
                case EScrollType.Horizontal:
                    return CreateItemsHorizontal();
            }

            return false;
        }
        private bool CreateItemsVertical()
        {
            bool isCreateItem = false;

            GameObject obejctTmp;
            RectTransform rectTmp;

            itemHeightCache = Prefabs[curPrefabIndex].GetComponent<RectTransform>().rect.height;

            int fillCount = Mathf.RoundToInt(container.height / itemHeightCache) + (PoolingCount*2);

            int itemCount = itemObjectCache[curPrefabIndex].Count;
            // Init item rect
            for (int i = itemCount; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefabs[curPrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0.5f, 1f);
                rectTmp.anchorMin = new Vector2(0f,1f);
                rectTmp.anchorMax = Vector2.one;
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                itemObjectCache[curPrefabIndex].Add(obejctTmp);
                obejctTmp.SetActive(false);
                isCreateItem = true;
            }

            itemCount = itemObjectCache[curPrefabIndex].Count;
            itemRectCache = new RectTransform[itemCount];
            for (int i = 0; i < itemObjectCache[curPrefabIndex].Count; i++)
            {
                itemRectCache[i] = itemObjectCache[curPrefabIndex][i].GetComponent<RectTransform>();
            }

            return isCreateItem;
        }
        private bool CreateItemsHorizontal()
        {
            bool isCreateItem = false;
            GameObject obejctTmp;
            RectTransform rectTmp;

            itemWidthCache = Prefabs[curPrefabIndex].GetComponent<RectTransform>().rect.width;

            int fillCount = Mathf.RoundToInt(container.width / itemWidthCache) + (PoolingCount*2);

            int itemCount = itemObjectCache[curPrefabIndex].Count;

            // Init item rect
            for (int i = itemCount; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefabs[curPrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0f, 0.5f);
                rectTmp.anchorMin = Vector2.zero;
                rectTmp.anchorMax = new Vector2(0f, 1f);
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                itemObjectCache[curPrefabIndex].Add(obejctTmp);
                obejctTmp.SetActive(false);
                isCreateItem = true;
            }

            itemCount = itemObjectCache[curPrefabIndex].Count;
            itemRectCache = new RectTransform[itemCount];
            for (int i = 0; i < itemCount; i++)
            {
                itemRectCache[i] = itemObjectCache[curPrefabIndex][i].GetComponent<RectTransform>();
            }

            return isCreateItem;
        }
    }
}
