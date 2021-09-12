using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WiseUtility.ScrollViewPooling
{
    // Initialize
    public partial class ScrollViewPooling
    {
        private void CreateIcons()
        {
            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    CreateIconsVertical();
                    break;
                case EScrollType.Horizontal:
                    CreateIconsHorizontal();
                    break;
            }
        }
        private void CreateIconsVertical()
        {
            GameObject topIcons = Instantiate(updateIconPrefab, Vector3.zero, Quaternion.identity);
            topIcons.transform.SetParent(scrollRect.viewport.transform);
            StartPullIcon = topIcons.GetComponent<Image>();

            RectTransform rect = StartPullIcon.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 1f);
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = Vector2.one;
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = new Vector2(0f, -UpdateIconOffest);
            rect.anchoredPosition3D = Vector3.zero;

            topIcons.SetActive(false);

            GameObject bottomIcons = Instantiate(updateIconPrefab, Vector3.zero, Quaternion.identity);
            bottomIcons.transform.SetParent(scrollRect.viewport.transform);
            EndPullIcon = bottomIcons.GetComponent<Image>();
            EndPullIcon.transform.position = Vector3.zero;

            rect = EndPullIcon.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(1f, 0f);
            rect.offsetMax = new Vector2(0f, UpdateIconOffest);
            rect.offsetMin = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;

            bottomIcons.SetActive(false);
        }
        private void CreateIconsHorizontal()
        {
            GameObject leftIcons = new GameObject("LeftIcons");
            leftIcons.transform.SetParent(scrollRect.viewport.transform);
            StartPullIcon = leftIcons.AddComponent<Image>();

            RectTransform rect = StartPullIcon.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0f, 0.5f);
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(0f, 1f);
            rect.offsetMax = Vector2.zero;
            rect.offsetMin = new Vector2(-UpdateIconOffest * 2, 0f);
            rect.anchoredPosition3D = Vector3.zero;

            leftIcons.SetActive(false);

            GameObject rightIcons = new GameObject("RightIcons");
            rightIcons.transform.SetParent(scrollRect.viewport.transform);
            EndPullIcon = rightIcons.AddComponent<Image>();
            EndPullIcon.transform.position = Vector3.zero;

            rect = EndPullIcon.GetComponent<RectTransform>();
            rect.pivot = new Vector2(1f, 0.5f);
            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = Vector3.one;
            rect.offsetMax = new Vector2(UpdateIconOffest * 2, 0f);
            rect.offsetMin = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;

            rightIcons.SetActive(false);
        }

        partial void InitVertical(float height)
        {
            itemHeightCache = height;
            CreateItems();
            previousScrollIndex = 0;

            float contentHeight = CalculateContentSize();
            content.sizeDelta = new Vector2(content.sizeDelta.x, contentHeight);

            Vector2 pos = content.anchoredPosition;
            pos.y = 0f;
            content.anchoredPosition = pos;

            Vector2 size = Vector2.zero;

            bool showed = false;
            for (int i = 0; i < itemObjectCache.Length; i++)
            {
                showed = i < itemCountCache;
                itemObjectCache[i].SetActive(showed);
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
        }
        partial void InitHorizontal(float width)
        {
            itemWidthCache = width;
        }

        private void CreateItems()
        {
            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    CreateItemsVertical();
                    break;
                case EScrollType.Horizontal:
                    CreateItemsHorizontal();
                    break;
            }
        }
        private void CreateItemsVertical()
        {
            if(itemObjectCache != null)
            {
                return;
            }

            GameObject obejctTmp;
            RectTransform rectTmp;

            int fillCount = Mathf.RoundToInt(container.height / itemHeightCache) + (PoolingCount*2);
            Debug.Log(fillCount);
            itemObjectCache = new GameObject[fillCount];

            // Init item rect
            for (int i = 0; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0.5f, 1f);
                rectTmp.anchorMin = new Vector2(0f,1f);
                rectTmp.anchorMax = Vector2.one;
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                itemObjectCache[i] = obejctTmp;
            }

            itemRectCache = new RectTransform[itemObjectCache.Length];
            for (int i = 0; i < itemObjectCache.Length; i++)
            {
                itemRectCache[i] = itemObjectCache[i].gameObject.GetComponent<RectTransform>();
            }
        }
        private void CreateItemsHorizontal()
        {
            if (itemObjectCache != null)
            {
                return;
            }

            GameObject obejctTmp;
            RectTransform rectTmp;

            int fillCount = Mathf.RoundToInt(container.width / itemWidthCache) + (PoolingCount*2);
            itemObjectCache = new GameObject[fillCount];

            // Init item rect
            for (int i = 0; i < fillCount; i++)
            {
                obejctTmp = Instantiate(Prefab, Vector3.zero, Quaternion.identity) as GameObject;
                obejctTmp.transform.SetParent(content);
                obejctTmp.transform.localScale = Vector3.one;
                obejctTmp.transform.localPosition = Vector3.zero;
                rectTmp = obejctTmp.GetComponent<RectTransform>();
                rectTmp.pivot = new Vector2(0.5f, 1f);
                rectTmp.anchorMin = new Vector2(0f, 1f);
                rectTmp.anchorMax = Vector2.one;
                rectTmp.offsetMax = Vector2.zero;
                rectTmp.offsetMin = Vector2.zero;
                itemObjectCache[i] = obejctTmp;
            }

            itemRectCache = new RectTransform[itemObjectCache.Length];
            for (int i = 0; i < itemObjectCache.Length; i++)
            {
                itemRectCache[i] = itemObjectCache[i].gameObject.GetComponent<RectTransform>();
            }
        }
    }
}
