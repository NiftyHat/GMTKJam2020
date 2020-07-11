using System;
using UnityEngine;

public class VectorFieldDynamicUpdateBehavior : MonoBehaviour
{
    [SerializeField][NonNull] private VectorFieldController _vectorFieldController;
    [SerializeField] protected Attractor _attractor;
    [SerializeField] protected float _radius = 1f;
    
    private VectorField _vectorField;
    
    void Start()
    {
        if (_vectorFieldController == null)
        {
            _vectorFieldController = FindObjectOfType<VectorFieldController>();
        }

        if (_vectorFieldController != null)
        {
            _vectorField = _vectorFieldController.VectorField;
        }
        _attractor = new Attractor();
        _attractor.x = (int)transform.position.x;
        _attractor.y = (int)transform.position.z;
        _attractor.force = _radius;
        _vectorField.AddAttractor(_attractor);
    }

    private void OnDestroy()
    {
        _vectorField.RemoveAttractor(_attractor);
    }

    // Update is called once per frame
    void Update()
    {
        if (_attractor != null)
        {
            _attractor.x = (int)transform.position.x;
            _attractor.y = (int)transform.position.z;
            _attractor.force = _radius;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _attractor.force < 0 ? Color.red : Color.green;
        Gizmos.DrawWireSphere(new Vector3(_attractor.x, 0, _attractor.y), _attractor.force);
    }
}
