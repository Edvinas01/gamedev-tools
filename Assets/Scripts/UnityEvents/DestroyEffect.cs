using UnityEngine;

namespace GamedevTools.UnityEvents
{
    [AddComponentMenu("Gamedev Tools/Unity Events/Destroy Effect")]
    public class DestroyEffect : MonoBehaviour
    {
        #region Editor Fields

        [Tooltip("Particle prefab")]
        [SerializeField]
        private ParticleSystem destroyParticles;

        [Tooltip("How much to offset the effect in Y coordinate on spawn")]
        [Range(0f, 10f)]
        [SerializeField]
        private float yOffset = 0.5f;

        #endregion

        #region Public Methods

        /// <summary>
        /// Spawn this effect at the position of this Game Object.
        /// </summary>
        public void SpawnEffect()
        {
            var destroyTransform = transform;
            var position = destroyTransform.position;
            position.y += yOffset;

            Instantiate(
                destroyParticles,
                position,
                destroyTransform.rotation,
                destroyTransform.parent
            );
        }

        #endregion
    }
}
