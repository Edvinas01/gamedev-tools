using TMPro;
using UnityEngine;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Controls slide content.
    /// </summary>
    public sealed class SlideContent : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Presentation state")]
        [SerializeField]
        private Presentation presentation;

        [Header("Text Objects")]
        [SerializeField]
        private TMP_Text titleText;

        [SerializeField]
        private TMP_Text titleContentText;

        [SerializeField]
        private TMP_Text textContentText;

        [Header("Content Transforms")]
        [SerializeField]
        private Transform placeholderContentTransform;

        [SerializeField]
        private Transform titleContentTransform;

        [SerializeField]
        private Transform textContentTransform;

        [SerializeField]
        private Transform customContentTransform;

        #endregion

        #region Private Fields

        private Transform activeContent;

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
            var slideType = currentSlide.SlideType;

            switch (slideType)
            {
                case SlideType.Empty:
                    SetupEmptySlide();
                    break;
                case SlideType.Title:
                    SetupTitleContentSlide(currentSlide);
                    break;
                case SlideType.TextContent:
                    SetupTextContentSlide(currentSlide);
                    break;
                case SlideType.CustomContent:
                    SetupCustomContentSlide(currentSlide);
                    break;
                default:
                    SetupEmptySlide();
                    break;
            }
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


        private void SetupEmptySlide()
        {
            SetupActiveContent(placeholderContentTransform);
            titleText.text = string.Empty;
        }

        private void SetupTitleContentSlide(Slide currentSlide)
        {
            SetupActiveContent(titleContentTransform);
            titleText.text = string.Empty;
            titleContentText.text = currentSlide.Title;
        }

        private void SetupTextContentSlide(Slide currentSlide)
        {
            SetupActiveContent(textContentTransform);
            titleText.text = currentSlide.Title;
            textContentText.text = currentSlide.Text;
        }

        private void SetupCustomContentSlide(Slide currentSlide)
        {
            SetupActiveContent(customContentTransform);
            titleText.text = currentSlide.Title;
            ClearActiveContent();
            AddToActiveContent(currentSlide.CustomContent);
        }

        private void SetupActiveContent(Transform newActiveContent)
        {
            if (activeContent != false)
            {
                activeContent.gameObject.SetActive(false);
            }

            activeContent = newActiveContent;
            activeContent.gameObject.SetActive(true);
        }

        private void ClearActiveContent()
        {
            for (var index = 0; index < activeContent.childCount; index++)
            {
                var child = activeContent.GetChild(index);
                Destroy(child.gameObject);
            }
        }

        private void AddToActiveContent(GameObject content)
        {
            if (content == false)
            {
                return;
            }

            Instantiate(content, activeContent);
        }

        #endregion
    }
}
