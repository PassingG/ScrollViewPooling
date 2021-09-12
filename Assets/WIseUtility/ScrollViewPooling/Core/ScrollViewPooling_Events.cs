using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WiseUtility.ScrollViewPooling
{
    // Events
    public partial class ScrollViewPooling
    {
        private void OnScrollChange(Vector2 vector)
        {
            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    ScrollChangeVertical(vector);
                    break;
                case EScrollType.Horizontal:
                    break;
            }
        }
        private void ScrollChangeVertical(Vector2 vector)
        {
            isCanLoadUp = false;
            isCanLoadDown = false;
            if (itemObjectCache == null)
            {
                return;
            }
            float y = 0f;
            float z = 0f;
            bool isScrollable = (scrollRect.verticalNormalizedPosition != 1f && scrollRect.verticalNormalizedPosition != 0f);
            y = content.anchoredPosition.y;
            if (isScrollable)
            {
                if (scrollRect.verticalNormalizedPosition < 0f)
                {
                    z = y - previousScrollPos;
                }
                else
                {
                    previousScrollPos = y;
                }
            }
            else
            {
                z = y;
            }

            if (y < -UpdateIconOffest && IsPullTop)
            {
                StartPullIcon.gameObject.SetActive(true);
                StartPullIcon.fillAmount = Mathf.Clamp((y + UpdateIconOffest) / ((-UpdateIconOffest * PullOffset) + UpdateIconOffest), 0f, 1f);

                if (y < -UpdateIconOffest * PullOffset)
                {
                    isCanLoadUp = true;
                }
            }
            else
            {
                StartPullIcon.gameObject.SetActive(false);
            }

            if (z > UpdateIconOffest && IsPullBottom)
            {
                EndPullIcon.gameObject.SetActive(true);
                EndPullIcon.fillAmount = Mathf.Clamp((z - UpdateIconOffest) / ((UpdateIconOffest * PullOffset) - UpdateIconOffest), 0f, 1f);

                if (z > UpdateIconOffest * PullOffset)
                {
                    isCanLoadDown = true;
                }
            }
            else
            {
                EndPullIcon.gameObject.SetActive(false);
            }
        }

        private void ScrollChangeHorizontal(Vector2 vector)
        {
            isCanLoadLeft = false;
            isCanLoadRight = false;

            if (itemObjectCache == null)
            {
                return;
            }
            float x = 0f;
            float z = 0f;
            bool isScrollable = (scrollRect.horizontalNormalizedPosition != 1f && scrollRect.horizontalNormalizedPosition != 0f);
            x = content.anchoredPosition.x;
            if (isScrollable)
            {
                if (scrollRect.horizontalNormalizedPosition > 1f)
                {
                    z = x - previousScrollPos;
                }
                else
                {
                    previousScrollPos = x;
                }
            }
            else
            {
                z = x;
            }
            if (x > UpdateIconOffest && IsPullLeft)
            {
                StartPullIcon.gameObject.SetActive(true);
                StartPullIcon.fillAmount = Mathf.Clamp((x - UpdateIconOffest) / ((UpdateIconOffest * PullOffset) - UpdateIconOffest), 0f, 1f);
                if (x > UpdateIconOffest * PullOffset)
                {
                    isCanLoadLeft = true;
                }
            }
            else
            {
                StartPullIcon.gameObject.SetActive(false);
            }

            if (z < -UpdateIconOffest && IsPullRight)
            {
                EndPullIcon.gameObject.SetActive(true);
                EndPullIcon.fillAmount = Mathf.Clamp((z + UpdateIconOffest) / ((-UpdateIconOffest * PullOffset) + UpdateIconOffest), 0f, 1f);

                if (z < -UpdateIconOffest * PullOffset)
                {
                    isCanLoadRight = true;
                }
            }
            else
            {
                EndPullIcon.gameObject.SetActive(false);
            }
        }
    }
}