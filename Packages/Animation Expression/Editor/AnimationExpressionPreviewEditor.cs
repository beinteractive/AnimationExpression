using System;
using AnimationExpression.Components;
using UnityEditor;
using UnityEngine;

namespace AnimationExpression.Editor
{
    [CustomEditor(typeof(AnimationExpressionPreview))]
    public class AnimationExpressionPreviewEditor : UnityEditor.Editor
    {
        private AnimationExpressionEditor.ConcreteEditor AssetEditor;

        private void OnEnable()
        {
            RefreshAssetEditor();
        }

        private void OnDisable()
        {
            AssetEditor = null;
        }

        private void RefreshAssetEditor()
        {
            var e = target as AnimationExpressionPreview;
            
            if (e == null || e.AnimationExpression == null)
            {
                AssetEditor = null;
            }
            else if (AssetEditor == null || AssetEditor.Asset != e.AnimationExpression)
            {
                AssetEditor = new AnimationExpressionEditor.ConcreteEditor(e.AnimationExpression, new SerializedObject(e.AnimationExpression));
            }
        }
        
        public override void OnInspectorGUI()
        {
            var e = target as AnimationExpressionPreview;
            
            serializedObject.Update();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(e.LiveTarget)));

            var assetHasChanged = false;
            
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(e.AnimationExpression)));

                if (scope.changed)
                {
                    assetHasChanged = true;
                }
            }
            
            serializedObject.ApplyModifiedProperties();

            if (assetHasChanged)
            {
                RefreshAssetEditor();
            }

            if (AssetEditor != null)
            {
                AssetEditor.OnGUI();

                if (GUILayout.Button("Reset Game Object"))
                {
                    Reset();
                }
                
                if (GUILayout.Button("Preview"))
                {
                    Reset();
                    e.Preview();
                }
            }
        }

        private void Reset()
        {
            var e = target as AnimationExpressionPreview;
            var t = e.LiveTarget;
            if (t == null)
            {
                return;
            }

            PrefabUtility.RevertPrefabInstance(t.gameObject, InteractionMode.AutomatedAction);
        }
    }
}