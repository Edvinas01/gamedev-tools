using NaughtyAttributes;
using SimplePresenter.Scenes;
using UnityEngine;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Holds immutable slide data.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SceneReference",
        menuName = "SimplePresenter/Slide"
    )]
    public sealed class Slide : ScriptableObject
    {
        #region Editor Fields

        [Header("Scene Loading")]
        [Tooltip("Scene required to display this slide")]
        [SerializeField]
        private SceneReference sceneReference;

        [Header("Content")]
        [SerializeField]
        private SlideType slideType;

        [HideIf(nameof(slideType), SlideType.Empty)]
        [TextArea(4, 10)]
        [Tooltip("Slide title text")]
        [SerializeField]
        private string title;

        [ShowIf(nameof(slideType), SlideType.TextContent)]
        [TextArea(4, 10)]
        [Tooltip("Slide content text")]
        [SerializeField]
        private string text;

        [ShowIf(nameof(slideType), SlideType.CustomContent)]
        [Tooltip("Slide content prefab")]
        [SerializeField]
        private GameObject customContent;

        #endregion

        #region Public Properties

        /// <summary>
        /// Scene assigned to this slide.
        /// </summary>
        public SceneReference SceneReference => sceneReference;

        public SlideType SlideType => slideType;

        /// <summary>
        /// Title assigned to this slide.
        /// </summary>
        public string Title => title;

        /// <summary>
        /// Slide text.
        /// </summary>
        public string Text => text;

        /// <summary>
        /// Content prefab assigned to this slide.
        /// </summary>
        public GameObject CustomContent => customContent;

        #endregion
    }
}
