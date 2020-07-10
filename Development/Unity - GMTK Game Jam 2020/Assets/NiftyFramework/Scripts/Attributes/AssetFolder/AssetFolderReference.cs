using System;
using UnityEngine;

namespace NiftyFramework.Attributes
{
    [Serializable]
    public class AssetFolderReference
    {
        public AssetFolderReference(string subfolderPath)
        {
            
        }

        /// <summary>
        ///   <para>The default file name used by newly created instances of this type.</para>
        /// </summary>
        [SerializeField]
        protected string _subfolderPath;
        public string subfolderPath => _subfolderPath;
    }
}