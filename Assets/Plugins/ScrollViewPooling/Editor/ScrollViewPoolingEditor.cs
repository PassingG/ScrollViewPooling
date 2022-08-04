#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;

namespace Wise.ScrollViewPooling
{
    [CustomEditor(typeof(ScrollViewPooling))]
    public class ScrollViewPoolingEditor : Editor
    {
        private ReorderableList list;

        private ScrollViewPooling _target;

        private SerializedObject _object;

        private SerializedProperty _prefabs;

        private SerializedProperty _isReverse;

        private SerializedProperty _poolingCount;
        private SerializedProperty _gridXSize;
        private SerializedProperty _gridYSize;
        private SerializedProperty _itemHeight;
        private SerializedProperty _itemWidth;
        private SerializedProperty _topPadding;
        private SerializedProperty _bottomPadding;
        private SerializedProperty _itemSpace;

        private SerializedProperty _startPullIcon;
        private SerializedProperty _endPullIcon;

        private SerializedProperty _isPullTop;
        private SerializedProperty _isPullBottom;

        private SerializedProperty _leftPadding;
        private SerializedProperty _rightPadding;

        private SerializedProperty _isPullLeft;
        private SerializedProperty _isPullRight;

        /// <summary>
        /// Coefficient when labels should action
        /// </summary>
        private SerializedProperty _pullOffset;

        private SerializedProperty _updateIconOffest;

        [MenuItem("GameObject/UI/ScrollViewPooling")]
        private static void Create()
        {
            GameObject thisObject = PrefabUtility.InstantiatePrefab(Resources.Load("ScrollViewPooling")) as GameObject;
            PrefabUtility.UnpackPrefabInstance(thisObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);

            Place(thisObject);
        }

        private static void Place(GameObject gameObject)
        {
            gameObject.transform.parent = Selection.activeGameObject.transform;

            SceneView lastView = SceneView.lastActiveSceneView;
            gameObject.transform.position = lastView ? lastView.pivot : Vector3.zero;
            gameObject.transform.localScale = Vector3.one;
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(gameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(gameObject);

            Undo.RegisterCreatedObjectUndo(gameObject, $"Create Object: {gameObject.name}");
            Selection.activeGameObject = gameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        /// <summary>
        /// Init data
        /// </summary>
        private void OnEnable()
        {
            _object = new SerializedObject(target);

            list = new ReorderableList(_object, _object.FindProperty("Prefabs"), true, true, true, true);
            // Element 가 그려질 때 Callback 
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                // 현재 그려질 요소 
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                // 위쪽 패딩 
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("eTriggerType"), GUIContent.none);
                EditorGUI.PropertyField(new Rect(rect.x + 100, rect.y, rect.width - 100 - 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ConditoinList"), GUIContent.none);
            };


            _target = (ScrollViewPooling)target;
            _isReverse = _object.FindProperty("isReverse");
            _prefabs = _object.FindProperty("Prefabs");
            _poolingCount = _object.FindProperty("PoolingCount");
            _gridXSize = _object.FindProperty("GridXSize");
            _gridYSize = _object.FindProperty("GridYSize");
            _itemWidth = _object.FindProperty("itemWidth");
            _itemHeight = _object.FindProperty("itemHeight");
            _topPadding = _object.FindProperty("TopPadding");
            _bottomPadding = _object.FindProperty("BottomPadding");
            _itemSpace = _object.FindProperty("ItemSpace");
            _isPullTop = _object.FindProperty("IsPullTop");
            _isPullBottom = _object.FindProperty("IsPullBottom");
            _leftPadding = _object.FindProperty("LeftPadding");
            _rightPadding = _object.FindProperty("RightPadding");
            _isPullLeft = _object.FindProperty("IsPullLeft");
            _isPullRight = _object.FindProperty("IsPullRight");
            _pullOffset = _object.FindProperty("PullOffset");
            _updateIconOffest = _object.FindProperty("UpdateIconOffest");
            _startPullIcon = _object.FindProperty("StartPullIcon");
            _endPullIcon = _object.FindProperty("EndPullIcon");
        }

        /// <summary>
        /// Draw inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(20f);
            EditorGUILayout.LabelField("[ Pooling ]");

            _object.Update();
            EditorGUI.BeginChangeCheck();
            _target.ScrollType = (EScrollType)GUILayout.Toolbar((int)_target.ScrollType, new string[] { "Vertical", "Horizontal" });

            switch (_target.ScrollType)
            {
                case EScrollType.Vertical:
                    EditorGUILayout.PropertyField(_isReverse);
                    EditorGUILayout.PropertyField(_prefabs);
                    EditorGUILayout.PropertyField(_poolingCount);
                    EditorGUILayout.PropertyField(_gridXSize);
                    EditorGUILayout.PropertyField(_itemHeight);
                    EditorGUILayout.PropertyField(_topPadding);
                    EditorGUILayout.PropertyField(_bottomPadding);
                    EditorGUILayout.PropertyField(_leftPadding);
                    EditorGUILayout.PropertyField(_rightPadding);
                    EditorGUILayout.PropertyField(_itemSpace);
                    EditorGUILayout.PropertyField(_isPullTop);
                    EditorGUILayout.PropertyField(_isPullBottom);
                    EditorGUILayout.PropertyField(_pullOffset);
                    EditorGUILayout.PropertyField(_updateIconOffest);
                    EditorGUILayout.PropertyField(_startPullIcon);
                    EditorGUILayout.PropertyField(_endPullIcon);
                    break;
                case EScrollType.Horizontal:
                    EditorGUILayout.PropertyField(_isReverse);
                    EditorGUILayout.PropertyField(_prefabs);
                    EditorGUILayout.PropertyField(_poolingCount);
                    EditorGUILayout.PropertyField(_gridYSize);
                    EditorGUILayout.PropertyField(_itemWidth);
                    EditorGUILayout.PropertyField(_topPadding);
                    EditorGUILayout.PropertyField(_bottomPadding);
                    EditorGUILayout.PropertyField(_leftPadding);
                    EditorGUILayout.PropertyField(_rightPadding);
                    EditorGUILayout.PropertyField(_itemSpace);
                    EditorGUILayout.PropertyField(_isPullLeft);
                    EditorGUILayout.PropertyField(_isPullRight);
                    EditorGUILayout.PropertyField(_pullOffset);
                    EditorGUILayout.PropertyField(_updateIconOffest);
                    EditorGUILayout.PropertyField(_startPullIcon);
                    EditorGUILayout.PropertyField(_endPullIcon);
                    break;
                default:
                    break;
            }
            if (EditorGUI.EndChangeCheck())
            {
                _object.ApplyModifiedProperties();
            }
        }
    }
}
#endif