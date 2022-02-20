using GamedevTools.Common;
using UnityEngine;
using UnityEngine.Events;

namespace GamedevTools.UnityEvents
{
    [AddComponentMenu("Gamedev Tools/Unity Events/Clickable")]
    public class Clickable : MonoBehaviour, IClickable
    {
        #region Editor Fields

        [SerializeField]
        public UnityEvent<int> onClicked;

        #endregion

        #region Public Methods

        public void OnClick(int clickDamage)
        {
            onClicked.Invoke(clickDamage);
        }

        #endregion
    }
}
