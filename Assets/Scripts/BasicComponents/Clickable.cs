using UnityEngine;

namespace GamedevTools.BasicComponents
{
    [AddComponentMenu("Gamedev Tools/Basic Components/Clickable")]
    public class Clickable : MonoBehaviour
    {
        #region Editor Fields

        [Tooltip("Destructible to notify about the click (deal damage)")]
        [SerializeField]
        private Destructible destructible;

        #endregion

        #region Public Methods

        /// <summary>
        /// Click this clickable.
        /// </summary>
        public void OnClick(int clickDamage)
        {
            destructible.ApplyDamage(clickDamage);
        }

        #endregion
    }
}
