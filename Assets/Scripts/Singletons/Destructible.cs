using UnityEngine;

namespace GamedevTools.Singletons
{
    [AddComponentMenu("Gamedev Tools/Singletons/Destructible")]
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

        #endregion

        #region Private Fields

        private const int MinHealth = 0;
        private const int MaxHealth = 100;

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

            Spawner.Instance.Spawn();
        }

        #endregion
    }
}
