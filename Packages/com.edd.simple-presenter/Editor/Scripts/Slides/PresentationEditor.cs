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
            EditorGUILayout.LabelField("Load Slides", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical();

            var isGuiEnabled = GUI.enabled;
            var currentSlide = presentation.CurrentSlide;

            EditorGUI.indentLevel++;
            foreach (var slide in presentation.Slides)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.enabled = false;
                EditorGUILayout.ObjectField(slide, typeof(Slide), false);
                GUI.enabled = isGuiEnabled;

                GUI.enabled = currentSlide != slide;
                if (GUILayout.Button("Load"))
                {
                    presentation.LoadSlide(slide);
                }

                GUI.enabled = isGuiEnabled;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}
