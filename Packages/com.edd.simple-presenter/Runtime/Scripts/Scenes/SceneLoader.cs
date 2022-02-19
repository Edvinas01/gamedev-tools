using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SimplePresenter.Scenes
{
    /// <summary>
    /// Manages scene loading and unloading.
    /// </summary>
    public sealed class SceneLoader : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Transition handle used to transition (e.g., smoothly fade) between scenes")]
        [SerializeField]
        private BaseSceneTransition sceneTransition;

        [Tooltip("Primary persistent scene")]
        [SerializeField]
        private SceneReference setupSceneReference;

        [Header("Events")]
        [SerializeField]
        private UnityEvent<Scene> onSceneActivated;

        #endregion

        #region Private Fields

        private bool isLoadingScene;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

#if UNITY_EDITOR
        private IEnumerator Start()
        {
            // Activate all scenes "below" the setup scene to simulate the load order you'd
            // experience in an actual build. The setup scene always loads first and activates
            // first, then the rest. The first scene should always be the setup scene as seen in
            // SetupSceneLoader.OpenSceneWithSetupScene. But only in editor.
            var activeScene = SceneManager.GetActiveScene();

            for (var sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var scene = SceneManager.GetSceneAt(sceneIndex);
                if (scene.buildIndex == activeScene.buildIndex)
                {
                    continue;
                }

                yield return new WaitUntil(() => scene.isLoaded);
                SceneManager.SetActiveScene(scene);
            }

            yield return FadeInScene();
        }
#else
        private void Start()
        {
            // Assuming that the SceneLoader is placed in the setup scene, it should always load
            // the next scene right away when in an actual build.
            LoadNextScene();
        }
#endif

        #endregion

        #region Public Methods

        /// <summary>
        /// Load the next scene (increment build index by 1).
        /// </summary>
        public void LoadNextScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            var nextIndex = activeScene.buildIndex + 1;

            StartLoadScene(nextIndex);
        }

        /// <summary>
        /// Load the provided scene.
        /// </summary>
        public void LoadScene(SceneReference sceneReference)
        {
            StartLoadScene(sceneReference.SceneBuildIndex);
        }

        /// <summary>
        /// Restart currently active scene.
        /// </summary>
        public void RestartScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            StartLoadScene(activeScene.buildIndex);
        }


        /// <summary>
        /// Exit the game into the editor or the OS.
        /// </summary>
        public void ExitGame()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        #endregion

        private void AddListeners()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private void RemoveListeners()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        #region Private Methods

        private void OnActiveSceneChanged(Scene prev, Scene next)
        {
            if (next.buildIndex != setupSceneReference.SceneBuildIndex)
            {
                onSceneActivated.Invoke(next);
            }
        }

        private void StartLoadScene(int sceneIndex)
        {
            if (isLoadingScene)
            {
                return;
            }

            StartCoroutine(LoadScene(sceneIndex));
        }

        private IEnumerator LoadScene(int sceneIndex)
        {
            isLoadingScene = true;

            var activeScene = SceneManager.GetActiveScene();

            // Can only unload regular scenes as the setup scene must always stay.
            if (setupSceneReference.SceneBuildIndex != activeScene.buildIndex)
            {
                yield return FadeOutScene();
                yield return SceneManager.UnloadSceneAsync(activeScene);
            }

            yield return SceneManager.LoadSceneAsync(
                sceneIndex,
                LoadSceneMode.Additive
            );

            var loadedScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            SceneManager.SetActiveScene(loadedScene);

            // Required to use light probes from the newly loaded scene.
            LightProbes.TetrahedralizeAsync();

            yield return FadeInScene();

            isLoadingScene = false;
        }

        private IEnumerator FadeInScene()
        {
            yield return sceneTransition.OnEnterScene();
        }

        private IEnumerator FadeOutScene()
        {
            yield return sceneTransition.OnExitScene();
        }

        #endregion
    }
}
