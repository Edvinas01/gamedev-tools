using UnityEngine;
using UnityEngine.Events;

namespace GamedevTools.ScriptableEvents
{
    [AddComponentMenu("Gamedev Tools/Scriptable Events/Scriptable Listener")]
    public class ScriptableListener : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private ScriptableEvent scriptableEvent;

        [SerializeField]
        private UnityEvent onRaised;

        #endregion

        #region Unity Lifecycle

        private void OnEnable()
        {
            scriptableEvent.OnRaised += OnRaised;
        }

        private void OnDisable()
        {
            scriptableEvent.OnRaised -= OnRaised;
        }

        #endregion

        #region Private Pethods

        private void OnRaised()
        {
            onRaised.Invoke();
        }

        #endregion
    }
}
