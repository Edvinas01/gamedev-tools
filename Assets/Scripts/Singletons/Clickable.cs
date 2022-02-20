using GamedevTools.Common;
using UnityEngine;

namespace GamedevTools.Singletons
{
    [AddComponentMenu("Gamedev Tools/Singletons/Clickable")]
    public class Clickable : MonoBehaviour, IClickable
    {
        #region Editor Fields

        [Tooltip("Destructible to notify about the click (deal damage)")]
        [SerializeField]
        private Destructible destructible;

        #endregion

        #region Public Methods

        public void OnClick(int clickDamage)
        {
            destructible.ApplyDamage(clickDamage);
        }

        #endregion
    }
}
