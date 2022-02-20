using GamedevTools.Common;
using GamedevTools.Singletons;
using UnityEngine;

namespace GamedevTools.BasicComponents
{
    [AddComponentMenu("Gamedev Tools/Basic Components/Clickable")]
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
