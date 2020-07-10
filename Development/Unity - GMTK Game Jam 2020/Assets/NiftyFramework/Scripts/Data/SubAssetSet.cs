using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NiftyFramework.Data
{
    public class SubAssetSet<TAsset> : ScriptableObject where TAsset : Object
    {
        [SerializeField] protected TAsset[] _references;
        public TAsset[] References => _references;

        public void Add(TAsset asset)
        {
            if (_references.Contains(asset))
            {
                return;
            }
            int newLength = _references.Length + 1;
            Array.Resize(ref _references, newLength);
            _references[newLength - 1] = asset;
        }

        public void Add(TAsset[] assetList)
        {
            for (int i = 0; i < assetList.Length; i++)
            {
                TAsset asset = assetList[i];
                Add(asset);
            }
        }

        public void Remove(TAsset asset)
        {
            //TODO - make this more efficient.
            int removedIndex = -1;
            for (int i = 0; i < _references.Length; i++)
            {
                if (_references[i] == asset)
                {
                    removedIndex = i;
                }

                if (i > removedIndex)
                {
                    _references[i - 1] = _references[i];
                }
            }
        }
    }
}