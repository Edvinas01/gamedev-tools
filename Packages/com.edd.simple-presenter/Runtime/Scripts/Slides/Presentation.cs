using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Holds presentation data and mutable state (such as current slide).
    /// </summary>
    [CreateAssetMenu(
        fileName = "Presentation",
        menuName = "SimplePresenter/Presentation"
    )]
    public sealed class Presentation : ScriptableObject
    {
        #region Editor Fields

        [Tooltip("List of slides in this presentation (in order)")]
        [SerializeField]
        private List<Slide> slides;

        #endregion

        #region Private Fields

        private Slide currentSlide;

        #endregion

        #region Public Properties

        /// <summary>
        /// Number of slides in the presentation.
        /// </summary>
        public int SlideCount => slides.Count;

        /// <summary>
        /// List of available slides.
        /// </summary>
        public IReadOnlyList<Slide> Slides => slides;

        /// <summary>
        /// Currently active slide, can be <c>null</c>!
        /// </summary>
        public Slide CurrentSlide
        {
            get => currentSlide;
            private set
            {
                var oldSlide = currentSlide;
                var newSlide = value;

                currentSlide = newSlide;

                if (oldSlide != newSlide)
                {
                    OnLoadSlide?.Invoke(oldSlide, newSlide);
                }
            }
        }

        /// <summary>
        /// Does the previous slide exist?
        /// </summary>
        public bool IsContainsPreviousSlide
        {
            get
            {
                var slideIndex = GetCurrentSlideIndex();
                var isFirst = IsFirstSlide(slideIndex);

                return isFirst == false;
            }
        }

        /// <summary>
        /// Does the next slide exist?
        /// </summary>
        public bool IsContainsNextSlide
        {
            get
            {
                var slideIndex = GetCurrentSlideIndex();
                var isLast = IsLastSlide(slideIndex);

                return isLast == false;
            }
        }

        /// <summary>
        /// Invoked when a new slide is loaded.
        /// </summary>
        public event OnLoadSlideDelegate OnLoadSlide;

        public delegate void OnLoadSlideDelegate(Slide oldSlide, Slide newSlide);

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            CurrentSlide = slides.FirstOrDefault();
        }

        private void OnDisable()
        {
            CurrentSlide = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Load  given slide.
        /// </summary>
        public void LoadSlide(Slide slide)
        {
            CurrentSlide = slide;
        }

        /// <summary>
        /// Load previous if possible.
        /// </summary>
        public void LoadPreviousSlide()
        {
            var slideIndex = GetCurrentSlideIndex();
            if (IsFirstSlide(slideIndex))
            {
                return;
            }

            CurrentSlide = slides[slideIndex - 1];
        }

        /// <summary>
        /// Load next slide if possible.
        /// </summary>
        public void LoadNextSlide()
        {
            var slideIndex = GetCurrentSlideIndex();
            if (IsLastSlide(slideIndex))
            {
                return;
            }

            CurrentSlide = slides[slideIndex + 1];
        }

        #endregion

        #region Private Methods

        private static bool IsFirstSlide(int slideIndex)
        {
            return slideIndex == 0;
        }

        private bool IsLastSlide(int slideIndex)
        {
            return slideIndex + 1 >= slides.Count;
        }

        private int GetCurrentSlideIndex()
        {
            if (CurrentSlide == null)
            {
                return 0;
            }

            var slideIndex = slides.IndexOf(CurrentSlide);

            return slideIndex;
        }

        #endregion
    }
}
