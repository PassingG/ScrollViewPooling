using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Utility.ScrollViewPooling
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
        private List<List<GameObject>> itemObjectCache;
        public GameObject[] GetGameObjects(int prefabIndex) => itemObjectCache[prefabIndex].ToArray();

        private Dictionary<int, float> itemPositionCache;

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
            content = scrollRect.content;
            itemPositionCache = new Dictionary<int, float>();

            itemObjectCache = new List<List<GameObject>>();
            for(int i=0;i<Prefabs.Length;i++)
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
        public void InitView()
        {
            bool showed = false;

            for (int i = 0; i < itemObjectCache[curPrefabIndex].Count; i++)
            {
                showed = i < itemCountCache;
                itemObjectCache[curPrefabIndex][i].SetActive(showed);
                if (i + 1 > itemCountCache)
                {
                    continue;
                }

                if(isReverse)
                {
                    OnUpdateItem(itemCountCache - i - 1, i);
                }
                else
                {
                    OnUpdateItem(i, i);
                }
            }

        }


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