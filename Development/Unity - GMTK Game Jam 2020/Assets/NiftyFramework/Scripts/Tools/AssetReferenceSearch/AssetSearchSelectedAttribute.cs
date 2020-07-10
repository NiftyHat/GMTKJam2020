using System;
using UnityEngine;

namespace NiftyFramework.Tools
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class AssetSearchSelectedAttribute : PropertyAttribute
    {
        public AssetSearchSelectedAttribute(string subfolderPath, Type creationType)
        {
            this.subfolderPath = subfolderPath;
            this.creationType = creationType;
        }

        public Type creationType { get; }

        /// <summary>
        ///   <para>The default file name used by newly created instances of this type.</para>
        /// </summary>
        public string subfolderPath { get; }
    }
}