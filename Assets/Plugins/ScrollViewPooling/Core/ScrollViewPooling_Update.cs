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

        private void UpdateItem(int newIndex, int index)
        {
            itemRectCache[newIndex].anchoredPosition = itemPositionCache[index];
            itemRectCache[newIndex].sizeDelta = itemSizeCache;

            if (isReverse)
            {
                OnUpdateItem(itemCountCache - index - 1, newIndex);
            }
            else
            {
                OnUpdateItem(index, newIndex);
            }
        }

        partial void UpdateVertical()
        {
            int curIndex = GetCurrentIndex(EScrollType.Vertical);

            if (itemPositionCache.ContainsKey(curIndex).Equals(false))
            {
                return;
            }

            scrollRect.velocity = new Vector2(scrollRect.velocity.x, Mathf.Clamp(scrollRect.velocity.y, -SCROLL_SPEED, SCROLL_SPEED));

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
                    UpdateItem(newIndex, index);
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

                UpdateItem(newIndex, curIndex);
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
                    UpdateItem(newIndex, index);
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

                UpdateItem(newIndex, curIndex);
            }
            previousScrollIndex = curIndex;
        }
    }
}