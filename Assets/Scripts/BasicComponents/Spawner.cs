using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GamedevTools.BasicComponents
{
    [AddComponentMenu("Gamedev Tools/Basic Components/Spawner")]
    public class Spawner : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Where to spawn clickables")]
        [SerializeField]
        private Transform spawnPoint;

        [Header("Clickables")]
        [Tooltip("Base clickable prefab used to create new clickables")]
        [SerializeField]
        private GameObject templatePrefab;

        [Tooltip("Scale to randomize when creating new clickables")]
        [MinMaxSlider(0f, 5f)]
        [SerializeField]
        private Vector2 scale = new Vector2(0.5f, 2f);

        [Tooltip("List of meshes to randomize when creating new clickables")]
        [SerializeField]
        private List<Mesh> meshes;

        [Tooltip("List of materials to randomize when creating new clickables")]
        [SerializeField]
        private List<Material> materials;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            Spawn();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Spawn a new clickable.
        /// </summary>
        public void Spawn()
        {
            var clickable = CreateClickable();
            SetupScale(clickable);

            var randomMesh = GetRandomElement(meshes);
            if (randomMesh)
            {
                SetupMeshCollider(clickable, randomMesh);
                SetupMeshFilter(clickable, randomMesh);
            }

            var randomMaterial = GetRandomElement(materials);
            if (randomMaterial)
            {
                SetupMeshRenderer(clickable, randomMaterial);
            }

            SetupDestructible(clickable);
        }

        #endregion

        #region Private Methods

        private GameObject CreateClickable()
        {
            return Instantiate(
                templatePrefab,
                spawnPoint.position,
                spawnPoint.rotation,
                spawnPoint
            );
        }

        private void SetupScale(GameObject clickable)
        {
            var clickableTransform = clickable.transform;
            var randomScale = Random.Range(scale.x, scale.y);

            clickableTransform.localScale = Vector3.one * randomScale;
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

        private static void SetupMeshFilter(GameObject clickable, Mesh mesh)
        {
            var meshFilter = clickable.GetComponent<MeshFilter>();
            if (meshFilter)
            {
                meshFilter.sharedMesh = mesh;
            }
        }

        private static void SetupMeshRenderer(GameObject clickable, Material material)
        {
            var renderer = clickable.GetComponent<MeshRenderer>();
            if (renderer)
            {
                renderer.sharedMaterial = material;
            }
        }

        private static void SetupMeshCollider(GameObject clickable, Mesh mesh)
        {
            var collider = clickable.GetComponent<MeshCollider>();
            if (collider)
            {
                collider.sharedMesh = mesh;
            }
        }

        private void SetupDestructible(GameObject clickable)
        {
            var destructible = clickable.GetComponent<Destructible>();
            if (destructible)
            {
                destructible.Spawner = this;
            }
        }

        #endregion
    }
}
