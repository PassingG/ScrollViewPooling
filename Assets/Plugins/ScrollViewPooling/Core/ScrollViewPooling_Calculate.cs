using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wise.ScrollViewPooling
{
    // Calculate
    public partial class ScrollViewPooling
    {
        public float CalculateContentSize()
        {
            return ScrollType == EScrollType.Vertical ? CalculateContentSizesVertical() : CalculateContentSizesHorizontal();
        }

        public float CalculateContentSizesVertical()
        {
            itemPositionCache.Clear();

            float result = 0f;
            for (int i = 0; i < itemCountCache; i++)
            {
                int newIndex = i % GridXSize;
                itemPositionCache[i] =
                new Vector2(LeftPadding + ((itemSizeCache.x + ItemSpace.x) * newIndex),
                -(TopPadding + result));

                if (newIndex == GridXSize - 1)
                {
                    result += itemSizeCache.y + ItemSpace.y;
                }
            }
            result += TopPadding + BottomPadding + itemSizeCache.y;

            return result;
        }
        public float CalculateContentSizesHorizontal()
        {
            itemPositionCache.Clear();

            float result = 0f;
            for (int i = 0; i < itemCountCache; i++)
            {
                int newIndex = i % GridYSize;

                itemPositionCache[i] =
                new Vector2(LeftPadding + result,
                -(TopPadding + (itemSizeCache.y + ItemSpace.y) * newIndex));

                if (newIndex == GridYSize - 1)
                {
                    result += itemSizeCache.x + ItemSpace.x;
                }
            }
            result += LeftPadding + RightPadding + itemSizeCache.x;
            return result;
        }
    }
}
