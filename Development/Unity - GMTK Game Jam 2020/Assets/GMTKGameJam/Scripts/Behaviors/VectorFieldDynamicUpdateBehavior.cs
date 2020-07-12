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

        _attractor = new Attractor {x = (int) transform.position.x, y = (int) transform.position.z, force = _radius};
		  _attractor.hardCutOff = true;
		  _attractor.behaviour = AttractorBehaviour.LINE_OF_SIGHT;
		  _vectorField.AddAttractor(_attractor);
    }

    private void OnDestroy()
    {
        if (_vectorField != null)
        {
            _vectorField.RemoveAttractor(_attractor);
        }
    }

    public void SetRepelForce(float force)
    {
        _radius = -force;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_attractor != null)
        {
            _attractor.x = (int)transform.position.x;
            _attractor.y = (int)transform.position.z;
            _attractor.force = _radius;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = _attractor.force < 0 ? Color.red : Color.green;
        Gizmos.DrawWireSphere(new Vector3(_attractor.x, 0, _attractor.y), _attractor.force);
    }
}
