using UnityEditor;
using UnityEngine;

namespace SimplePresenter.Editor.Utils
{
    /// <summary>
    /// Used to create scriptable objects which contain settings. Only to be used in Editor scripts.
    /// </summary>
    internal abstract class BaseEditorSettings<T> : ScriptableObject where T : ScriptableObject
    {
        #region Private Fields

        private static T instance;

        #endregion

        #region Public Properties

        /// <summary>
        /// Instance of the settings asset.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = LoadSettings();
                }

                return instance;
            }
        }

        #endregion

        #region Private Properties

        private static string AssetPath
        {
            get
            {
                var pathAttribute = FindPathAttribute();
                if (pathAttribute == null)
                {
                    return DefaultAssetPath;
                }

                return $"{pathAttribute.Path}.{pathAttribute.Extension}";
            }
        }

        private static string DefaultAssetPath => $"{typeof(T).Name}.asset";

        #endregion

        #region Public Methods

        /// <summary>
        /// Persist settings asset to disk. Should be called after making changes via an Editor
        /// script.
        /// </summary>
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        #endregion

        #region Private Methods

        private static EditorSettingsPathAttribute FindPathAttribute()
        {
            var attributes = typeof(T).GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                if (attribute is EditorSettingsPathAttribute pathAttribute)
                {
                    return pathAttribute;
                }
            }

            return null;
        }

        private static T LoadSettings()
        {
            return EditorScriptableObjects.FindOrCreateAsset<T>(AssetPath);
        }

        #endregion
    }
}
