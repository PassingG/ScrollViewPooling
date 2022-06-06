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
                itemPositionCache[i] = -(TopPadding + result + (i * ItemSpace));
                result += itemHeightCache;
            }
            result += TopPadding + BottomPadding + (itemCountCache == 0 ? 0 : ((itemCountCache - 1) * ItemSpace));

            return result;
        }
        public float CalculateContentSizesHorizontal()
        {
            itemPositionCache.Clear();

            float result = 0f;
            for (int i = 0; i < itemCountCache; i++)
            {
                itemPositionCache[i] = LeftPadding + result + (i * ItemSpace);
                result += itemWidthCache;
            }
            result += LeftPadding + RightPadding + (itemCountCache == 0 ? 0 : ((itemCountCache - 1) * ItemSpace));
            return result;
        }
    }
}
