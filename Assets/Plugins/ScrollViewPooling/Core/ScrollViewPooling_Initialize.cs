using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wise.ScrollViewPooling
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

            float size = CalculateContentSize();
            previousScrollIndex = 0;

            content.pivot = new Vector2(0.5f, 1f);
            content.anchorMin = new Vector2(0f, 1f);
            content.anchorMax = Vector2.one;
            content.offsetMax = Vector2.zero;
            content.offsetMin = Vector2.zero;
            content.sizeDelta = new Vector2(content.sizeDelta.x, size);

            Vector2 pos = content.anchoredPosition;
            pos.y = 0f;
            content.anchoredPosition = pos;

            return isMakeObject;
        }
        private bool InitHorizontal()
        {
            bool isMakeObject = CreateItems();

            float size = CalculateContentSize();

            previousScrollIndex = 0;

            content.pivot = new Vector2(0f, 0.5f);
            content.anchorMin = Vector2.zero;
            content.anchorMax = new Vector2(0f, 1f);
            content.offsetMax = Vector2.zero;
            content.offsetMin = Vector2.zero;
            content.sizeDelta = new Vector2(size, content.sizeDelta.y);

            Vector2 pos = content.anchoredPosition;
            pos.x = 0f;
            content.anchoredPosition = pos;

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

            itemSizeCache.x = (viewport.rect.width - LeftPadding - RightPadding - (ItemSpace.x * (GridXSize - 1))) / GridXSize;
            itemSizeCache.y = itemHeight;

            int fillCount = GridXSize * (Mathf.RoundToInt(viewport.rect.height / itemSizeCache.y) + (PoolingCount * 2));

            int itemCount = itemObjectCache[curPrefabIndex].Count;
            // Init item rect
            for (int i = itemCount; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefabs[curPrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0f, 1f);
                rectTmp.anchorMin = new Vector2(0f, 1f);
                rectTmp.anchorMax = new Vector2(0f, 1f);
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                rectTmp.sizeDelta = itemSizeCache;
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

            itemSizeCache.x = itemWidth;
            itemSizeCache.y = (viewport.rect.height - TopPadding - BottomPadding - (ItemSpace.y * (GridYSize - 1))) / GridYSize;

            int fillCount = GridYSize * (Mathf.RoundToInt(viewport.rect.width / itemSizeCache.x) + (PoolingCount * 2));

            int itemCount = itemObjectCache[curPrefabIndex].Count;

            // Init item rect
            for (int i = itemCount; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefabs[curPrefabIndex], Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0f, 1f);
                rectTmp.anchorMin = new Vector2(0f, 1f);
                rectTmp.anchorMax = new Vector2(0f, 1f);
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                rectTmp.sizeDelta = itemSizeCache;
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
