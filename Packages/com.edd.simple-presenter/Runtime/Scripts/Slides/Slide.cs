using System;
using SimplePresenter.Scenes;
using UnityEngine;

namespace SimplePresenter.Slides
{
    /// <summary>
    /// Holds immutable slide data.
    /// </summary>
    [Serializable]
    public sealed class Slide
    {
        #region Editor

        [Header("Scene Loading")]
        [Tooltip("Scene required to display this slide")]
        [SerializeField]
        private SceneReference sceneReference;

        [Header("Content")]
        [Tooltip("Slide title text")]
        [SerializeField]
        private string title;

        [Tooltip("Slide content prefab")]
        [SerializeField]
        private GameObject content;

        #endregion

        #region Public Properties

        /// <summary>
        /// Scene assigned to this slide.
        /// </summary>
        public SceneReference SceneReference => sceneReference;

        /// <summary>
        /// Title assigned to this slide.
        /// </summary>
        public string Title => title;

        /// <summary>
        /// Content prefab assigned to this slide.
        /// </summary>
        public GameObject Content => content;

        #endregion
    }
}
