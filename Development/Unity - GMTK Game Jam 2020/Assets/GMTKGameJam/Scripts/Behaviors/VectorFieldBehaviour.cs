using System;
using UnityEngine;

public class VectorFieldBehaviour : MonoBehaviour
{
    [SerializeField] private VectorFieldController _vectorFieldController;
    [SerializeField] protected Attractor _attractor;
    
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

		  _attractor.x = (int)transform.position.x;
		  _attractor.y = (int)transform.position.z;

		  _vectorField.AddAttractor(_attractor);
    }

	 void Update() {
		  _attractor.x = (int)transform.position.x;
		  _attractor.y = (int)transform.position.z;
		  _attractor.dirty = true;
	 }

    private void OnDestroy()
    {
        if (_vectorField != null)
        {
            _vectorField.RemoveAttractor(_attractor);
        }
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _attractor.force < 0 ? Color.red : Color.green;
        Gizmos.DrawWireSphere(new Vector3(_attractor.x, 0, _attractor.y), _attractor.force);
    }
}
