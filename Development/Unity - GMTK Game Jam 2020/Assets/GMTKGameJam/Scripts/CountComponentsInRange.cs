using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CountComponentsInRange : MonoBehaviour
{
    [SerializeField] private float _radius = 5;
    private int _count;

    public int Count<TComponent>() where TComponent : MonoBehaviour
    {
        Collider[] overlappingCollides = Physics.OverlapSphere(transform.position, _radius);
        _count =  overlappingCollides.Count(item => item.GetComponent<TComponent>());
        return _count;
    }
    
    public List<TComponent> GetAll<TComponent>() where TComponent : MonoBehaviour
    {
        Collider[] overlappingCollides = Physics.OverlapSphere(transform.position, _radius);
        List<TComponent> all = new List<TComponent>();
        foreach (var item in overlappingCollides)
        {
            TComponent comp = item.GetComponent<TComponent>();
            if (comp != null)
            {
                all.Add(comp);
            }
        }
        return all;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }

    // Update is called once per frame
    public void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(position.x, position.y, position.z), _radius);
    }
}
