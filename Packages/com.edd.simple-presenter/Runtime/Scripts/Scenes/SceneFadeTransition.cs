using System;
using System.Collections;
using UnityEngine;

namespace SimplePresenter.Scenes
{
    /// <summary>
    /// Fades the scene in and out by utilizing a canvas.
    /// </summary>
    public sealed class SceneFadeTransition : BaseSceneTransition
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Canvas group whose alpha value to change on fade")]
        [SerializeField]
        private CanvasGroup canvasGroup;

        [Tooltip("Canvas which to disable and enable on fade")]
        [SerializeField]
        private Canvas canvas;

        [Header("Alpha")]
        [Tooltip("Canvas alpha when the scene is fully loaded")]
        [Range(0f, 1f)]
        [SerializeField]
        private float enteredAlpha;

        [Tooltip("Canvas alpha when the scene is unloaded")]
        [Range(0f, 1f)]
        [SerializeField]
        private float exitedAlpha = 1f;

        [Header("Duration")]
        [Tooltip("Time taken to fade in the scene (seconds)")]
        [Range(0f, 10f)]
        [SerializeField]
        private float enterDuration = 0.2f;

        [Tooltip("Time taken to fade out the scene (seconds)")]
        [Range(0f, 10f)]
        [SerializeField]
        private float exitDuration = 0.2f;

        #endregion

        #region Public Methods

        public override IEnumerator OnEnterScene()
        {
            ShowFader();
            yield return Fade(exitedAlpha, enteredAlpha, enterDuration, SetAlpha);
            HideFader();
        }

        public override IEnumerator OnExitScene()
        {
            ShowFader();
            yield return Fade(enteredAlpha, exitedAlpha, exitDuration, SetAlpha);
        }

        #endregion

        #region Private Methods

        private void ShowFader()
        {
            canvasGroup.enabled = true;
            canvas.enabled = true;
        }

        private void HideFader()
        {
            canvasGroup.enabled = false;
            canvas.enabled = false;
        }

        private void SetAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
        }

        private static IEnumerator Fade(float from, float to, float duration, Action<float> onFade)
        {
            var progress = 0f;

            while (progress < 1f)
            {
                var value = Mathf.Lerp(from, to, progress);
                onFade(value);

                progress += Time.unscaledDeltaTime / duration;

                yield return null;
            }

            onFade(to);

            yield return null;
        }

        #endregion
    }
}
