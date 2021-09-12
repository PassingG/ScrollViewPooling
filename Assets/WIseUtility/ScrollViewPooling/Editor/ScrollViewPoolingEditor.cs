using UnityEditor;
using UnityEngine;

namespace WiseUtility.ScrollViewPooling
{
    [CustomEditor(typeof(ScrollViewPooling))]
    public class ScrollViewPoolingEditor : Editor
    {
        private ScrollViewPooling _target;


        private SerializedObject _object;

        private SerializedProperty _prefab;
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

        /// <summary>
        /// Init data
        /// </summary>
        private void OnEnable()
        {
            _target = (ScrollViewPooling) target;
            _object = new SerializedObject(target);
            _prefab = _object.FindProperty("Prefab");
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
        }

        /// <summary>
        /// Draw inspector
        /// </summary>
        public override void OnInspectorGUI()
        {
            _object.Update();
            EditorGUI.BeginChangeCheck();
            _target.ScrollType = (EScrollType)GUILayout.Toolbar((int)_target.ScrollType, new string[] { "Vertical", "Horizontal" });
            switch (_target.ScrollType)
            {
                case EScrollType.Vertical:
                    EditorGUILayout.PropertyField(_prefab);
                    EditorGUILayout.PropertyField(_topPadding);
                    EditorGUILayout.PropertyField(_bottomPadding);
                    EditorGUILayout.PropertyField(_itemSpace);
                    EditorGUILayout.PropertyField(_isPullTop);
                    EditorGUILayout.PropertyField(_isPullBottom);
                    EditorGUILayout.PropertyField(_pullOffset);
                    EditorGUILayout.PropertyField(_updateIconOffest);
                    break;
                case EScrollType.Horizontal:
                    EditorGUILayout.PropertyField(_prefab);
                    EditorGUILayout.PropertyField(_leftPadding);
                    EditorGUILayout.PropertyField(_rightPadding);
                    EditorGUILayout.PropertyField(_itemSpace);
                    EditorGUILayout.PropertyField(_isPullLeft);
                    EditorGUILayout.PropertyField(_isPullRight);
                    EditorGUILayout.PropertyField(_pullOffset);
                    EditorGUILayout.PropertyField(_updateIconOffest);
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