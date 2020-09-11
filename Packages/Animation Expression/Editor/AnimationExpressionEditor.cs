using UnityEditor;
using UnityEngine;

namespace AnimationExpression.Editor
{
    [CustomEditor(typeof(Data.AnimationExpression))]
    public class AnimationExpressionEditor : UnityEditor.Editor
    {
        public class ConcreteEditor
        {
            public ConcreteEditor(Data.AnimationExpression asset, SerializedObject serializedObject)
            {
                Asset = asset;
                SerializedObject = serializedObject;
            }
            
            public Data.AnimationExpression Asset { get; private set; }
            public SerializedObject SerializedObject { get; private set; }

            public void OnGUI()
            {
                SerializedObject.Update();
            
                EditorGUILayout.PropertyField(SerializedObject.FindProperty(nameof(Asset.ObjectMaps)));
                EditorGUILayout.PropertyField(SerializedObject.FindProperty(nameof(Asset.Variables)));
            
                EditorGUILayout.LabelField("Expression");
            
                Asset.Expression = EditorGUILayout.TextArea(Asset.Expression, GUILayout.Height(120f));

                SerializedObject.ApplyModifiedProperties();
            }
        }

        private ConcreteEditor Editor;
        
        private void OnEnable()
        {
            Editor = new ConcreteEditor(target as Data.AnimationExpression, serializedObject);
        }

        private void OnDisable()
        {
            Editor = null;
        }

        public override void OnInspectorGUI()
        {
            Editor.OnGUI();
        }
    }
}