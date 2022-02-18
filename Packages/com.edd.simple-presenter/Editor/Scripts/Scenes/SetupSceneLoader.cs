using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace SimplePresenter.Editor.Scenes
{
    /// <summary>
    /// Injects SetupScene in Edit mode and after exiting PlayMode.
    /// </summary>
    [InitializeOnLoad]
    internal static class SetupSceneLoader
    {
        #region Private Init Methods

        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            if (!EditorSceneSettings.Instance.IsLoadSetupScene)
            {
                return false;
            }

            var sceneAsset = EditorUtility.InstanceIDToObject(instanceID) as SceneAsset;
            if (sceneAsset == null)
            {
                return false;
            }

            var scenePath = AssetDatabase.GetAssetPath(sceneAsset);
            if (IsSetupScene(scenePath))
            {
                return false;
            }

            OpenSceneWithSetupScene(scenePath);

            return true;
        }

        static SetupSceneLoader()
        {
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        #endregion

        #region Private Methods

        private static bool IsSetupScene(string scenePath)
        {
            var buildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
            return buildIndex == EditorSceneSettings.Instance.SetupSceneBuildIndex;
        }

        private static void OpenSceneWithSetupScene(string scenePath)
        {
            var setupScenePath = SceneUtility
                .GetScenePathByBuildIndex(EditorSceneSettings.Instance.SetupSceneBuildIndex);

            // Open two scenes at once, the setup scene and the target scene "below" it.
            EditorSceneManager.RestoreSceneManagerSetup(new[]
            {
                new SceneSetup
                {
                    isActive = false,
                    isLoaded = true,
                    path = setupScenePath
                },
                new SceneSetup
                {
                    isActive = true,
                    isLoaded = true,
                    path = scenePath
                }
            });
        }

        private static void OnPlaymodeStateChanged(PlayModeStateChange change)
        {
            if (!EditorSceneSettings.Instance.IsLoadSetupScene)
            {
                return;
            }

            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (change)
            {
                case PlayModeStateChange.ExitingEditMode:

                    // The setup scene is activated before entering play mode to ensure that it
                    // fires its lifecycle methods before any other scene.
                    ActivateSetupScene();
                    break;

                case PlayModeStateChange.EnteredEditMode:

                    // Upon returning from play mode, the scene setup before playing has to be
                    // restored or otherwise it will be rather annoying having to re-open
                    // everything.
                    RestoreActiveScene();
                    break;
            }
        }

        private static void ActivateSetupScene()
        {
            var setups = EditorSceneManager.GetSceneManagerSetup();
            ReplaceSceneSetups(setups);

            var setupScene = SceneManager
                .GetSceneByBuildIndex(EditorSceneSettings.Instance.SetupSceneBuildIndex);

            if (setupScene.IsValid())
            {
                SceneManager.SetActiveScene(setupScene);
            }
        }

        private static void RestoreActiveScene()
        {
            var activeSceneSetup = EditorSceneSettings
                .Instance
                .SceneSetups
                .FirstOrDefault(sceneSetup => sceneSetup.isActive);

            if (activeSceneSetup == null)
            {
                return;
            }

            var activeScene = SceneManager.GetSceneByPath(activeSceneSetup.path);
            SceneManager.SetActiveScene(activeScene);
        }

        private static void ReplaceSceneSetups(IEnumerable<SceneSetup> setups)
        {
            var editorSceneSettings = EditorSceneSettings.Instance;
            var sceneSetups = editorSceneSettings.SceneSetups;

            sceneSetups.Clear();

            // The scene setups are copied as they're classes (refs) and might change at any time.
            var copiedSetups = setups.Select(CopySceneSetup);
            sceneSetups.AddRange(copiedSetups);

            editorSceneSettings.Save();
        }

        private static SceneSetup CopySceneSetup(SceneSetup sceneSetup)
        {
            return new SceneSetup
            {
                isActive = sceneSetup.isActive,
                isLoaded = sceneSetup.isLoaded,
                isSubScene = sceneSetup.isSubScene,
                path = sceneSetup.path
            };
        }

        #endregion
    }
}
