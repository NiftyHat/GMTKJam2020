using System;
using UnityEngine;

[Serializable]
public class SerializedObjectPool
{
    [SerializeField] protected GameObject _prefab;
    [SerializeField] protected int _size;
    private PrefabObjectPool _pool;

    public PrefabObjectPool Pool
    {
        get
        {
            if (_pool == null)
            {
                _pool = new PrefabObjectPool(_prefab, _size);
            }
            return _pool;
        }
    }
}

