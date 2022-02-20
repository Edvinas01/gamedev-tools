using System;
using UnityEngine;

namespace GamedevTools.CsharpEvents
{
    [AddComponentMenu("Gamedev Tools/CSharp Events/Clickable")]
    public class Clickable : MonoBehaviour
    {
        #region Public Properties

        /// <summary>
        /// Called when this clickable was clicked.
        /// </summary>
        public event Action<int> OnClicked;

        #endregion

        #region Public Methods

        /// <summary>
        /// Click this clickable.
        /// </summary>
        public void OnClick(int clickDamage)
        {
            OnClicked?.Invoke(clickDamage);
        }

        #endregion
    }
}
