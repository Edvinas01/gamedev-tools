using NaughtyAttributes;
using UnityEngine;

namespace SimplePresenter.Scenes
{
    /// <summary>
    /// Reference to a scene which can be used to load scenes via <see cref="SceneLoader"/>.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SceneReference",
        menuName = "SimplePresenter/Scene Reference"
    )]
    public sealed class SceneReference : ScriptableObject
    {
        #region Editor Fields

        [Tooltip("Scene build index assigned to this reference")]
        [Scene]
        [SerializeField]
        private int sceneBuildIndex;

        #endregion

        #region Public Properties

        /// <summary>
        /// Scene build index which can be used in
        /// <see cref="UnityEngine.SceneManagement.SceneManager"/>.
        /// </summary>
        public int SceneBuildIndex => sceneBuildIndex;

        #endregion
    }
}
