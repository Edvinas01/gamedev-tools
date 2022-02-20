using UnityEngine;
using UnityEngine.Events;

namespace GamedevTools.UnityEvents
{
    [AddComponentMenu("Gamedev Tools/Unity Events/Destructible")]
    public class Destructible : MonoBehaviour
    {
        #region Editor Fields

        [Tooltip(
            "Total amount of health on this destructible, if it goes below 0, the destructible " +
            "gets destroyed"
        )]
        [Range(MinHealth, MaxHealth)]
        [SerializeField]
        private int health = MinHealth + 1;

        [SerializeField]
        private UnityEvent onDestroyed;

        #endregion

        #region Private Fields

        private const int MinHealth = 0;
        private const int MaxHealth = 100;

        #endregion

        #region Public Properties

        /// <summary>
        /// Called when this destructible is destroyed.
        /// </summary>
        public event UnityAction OnDestroyed
        {
            add => onDestroyed.AddListener(value);
            remove => onDestroyed.RemoveListener(value);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Apply damage to this destructible and destroy it if necessary.
        /// </summary>
        public void ApplyDamage(int damage)
        {
            if (damage < 0)
            {
                Debug.LogError($"{nameof(damage)} muse be a positive value", this);
                return;
            }

            if (health <= MinHealth)
            {
                health = MinHealth;
                return;
            }

            health -= damage;

            if (health > MinHealth)
            {
                return;
            }

            Destroy(gameObject);

            onDestroyed.Invoke();
        }

        #endregion
    }
}
