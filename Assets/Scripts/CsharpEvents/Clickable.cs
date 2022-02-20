using System;
using GamedevTools.Common;
using UnityEngine;

namespace GamedevTools.CsharpEvents
{
    [AddComponentMenu("Gamedev Tools/CSharp Events/Clickable")]
    public class Clickable : MonoBehaviour, IClickable
    {
        #region Public Properties

        /// <summary>
        /// Called when this clickable was clicked.
        /// </summary>
        public event Action<int> OnClicked;

        #endregion

        #region Public Methods

        public void OnClick(int clickDamage)
        {
            OnClicked?.Invoke(clickDamage);
        }

        #endregion
    }
}
