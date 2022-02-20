using GamedevTools.Common;
using UnityEngine;

namespace GamedevTools.CsharpEvents
{
    [AddComponentMenu("Gamedev Tools/CSharp Events/Spawner")]
    public class Spawner : MonoBehaviour
    {
        #region Editor Fields

        [Header("Dependencies")]
        [Tooltip("Common spawner settings")]
        [SerializeField]
        private SpawnerSettings spawnerSettings;

        [Tooltip("Base clickable prefab used to create new clickables")]
        [SerializeField]
        private GameObject templatePrefab;

        [Tooltip("Where to spawn clickables")]
        [SerializeField]
        private Transform spawnPoint;

        #endregion

        #region Unity Lifecycle

        private void Start()
        {
            Spawn();
        }

        #endregion

        #region Private Methods

        private void Spawn()
        {
            var clickable = CreateClickable();
            SetupScale(clickable);

            var randomMesh = spawnerSettings.RandomMesh;
            if (randomMesh)
            {
                SetupMeshCollider(clickable, randomMesh);
                SetupMeshFilter(clickable, randomMesh);
            }

            var randomMaterial = spawnerSettings.RandomMaterial;
            if (randomMaterial)
            {
                SetupMeshRenderer(clickable, randomMaterial);
            }

            SetupDestructible(clickable);
        }

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
            var randomScale = spawnerSettings.RandomScale;

            clickableTransform.localScale = randomScale;
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
                destructible.OnDestroyed += Spawn;
            }
        }

        #endregion
    }
}
