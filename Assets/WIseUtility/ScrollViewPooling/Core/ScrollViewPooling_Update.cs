using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WiseUtility.ScrollViewPooling
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

            float topPos = content.anchoredPosition.y - ItemSpace;
            if(topPos < 0f)
            {
                return;
            }
            if(!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            float itemPos = Mathf.Abs(itemPositionCache[previousScrollIndex]) + itemHeightCache * 3;
            int curIndex = topPos > itemPos ? previousScrollIndex + 1 : previousScrollIndex - 1;
            int border = (int)(itemPositionCache[0] + itemHeightCache);
            int step = (int)((topPos + (topPos / 1.25f)) / border);
            if(step != saveStepPosition)
            {
                saveStepPosition = step;
            }
            else
            {
                return;
            }

            if(curIndex < 0 || previousScrollIndex == curIndex || scrollRect.velocity.y == 0f)
            {
                return;
            }
            if(curIndex > previousScrollIndex)
            {
                if (curIndex - previousScrollIndex > 1)
                {
                    curIndex = previousScrollIndex + 1;
                }
                
                int itemLength = itemObjectCache.Length;
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
                    pos.y = itemPositionCache[index];
                    itemRectCache[newIndex].anchoredPosition = pos;

                    Vector2 size = itemRectCache[newIndex].sizeDelta;
                    size.y = itemHeightCache;
                    itemRectCache[newIndex].sizeDelta = size;

                    OnUpdateItem(index, newIndex);
                }
            }
            else
            {
                if (previousScrollIndex - curIndex > 1)
                {
                    curIndex = previousScrollIndex - 1;
                }

                int itemLength = itemObjectCache.Length;
                int newIndex = curIndex % itemLength;

                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                pos.y = itemPositionCache[curIndex];
                itemRectCache[newIndex].anchoredPosition = pos;

                Vector2 size = itemRectCache[newIndex].sizeDelta;
                size.y = itemHeightCache;

                itemRectCache[newIndex].sizeDelta = size;

                OnUpdateItem(curIndex, newIndex);
            }
            previousScrollIndex = curIndex;
        }
        partial void UpdateHorizontal()
        {
            if (itemCountCache == 0)
            {
                return;
            }

            float leftPos = -content.anchoredPosition.x - ItemSpace;
            if (leftPos < 0f)
            {
                return;
            }
            if (!itemPositionCache.ContainsKey(previousScrollIndex))
            {
                return;
            }

            float itemPos = Mathf.Abs(itemPositionCache[previousScrollIndex]) + itemWidthCache;
            int curIndex = leftPos > itemPos ? previousScrollIndex + 1 : previousScrollIndex - 1;
            int border = (int)(itemPositionCache[0] + itemWidthCache);
            int step = (int)((leftPos + (leftPos / 1.25f)) / border);
            if (step != saveStepPosition)
            {
                saveStepPosition = step;
            }
            else
            {
                return;
            }

            if (curIndex < 0 || previousScrollIndex == curIndex || scrollRect.velocity.x == 0f)
            {
                return;
            }
            if (curIndex > previousScrollIndex)
            {
                if (curIndex - previousScrollIndex > 1)
                {
                    curIndex = previousScrollIndex + 1;
                }

                int itemLength = itemObjectCache.Length;
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
                    size.x = itemHeightCache;
                    itemRectCache[newIndex].sizeDelta = size;

                    OnUpdateItem(index, newIndex);
                }
            }
            else
            {
                if (previousScrollIndex - curIndex > 1)
                {
                    curIndex = previousScrollIndex - 1;
                }

                int newIndex = curIndex % itemObjectCache.Length;
                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                pos.y = itemPositionCache[curIndex];
                itemRectCache[newIndex].anchoredPosition = pos;

                Vector2 size = itemRectCache[newIndex].sizeDelta;
                size.y = itemHeightCache;

                itemRectCache[newIndex].sizeDelta = size;

                OnUpdateItem(curIndex, newIndex);
            }
            previousScrollIndex = curIndex;
        }
    }
}