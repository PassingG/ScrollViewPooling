using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wise.ScrollViewPooling
{
    // Main
    [RequireComponent(typeof(ScrollRect))]
    public partial class ScrollViewPooling : MonoBehaviour
    {
        #region ----- [ Settings ] -----
        private const float SCROLL_SPEED = 5000f;

        public bool isReverse = false;

        [Space(10)]
        public GameObject[] Prefabs;

        [Header("UpdateIcon Prefab"), Space(10)]
        public GameObject updateIconPrefab;

        // Vertical Option
        [Header("How many item will pooling"), Space(10)]
        public int PoolingCount = 3;

        [Header("Grid Size"), Space(10)]
        public Vector2 GridSize = Vector2.zero;

        [Header("Paddings"), Space(10)]
        public int TopPadding = 10;
        public int BottomPadding = 10;

        // Horizontal Option
        [Header("Paddings"), Space(10)]
        public int LeftPadding = 10;
        public int RightPadding = 10;
        public int ItemSpace = 2;

        [Header("Pulling Available"), Space(10)]
        public bool IsPullTop = true;
        public bool IsPullBottom = true;

        [Header("Pulling Available"), Space(10)]
        public bool IsPullLeft = true;
        public bool IsPullRight = true;

        [Header("Update Comment"), Space(10)]
        public string PullingComment = "";

        [Header("Offsets"), Space(10)]
        public float PullOffset = 1.5f;
        public float UpdateIconOffest = 85f;
        #endregion

        #region ----- [ Variable ] -----
        [HideInInspector] private Image StartPullIcon;
        [HideInInspector] private Image EndPullIcon;

        [SerializeField] public EScrollType ScrollType;

        [HideInInspector] public ScrollRect scrollRect { get; private set; }

        private RectTransform content;
        private Rect container;

        private RectTransform[] itemRectCache;
        private List<List<GameObject>> itemObjectCache;
        public GameObject[] GetGameObjects(int prefabIndex) => itemObjectCache[prefabIndex].ToArray();

        public Dictionary<int, float> itemPositionCache { get; private set; }

        private int curPrefabIndex = 0;

        private int itemCountCache;
        private float itemHeightCache;
        private float itemWidthCache;

        private DateTime lastMoveTime;
        private int previousScrollIndex;
        private float previousScrollPos;
        private int saveStepPosition = -1;

        private bool isCanLoadUp;
        private bool isCanLoadDown;
        private bool isCanLoadLeft;
        private bool isCanLoadRight;

        #endregion

        #region ----- [ Events] -----
        /// <summary>
		/// Callback on item update. Return dataIndex, objectIndex.
		/// </summary>
        public Action<int, int> OnUpdateItem = delegate { };

        /// <summary>
        /// Callback on list is pulled. Return which way pulled direction.
        /// </summary>
        public Action<EDirection> OnPullItme = delegate { };
        #endregion

        #region ----- [ Function ] -----

        private void Awake()
        {
            container = GetComponent<RectTransform>().rect;
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.content;
            itemPositionCache = new Dictionary<int, float>();

            itemObjectCache = new List<List<GameObject>>();
            for (int i = 0; i < Prefabs.Length; i++)
            {
                itemObjectCache.Add(new List<GameObject>());
            }
            // scrollRect.onValueChanged.AddListener(OnScrollChange);
            // CreateIcons();
        }

        public bool Initialize(int itemCount, int curPrefabIndex)
        {
            for (int i = 0; i < itemObjectCache?[this.curPrefabIndex]?.Count; i++)
            {
                itemObjectCache[this.curPrefabIndex][i].SetActive(false);
            }
            this.curPrefabIndex = curPrefabIndex;
            itemCountCache = itemCount;

            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    return InitVertical();
                case EScrollType.Horizontal:
                    return InitHorizontal();
            }
            return false;
        }

        public void StopScollViewMoving()
        {
            scrollRect.velocity = Vector2.zero;
        }

        public int GetCurrentIndex(EScrollType scrollType)
        {
            int result = 0;

            switch (scrollType)
            {
                case EScrollType.Vertical:
                    {
                        float topPos = content.anchoredPosition.y - ItemSpace;
                        float perItemSize = itemHeightCache + ItemSpace;
                        float calculateCurPos = topPos - (perItemSize * (float)(PoolingCount - 1));
                        calculateCurPos = Mathf.Max(calculateCurPos, 0f);
                        result = (int)(calculateCurPos / perItemSize);
                    }
                    break;
                case EScrollType.Horizontal:
                    {
                        float leftPos = -content.anchoredPosition.x - LeftPadding;
                        float perItemSize = itemWidthCache + ItemSpace;
                        float calculateCurPos = leftPos - (perItemSize * (float)(PoolingCount - 1));
                        calculateCurPos = Mathf.Max(calculateCurPos, 0f);
                        result = (int)(calculateCurPos / perItemSize);
                    }
                    break;
            }

            return result;
        }

        public Vector2 GetTargetItemPos(EScrollType scrollType, float itemIndex, float offset = 4.5f)
        {
            Vector2 result = Vector2.zero;
            var contentRectTrfTmp = scrollRect.content;
            float calculate = 0f;

            switch (scrollType)
            {
                case EScrollType.Vertical:
                    {
                        float index = isReverse ? itemIndex : (itemCountCache - itemIndex);
                        calculate = -(TopPadding + (itemHeightCache + ItemSpace) * index);
                        result = new Vector2(contentRectTrfTmp.anchoredPosition.x, Mathf.Clamp(contentRectTrfTmp.rect.height + calculate, 0f, contentRectTrfTmp.rect.height));
                    }
                    break;
                case EScrollType.Horizontal:
                    {
                        float index = isReverse ? (itemCountCache - itemIndex) : itemIndex;
                        calculate = LeftPadding + (itemWidthCache + ItemSpace) * index;
                        result = new Vector2(Mathf.Clamp(-calculate, -contentRectTrfTmp.rect.width, 0f), contentRectTrfTmp.anchoredPosition.y);
                    }
                    break;
            }

            return result;
        }

        public void InitView()
        {
            bool showed = false;

            int itemLength = itemObjectCache[curPrefabIndex].Count;

            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    {
                        int curIndex = GetCurrentIndex(ScrollType);

                        for (int i = 0; i < itemLength; i++)
                        {
                            showed = i < itemCountCache;
                            itemObjectCache[curPrefabIndex][i].SetActive(showed);
                            if (i + 1 > itemCountCache)
                            {
                                continue;
                            }

                            int calculateIndex = curIndex + i;

                            int newIndex = calculateIndex % itemLength;

                            if (calculateIndex < itemCountCache && calculateIndex >= 0)
                            {
                                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                                pos.y = itemPositionCache[calculateIndex];
                                itemRectCache[newIndex].anchoredPosition = pos;

                                Vector2 size = itemRectCache[newIndex].sizeDelta;
                                size.y = itemHeightCache;
                                itemRectCache[newIndex].sizeDelta = size;

                                if (isReverse)
                                {
                                    OnUpdateItem(itemCountCache - calculateIndex - 1, newIndex);
                                }
                                else
                                {
                                    OnUpdateItem(calculateIndex, newIndex);
                                }
                            }
                        }
                    }
                    break;
                case EScrollType.Horizontal:
                    {
                        int curIndex = GetCurrentIndex(ScrollType);

                        for (int i = 0; i < itemLength; i++)
                        {
                            showed = i < itemCountCache;
                            itemObjectCache[curPrefabIndex][i].SetActive(showed);
                            if (i + 1 > itemCountCache)
                            {
                                continue;
                            }

                            int calculateIndex = curIndex + i;

                            int newIndex = calculateIndex % itemLength;

                            if (calculateIndex < itemCountCache && calculateIndex >= 0)
                            {
                                Vector2 pos = itemRectCache[newIndex].anchoredPosition;
                                pos.x = itemPositionCache[calculateIndex];
                                itemRectCache[newIndex].anchoredPosition = pos;

                                Vector2 size = itemRectCache[newIndex].sizeDelta;
                                size.x = itemWidthCache;
                                itemRectCache[newIndex].sizeDelta = size;

                                if (isReverse)
                                {
                                    OnUpdateItem(itemCountCache - calculateIndex - 1, newIndex);
                                }
                                else
                                {
                                    OnUpdateItem(calculateIndex, newIndex);
                                }
                            }
                        }
                    }
                    break;
            }
        }


        private void Update()
        {
            if (itemCountCache == 0)
            {
                return;
            }

            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    UpdateVertical();
                    break;
                case EScrollType.Horizontal:
                    UpdateHorizontal();
                    break;
            }
        }
        partial void UpdateVertical();
        partial void UpdateHorizontal();
        #endregion
    }
}