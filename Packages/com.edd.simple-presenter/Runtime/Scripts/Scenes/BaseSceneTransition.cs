using System.Collections;
using UnityEngine;

namespace SimplePresenter.Scenes
{
    /// <summary>
    /// Base class for performing smooth scene transitions in <see cref="SceneLoader"/>.
    /// </summary>
    public abstract class BaseSceneTransition : MonoBehaviour
    {
        /// <summary>
        /// Invoked when the scene is about to be entered.
        /// </summary>
        public abstract IEnumerator OnEnterScene();

        /// <summary>
        /// Invoked when the scene is about to be exited.
        /// </summary>
        public abstract IEnumerator OnExitScene();
    }
}
