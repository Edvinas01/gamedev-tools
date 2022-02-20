using SimplePresenter.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Navigates slides and controls <see cref="Presentation"/> state.
    /// </summary>
    public sealed class SlideNavigation : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Presentation state")]
        [SerializeField]
        private Presentation presentation;

        [SerializeField]
        private SceneLoader sceneLoader;

        [Tooltip("Button to load the previous slide (or scene)")]
        [SerializeField]
        private Button backButton;

        [Tooltip("Button to load the next slide (or scene)")]
        [SerializeField]
        private Button nextButton;

        [Tooltip("Button to restart the scene")]
        [SerializeField]
        private Button restartButton;

        [Tooltip("Button to exit the game")]
        [SerializeField]
        private Button exitButton;

        [Header("Navigation")]
        [SerializeField]
        private KeyCode previousSlideKey = KeyCode.LeftArrow;

        [SerializeField]
        private KeyCode nextSlideKey = KeyCode.RightArrow;

        [SerializeField]
        private KeyCode restartKey = KeyCode.R;

        [SerializeField]
        private KeyCode exitKey = KeyCode.Escape;

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

        private void Start()
        {
            UpdateNavigation();
        }

        private void Update()
        {
            UpdateKeyNavigation();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update navigation button enabled and disabled state based on <see cref="presentation"/>
        /// state.
        /// </summary>
        public void UpdateNavigation()
        {
            backButton.interactable = presentation.IsContainsPreviousSlide;
            nextButton.interactable = presentation.IsContainsNextSlide;
        }

        #endregion

        #region Private Methods

        private void AddListeners()
        {
            presentation.OnLoadSlide += OnLoadSlide;
            backButton.onClick.AddListener(LoadPrevious);
            nextButton.onClick.AddListener(LoadNext);
            restartButton.onClick.AddListener(RestartScene);
            exitButton.onClick.AddListener(ExitGame);
        }

        private void RemoveListeners()
        {
            presentation.OnLoadSlide -= OnLoadSlide;
            backButton.onClick.RemoveListener(LoadPrevious);
            nextButton.onClick.RemoveListener(LoadNext);
            restartButton.onClick.RemoveListener(RestartScene);
            exitButton.onClick.RemoveListener(ExitGame);
        }

        private void UpdateKeyNavigation()
        {
            if (Input.GetKeyDown(previousSlideKey))
            {
                LoadPrevious();
                return;
            }

            if (Input.GetKeyDown(nextSlideKey))
            {
                LoadNext();
                return;
            }

            if (Input.GetKeyDown(restartKey))
            {
                RestartScene();
                return;
            }

            if (Input.GetKeyDown(exitKey) && presentation.IsContainsNextSlide == false)
            {
                ExitGame();
            }
        }

        private void OnLoadSlide(Slide oldSlide, Slide newSlide)
        {
            var oldScene = oldSlide.SceneReference;
            var newScene = newSlide.SceneReference;

            if (oldScene != newScene && newScene != null)
            {
                sceneLoader.LoadScene(newScene);
            }
            else
            {
                // Update immediately only if the scene didn't change.
                UpdateNavigation();
            }
        }

        private void LoadPrevious()
        {
            if (sceneLoader.IsLoadingScene)
            {
                return;
            }

            presentation.LoadPreviousSlide();
        }

        private void LoadNext()
        {
            if (sceneLoader.IsLoadingScene)
            {
                return;
            }

            presentation.LoadNextSlide();
        }

        private void RestartScene()
        {
            if (sceneLoader.IsLoadingScene)
            {
                return;
            }

            sceneLoader.RestartScene();
        }

        private void ExitGame()
        {
            sceneLoader.ExitGame();
        }

        #endregion
    }
}
