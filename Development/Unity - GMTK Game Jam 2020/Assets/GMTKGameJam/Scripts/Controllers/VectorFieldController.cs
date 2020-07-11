using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorFieldController : MonoBehaviour
{
    private VectorField _vectorField;
    public VectorField VectorField => _vectorField;
    [SerializeField] protected Rect _bounds;
    public Rect Bounds => _bounds;

    [SerializeField] public Attractor[] _attractors;
    
    // Start is called before the first frame update
    void Awake()
    {
        _vectorField = new VectorField();
        foreach (Attractor attractor in _attractors)
        {
            _vectorField.AddAttractor(attractor);
        }
    }

    // Update is called once per frame
    private void OnDrawGizmos()
    {
        // Attractors are classes, and thus references. Feel free to store them somewhere
        // Remove them from the VectorField with RemoveAttractor(id)
        foreach (Attractor attractor in _attractors)
        {
            Gizmos.color = attractor.force < 0 ? Color.red : Color.green;
            Gizmos.DrawWireSphere(new Vector3(attractor.x, 0, attractor.y), attractor.force);
        }
    }
}
