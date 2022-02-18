using System.IO;
using UnityEditor;

namespace SimplePresenter.Editor.Utils
{
    /// <summary>
    /// Utilities for working with files in Editor.
    /// </summary>
    internal static class EditorFiles
    {
        #region Public Methods

        /// <summary>
        /// Recursively create directory at given path.
        /// </summary>
        public static void CreateDirectory(string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (directoryPath == null || Directory.Exists(directoryPath))
            {
                return;
            }

            Directory.CreateDirectory(directoryPath);
            AssetDatabase.Refresh();
        }

        #endregion
    }
}
