using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace GamedevTools.Common
{
    [CreateAssetMenu(
        fileName = "SpawnerSettings",
        menuName = "Gamedev Tools/Spawner Settings"
    )]
    public class SpawnerSettings : ScriptableObject
    {
        #region Editor Fields

        [Header("Clickables")]
        [Tooltip("Scale to randomize when creating new clickables")]
        [MinMaxSlider(0f, 5f)]
        [SerializeField]
        private Vector2 scaleRange = new Vector2(0.5f, 2f);

        [Tooltip("List of meshes to randomize when creating new clickables")]
        [SerializeField]
        private List<Mesh> meshes;

        [Tooltip("List of materials to randomize when creating new clickables")]
        [SerializeField]
        private List<Material> materials;

        #endregion

        #region Public Properties

        /// <summary>
        /// Random scale within <see cref="scaleRange"/>.
        /// </summary>
        public Vector3 RandomScale => Vector3.one * GetRandom(scaleRange);

        /// <summary>
        /// Random mesh from <see cref="meshes"/>.
        /// </summary>
        public Mesh RandomMesh => GetRandomElement(meshes);

        /// <summary>
        /// Random material from <see cref="materials"/>.
        /// </summary>
        public Material RandomMaterial => GetRandomElement(materials);

        #endregion

        #region Private Methods

        private static float GetRandom(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        private static T GetRandomElement<T>(IReadOnlyList<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }

            var randomIndex = Random.Range(0, list.Count);
            var element = list[randomIndex];

            return element;
        }

        #endregion
    }
}
