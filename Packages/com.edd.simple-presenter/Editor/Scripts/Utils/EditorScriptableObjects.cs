using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimplePresenter.Editor.Utils
{
    /// <summary>
    /// Utilities for working with scriptable objects in the Editor.
    /// </summary>
    internal static class EditorScriptableObjects
    {
        #region Public Methods

        /// <returns>
        /// New ScriptableObject asset at given path or an existing one.
        /// </returns>
        public static T FindOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            var asset = FindAsset<T>();
            return asset != null
                ? asset
                : CreateAsset<T>(path);
        }

        /// <summary>
        /// Find scriptable object asset of given type..
        /// </summary>
        public static T FindAsset<T>() where T : ScriptableObject
        {
            return AssetDatabase
                .FindAssets($"t:{typeof(T).Name}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .FirstOrDefault();
        }

        /// <summary>
        /// Create a scriptable object asset at given path.
        /// </summary>
        public static T CreateAsset<T>(string path) where T : ScriptableObject
        {
            EditorFiles.CreateDirectory(path);

            var scriptableObject = ScriptableObject.CreateInstance<T>();
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(scriptableObject, uniquePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return scriptableObject;
        }

        #endregion
    }
}
