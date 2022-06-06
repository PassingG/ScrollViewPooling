using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wise.ScrollViewPooling
{
    // Update
    public partial class ScrollViewPooling
    {
        partial void UpdateVertical()
        {

            if (!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            scrollRect.velocity = new Vector2(scrollRect.velocity.x, Mathf.Clamp(scrollRect.velocity.y, -SCROLL_SPEED, SCROLL_SPEED));

            int curIndex = GetCurrentIndex(EScrollType.Vertical);

            if (previousScrollIndex == curIndex)
            {
                return;
            }
            if (curIndex > previousScrollIndex)
            {
                if (curIndex - previousScrollIndex > 1)
                {
                    curIndex = previousScrollIndex + 1;
                }

                int itemLength = itemObjectCache[curPrefabIndex].Count;
                int newIndex = curIndex % itemLength;
                newIndex--;

                if (newIndex < 0)
                {
                    newIndex = itemLength - 1;
                }

                int index = curIndex + itemLength - 1;
                if (index < itemCountCache && index >= 0)
                {
                    Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                    pos.y = itemPositionCache[index];
                    itemRectCache[newIndex].anchoredPosition = pos;

                    Vector2 size = itemRectCache[newIndex].sizeDelta;
                    size.y = itemHeightCache;
                    itemRectCache[newIndex].sizeDelta = size;

                    if (isReverse)
                    {
                        OnUpdateItem(itemCountCache - index - 1, newIndex);
                    }
                    else
                    {
                        OnUpdateItem(index, newIndex);
                    }
                }
            }
            else
            {
                if (previousScrollIndex - curIndex > 1)
                {
                    curIndex = previousScrollIndex - 1;
                }

                int itemLength = itemObjectCache[curPrefabIndex].Count;
                int newIndex = curIndex % itemLength;

                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                pos.y = itemPositionCache[curIndex];
                itemRectCache[newIndex].anchoredPosition = pos;

                Vector2 size = itemRectCache[newIndex].sizeDelta;
                size.y = itemHeightCache;

                itemRectCache[newIndex].sizeDelta = size;

                if (isReverse)
                {
                    OnUpdateItem(itemCountCache - curIndex - 1, newIndex);
                }
                else
                {
                    OnUpdateItem(curIndex, newIndex);
                }
            }
            previousScrollIndex = curIndex;
        }
        partial void UpdateHorizontal()
        {

            if (!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            scrollRect.velocity = new Vector2(Mathf.Clamp(scrollRect.velocity.x, -SCROLL_SPEED, SCROLL_SPEED), scrollRect.velocity.y);

            int curIndex = GetCurrentIndex(EScrollType.Horizontal);

            if (previousScrollIndex == curIndex)
            {
                return;
            }
            if (curIndex > previousScrollIndex)
            {
                if (curIndex - previousScrollIndex > 1)
                {
                    curIndex = previousScrollIndex + 1;
                }

                int itemLength = itemObjectCache[curPrefabIndex].Count;
                int newIndex = curIndex % itemLength;
                newIndex--;

                if (newIndex < 0)
                {
                    newIndex = itemLength - 1;
                }

                int index = curIndex + itemLength - 1;
                if (index < itemCountCache)
                {
                    Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                    pos.x = itemPositionCache[index];
                    itemRectCache[newIndex].anchoredPosition = pos;

                    Vector2 size = itemRectCache[newIndex].sizeDelta;
                    size.x = itemWidthCache;
                    itemRectCache[newIndex].sizeDelta = size;

                    if (isReverse)
                    {
                        OnUpdateItem(itemCountCache - index - 1, newIndex);
                    }
                    else
                    {
                        OnUpdateItem(index, newIndex);
                    }
                }
            }
            else
            {
                if (previousScrollIndex - curIndex > 1)
                {
                    curIndex = previousScrollIndex - 1;
                }

                int itemLength = itemObjectCache[curPrefabIndex].Count;
                int newIndex = curIndex % itemLength;

                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                pos.x = itemPositionCache[curIndex];
                itemRectCache[newIndex].anchoredPosition = pos;

                Vector2 size = itemRectCache[newIndex].sizeDelta;
                size.x = itemWidthCache;

                itemRectCache[newIndex].sizeDelta = size;

                if (isReverse)
                {
                    OnUpdateItem(itemCountCache - curIndex - 1, newIndex);
                }
                else
                {
                    OnUpdateItem(curIndex, newIndex);
                }
            }
            previousScrollIndex = curIndex;
        }
    }
}