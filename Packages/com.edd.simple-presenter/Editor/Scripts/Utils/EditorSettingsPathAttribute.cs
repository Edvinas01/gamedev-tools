using System;

namespace SimplePresenter.Editor.Utils
{
    /// <summary>
    /// Attribute used on <see cref="BaseEditorSettings{T}"/> to specify how the asset should be
    /// saved.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class EditorSettingsPathAttribute : Attribute
    {
        #region Public Properties

        /// <summary>
        /// Path to the asset (without the extension).
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Asset extension (usually 'asset').
        /// </summary>
        public string Extension { get; }

        #endregion

        #region Public Methods

        public EditorSettingsPathAttribute(string path, string extension = "asset")
        {
            Path = path;
            Extension = extension;
        }

        #endregion
    }
}
