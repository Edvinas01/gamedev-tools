using UnityEngine;

namespace GamedevTools.CsharpEvents
{
    [AddComponentMenu("Gamedev Tools/CSharp Events/Destroy Effect")]
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

        #region Unity Lifecycle

        private void Awake()
        {
            var destructible = GetComponent<Destructible>();
            if (destructible)
            {
                destructible.OnDestroyed += SpawnEffect;
            }
        }

        #endregion

        #region Private Methods

        private void SpawnEffect()
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
