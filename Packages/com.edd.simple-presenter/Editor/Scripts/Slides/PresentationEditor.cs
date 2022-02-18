using System;
using SimplePresenter.Slides;
using UnityEditor;
using UnityEngine;

namespace SimplePresenter.Editor.Slides
{
    /// <summary>
    /// Debugger <see cref="Presentation"/>.
    /// </summary>
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Presentation))]
    public class PresentationEditor : UnityEditor.Editor
    {
        #region Private Fields

        private Presentation presentation;
        private int slideIndex;

        #endregion

        #region Unity Lifecycvle

        private void OnEnable()
        {
            presentation = target as Presentation;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);

            GUI.enabled = Application.isPlaying;
            DrawNavigation();
            DrawLoadSlide();
            GUI.enabled = true;
        }

        #endregion

        #region Private Methods

        private void DrawNavigation()
        {
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Back"))
            {
                presentation.LoadPreviousSlide();
            }

            if (GUILayout.Button("Next"))
            {
                presentation.LoadNextSlide();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawLoadSlide()
        {
            EditorGUILayout.BeginHorizontal();

            slideIndex = EditorGUILayout.IntField(slideIndex);
            slideIndex = Math.Max(0, slideIndex);
            slideIndex = Math.Min(slideIndex, presentation.SlideCount - 1);

            if (GUILayout.Button("Load Slide"))
            {
                presentation.LoadSlide(slideIndex);
            }

            EditorGUILayout.EndHorizontal();
        }

        #endregion
    }
}
