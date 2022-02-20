using System;
using UnityEngine;

namespace GamedevTools.ScriptableEvents
{
    [CreateAssetMenu(
        fileName = "ScriptableEvent",
        menuName = "Gamedev Tools/Scriptable Event"
    )]
    public class ScriptableEvent : ScriptableObject
    {
        #region Public Properties

        /// <summary>
        /// Action which is triggered when this event is raised.
        /// </summary>
        public event Action OnRaised;

        #endregion

        #region Unity Lifecycle

        private void OnDisable()
        {
            OnRaised = null;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Raise this event and trigger its listeners.
        /// </summary>
        public void Raise()
        {
            OnRaised?.Invoke();
        }

        #endregion
    }
}
