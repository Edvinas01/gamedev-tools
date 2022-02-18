using System.Collections.Generic;
using SimplePresenter.Editor.Utils;
using SimplePresenter.Scenes;
using Unity.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SimplePresenter.Editor.Scenes
{
    /// <summary>
    /// Settings for <see cref="SetupSceneLoader"/>.
    /// </summary>
    [EditorSettingsPath("Assets/Settings/Editor/EditorSceneSettings")]
    internal class EditorSceneSettings : BaseEditorSettings<EditorSceneSettings>
    {
        #region Editor

        [Tooltip("Scene used as a setup scene")]
        [SerializeField]
        private SceneReference setupSceneReference;

        [Tooltip("Enable setup scene loading functionality while in editor")]
        [SerializeField]
        private bool isLoadSetupScene = true;

        [ReadOnly]
        [Tooltip("Currently saved scene setups before entering play mode")]
        [SerializeField]
        private List<SceneSetup> sceneSetups = new List<SceneSetup>();

        #endregion

        #region Public Properties

        /// <summary>
        /// Build index assigned to the setup scene.
        /// </summary>
        public int SetupSceneBuildIndex
        {
            get
            {
                if (!setupSceneReference)
                {
                    setupSceneReference = EditorScriptableObjects
                        .FindOrCreateAsset<SceneReference>(
                            "Assets/ScriptableObjects/Scenes/SetupSceneReference.asset"
                        );
                }

                var buildIndex = setupSceneReference.SceneBuildIndex;

                return buildIndex;
            }
        }

        /// <summary>
        /// Should persistent scenes be loaded in the editor.
        /// </summary>
        public bool IsLoadSetupScene => isLoadSetupScene;

        /// <summary>
        /// Scene setups before entering the play mode.
        /// </summary>
        public List<SceneSetup> SceneSetups => sceneSetups;

        #endregion
    }
}
