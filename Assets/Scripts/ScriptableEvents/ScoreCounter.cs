using TMPro;
using UnityEngine;

namespace GamedevTools.ScriptableEvents
{
    public class ScoreCounter : MonoBehaviour
    {
        #region Editor

        [Tooltip("Text where to display score")]
        [SerializeField]
        private TMP_Text scoreText;

        #endregion

        #region Private Fields

        private int currentScore;

        #endregion

        #region Public Methods

        /// <summary>
        /// Increment current score.
        /// </summary>
        public void IncrementScore()
        {
            currentScore++;
            scoreText.text = currentScore.ToString();
        }

        #endregion
    }
}
