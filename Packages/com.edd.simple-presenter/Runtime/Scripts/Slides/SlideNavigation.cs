using SimplePresenter.Scenes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Navigates slides and controls <see cref="Presentation"/> state.
    /// </summary>
    public sealed class SlideNavigation : MonoBehaviour
    {
        #region Editor

        [Header("Dependencies")]
        [Tooltip("Presentation state")]
        [SerializeField]
        private Presentation presentation;

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

        [Header("Events")]
        [SerializeField]
        private UnityEvent<SceneReference> onLoadScene;

        [SerializeField]
        private UnityEvent onRestart;

        [SerializeField]
        private UnityEvent onExit;

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

        private void OnLoadSlide(Slide oldSlide, Slide newSlide)
        {
            var oldScene = oldSlide.SceneReference;
            var newScene = newSlide.SceneReference;

            if (oldScene != newScene && newScene != null)
            {
                onLoadScene.Invoke(newScene);
            }
            else
            {
                // Update immediately only if the scene didn't change.
                UpdateNavigation();
            }
        }

        private void LoadPrevious()
        {
            presentation.LoadPreviousSlide();
        }

        private void LoadNext()
        {
            presentation.LoadNextSlide();
        }

        private void RestartScene()
        {
            onRestart.Invoke();
        }

        private void ExitGame()
        {
            onExit.Invoke();
        }

        #endregion
    }
}
