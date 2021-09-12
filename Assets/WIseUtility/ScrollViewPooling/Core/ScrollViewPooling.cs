using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WiseUtility.ScrollViewPooling
{
    // Main
    [RequireComponent(typeof(ScrollRect))]
    public partial class ScrollViewPooling : MonoBehaviour
    {
        #region ----- [ Settings ] -----
        private const float SCROLL_DURATION = 0.25f;
        private const int MIN_UPDATE_TIME = 500;
        private const float SCROLL_SPEED = 50f;

        [Header("Item Prefab"), Space(10)]
        public GameObject Prefab;

        [Header("UpdateIcon Prefab"), Space(10)]
        public GameObject updateIconPrefab;

        // Vertical Option
        [Header("How many item will pooling"), Space(10)]
        public int PoolingCount = 2;

        [Header("Paddings"),Space(10)]
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

        [HideInInspector] private ScrollRect scrollRect;

        private RectTransform content;
        private Rect container;

        private RectTransform[] itemRectCache;
        private GameObject[] itemObjectCache;

        private Dictionary<int, float> itemPositionCache;
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
        public Action<int, int> OnUpdateItem = delegate {};

        /// <summary>
        /// Callback on list is pulled. Return which way pulled direction.
        /// </summary>
        public Action<EDirection> OnPullItme = delegate {};
        #endregion

        #region ----- [ Function ] -----

        private void Awake()
        {
            container = GetComponent<RectTransform>().rect;
            scrollRect = GetComponent<ScrollRect>();
            content = scrollRect.viewport.transform.GetChild(0).GetComponent<RectTransform>();
            itemPositionCache = new Dictionary<int, float>();
            scrollRect.onValueChanged.AddListener(OnScrollChange);
            CreateIcons();
        }

        public GameObject[] Initialize(int itemCount, float size)
        {
            itemCountCache = itemCount;

            switch (ScrollType)
            {
                case EScrollType.Vertical:
                    InitVertical(size);
                    break;
                case EScrollType.Horizontal:
                    InitHorizontal(size);
                    break;
            }

            return itemObjectCache;
        }
        public void InitView()
        {
            for (int i = 0; i < itemObjectCache.Length; i++)
            {
                OnUpdateItem(i,i);
            }
        }
        partial void InitVertical(float height);
        partial void InitHorizontal(float width);


        private void Update()
        {
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