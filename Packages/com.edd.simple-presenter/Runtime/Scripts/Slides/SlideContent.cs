using TMPro;
using UnityEngine;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Controls slide content.
    /// </summary>
    public sealed class SlideContent : MonoBehaviour
    {
        #region Editor

        [Header("Dependencies")]
        [Tooltip("Presentation state")]
        [SerializeField]
        private Presentation presentation;

        [Tooltip("Current slide title")]
        [SerializeField]
        private TMP_Text titleText;

        [Tooltip("Current slide content")]
        [SerializeField]
        private Transform contentTransform;

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
            UpdateContent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Update title text and content UI based on <see cref="presentation"/> state.
        /// </summary>
        public void UpdateContent()
        {
            var currentSlide = presentation.CurrentSlide;
            UpdateTitle(currentSlide.Title);
            UpdateContent(currentSlide.Content);
        }

        #endregion

        #region Private Methods

        private void AddListeners()
        {
            presentation.OnLoadSlide += OnLoadSlide;
        }

        private void RemoveListeners()
        {
            presentation.OnLoadSlide -= OnLoadSlide;
        }

        private void OnLoadSlide(Slide oldSlide, Slide newSlide)
        {
            var oldScene = oldSlide.SceneReference;
            var newScene = newSlide.SceneReference;

            if (oldScene == newScene)
            {
                // Update immediately only if the scene didn't change.
                UpdateContent();
            }
        }

        private void UpdateTitle(string newTitle)
        {
            titleText.text = newTitle;
        }

        private void UpdateContent(GameObject newContent)
        {
            ClearContent();
            if (newContent == false)
            {
                return;
            }

            SetupContent(newContent);
        }

        private void ClearContent()
        {
            for (var index = 0; index < contentTransform.childCount; index++)
            {
                var child = contentTransform.GetChild(index);
                Destroy(child.gameObject);
            }
        }

        private void SetupContent(GameObject newContent)
        {
            Instantiate(newContent, contentTransform);
        }

        #endregion
    }
}
