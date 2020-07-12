using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Managed pool of prefab objects
/// </summary>
/// <typeparam name="TComponent">Required component on the root of the prefab</typeparam>
public class PrefabObjectPool
{
    private GameObject[] _prefabList;
    private int _size;
    private List<GameObject> _pooledItems = new List<GameObject>();
    private string _name;
    private GameObject _root;
    
    public const string SceneName = "POOL SCENE";
    private static Scene _poolingScene;

    public PrefabObjectPool(GameObject prefab, int size = 500)
    {
        _name = "Pool - " + prefab.name;
        _size = size;
        _prefabList = new[] {prefab};
        _root = GetRoot();
    }
    
    public PrefabObjectPool(GameObject[] prefabList, int size = 500)
    {
        _name = "Pool - Mixed";
        _prefabList = prefabList;
        _size = size;
        _root = GetRoot();
    }

    public GameObject GetRoot()
    {
        _poolingScene = SceneManager.GetSceneByName(SceneName);
        if (!_poolingScene.IsValid())
        {
            _poolingScene = SceneManager.CreateScene(SceneName);
            if (_poolingScene.IsValid())
            {
                _root = new GameObject(_name);
                SceneManager.MoveGameObjectToScene(_root, _poolingScene);
                return _root;
            }
        }
        return null;
    }

    public void Fill(int count)
    {
        GameObject prefab = GetPrefab();
        _root = new GameObject(_name);
        for (int i = 0; i < count && i < _size; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, _root.transform);
        }
    }
    
    public TComponent Get<TComponent>(Transform parent, Vector3 position, Quaternion rotation) where TComponent : MonoBehaviour 
    {
        for (int i = 0; i < _pooledItems.Count; i++)
        {
            var item = _pooledItems[i];
            // check active & return innactive
            if (!item.gameObject.activeInHierarchy)
            {
                if (parent != null)
                {
                    ResetGameObject(item.gameObject, parent, position, rotation);
                }
                else
                {
                    ResetGameObject(item.gameObject, position, rotation);
                }
                
                return item.GetComponent<TComponent>();
            }
        }
        
        // Put a cap on how many we can have
        if (_pooledItems.Count < _size)
        {
            GameObject prefab = GetPrefab();
            GameObject obj = GameObject.Instantiate(prefab, position, rotation, parent);
            if (obj != null)
            {
                TComponent component = obj.GetComponent<TComponent>();
                if (component != null)
                {
                    component.gameObject.SetActive(true);
                    _pooledItems.Add(component.gameObject);
                }
                return component;
            }
        }
        return null;
    }

    private GameObject GetPrefab()
    {
        if (_prefabList.Length == 1)
        {
            return _prefabList[0];
        }
        return _prefabList[Random.Range(0, _prefabList.Length)];
    }

    public void ResetGameObject(GameObject gameObject, Transform parent, Vector3 position, Quaternion rotation)
    {
        var transform = gameObject.transform;
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
        transform.parent = parent;
    }
    
    public void ResetGameObject(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        var transform = gameObject.transform;
        gameObject.SetActive(true);
        transform.position = position;
        transform.rotation = rotation;
    }

    private void PoolAll()
    {
        foreach (var item in _pooledItems)
        {
            item.gameObject.SetActive(false);
        }
    }
}
