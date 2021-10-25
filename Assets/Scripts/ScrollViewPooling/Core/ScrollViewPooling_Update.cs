using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.ScrollViewPooling
{
    // Update
    public partial class ScrollViewPooling
    {
        partial void UpdateVertical()
        {
            if(itemCountCache == 0)
            {
                return;
            }
            if(scrollRect.velocity.y == 0f)
            {
                return;
            }

            float topPos = content.anchoredPosition.y - ItemSpace;

            if(!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            scrollRect.velocity = new Vector2(scrollRect.velocity.x,Mathf.Clamp(scrollRect.velocity.y, -SCROLL_SPEED, SCROLL_SPEED));

            float perItemSize = itemHeightCache + ItemSpace;
            float calculateCurPos = topPos - (perItemSize * (float)(PoolingCount-1));
            calculateCurPos = Mathf.Max(calculateCurPos, 0f);
            int curIndex = (int)(calculateCurPos / perItemSize);

            if(previousScrollIndex == curIndex)
            {
                return;
            }
            if(curIndex > previousScrollIndex)
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
            if (itemCountCache == 0)
            {
                return;
            }
            if(scrollRect.velocity.x == 0f)
            {
                return;
            }

            float leftPos = -content.anchoredPosition.x - LeftPadding;

            if (!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            scrollRect.velocity = new Vector2(Mathf.Clamp(scrollRect.velocity.x, -SCROLL_SPEED, SCROLL_SPEED),scrollRect.velocity.y);

            float perItemSize = itemWidthCache + ItemSpace;
            float calculateCurPos = leftPos - (perItemSize * (float)(PoolingCount-1));
            calculateCurPos = Mathf.Max(calculateCurPos, 0f);
            int curIndex = (int)(calculateCurPos / perItemSize);

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