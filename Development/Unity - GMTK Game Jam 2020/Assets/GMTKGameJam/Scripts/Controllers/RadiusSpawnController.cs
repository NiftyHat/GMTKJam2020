using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class RadiusSpawnController : MonoBehaviour
{
    [SerializeField] private float _radius = 1f;
    [SerializeField] private int _itemsPerSpawn;
    [SerializeField] private GameObject[] _prefabList;
    [SerializeField] private PrefabObjectPool _pool;
    [SerializeField] private bool _spawnOnStart;
    [SerializeField] private float _objectSpacing = 1.4f;

    private Vector3 _offset;

    public void Awake()
    {
        _pool = new PrefabObjectPool(_prefabList, 100);
        _offset = new Vector3(-_radius,0 , -_radius);
    }
    
    public void Start()
    {
        if (_spawnOnStart)
        {
            Spawn(_itemsPerSpawn, transform);
        }
    }

    public void Spawn(int count = -1, Transform parent = null)
    {
        PoissonDiscSampler poissonDiscSampler = new PoissonDiscSampler(_radius * 2, _radius * 2, _objectSpacing);
        IEnumerable<Vector2> samples = poissonDiscSampler.Samples();
        int spawnCount = 0;
        
        foreach (var item in samples)
        {
            Vector3 origin = transform.position + _offset;
            spawnCount++;
            Vector3 randomPosition = origin + new Vector3(item.x, 0, item.y);
            Quaternion facing = Quaternion.Euler(randomPosition - origin);
            RollingCatController controller = _pool.Get<RollingCatController>(parent, randomPosition, facing);
            //controller.gameObject.transform.LookAt(transform.position - randomPosition  *2f);
            if (spawnCount > _itemsPerSpawn)
            {
                break;
            }
        }
    }

    public void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(new Vector3(position.x, position.y, position.z), _radius);
    }
}
