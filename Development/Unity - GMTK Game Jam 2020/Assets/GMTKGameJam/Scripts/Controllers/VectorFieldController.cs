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

	 [NonNull] public GameObject BorderCollider;
    
    // Start is called before the first frame update
    void Awake()
    {
		int width = (int)Bounds.width;
		int height = (int)Bounds.height;
		_vectorField = new VectorField((int)Bounds.width, (int)Bounds.height, 1);

		// Create borders...
		GameObject border = new GameObject();
		border.name = "Borders";
		border.transform.parent = transform;
		Transform borderParentRef = border.transform;
		/*
		GameObject a = Instantiate(BorderCollider, new Vector3(Bounds.width / 2, 0, 0), Quaternion.identity);
		a.transform.localScale = new Vector3(Bounds.width, 1, 1);
		a.transform.parent = border.transform;
		a = Instantiate(BorderCollider, new Vector3(Bounds.width / 2, 0, Bounds.height - 1), Quaternion.identity);
		a.transform.localScale = new Vector3(Bounds.width, 1, 1);
		a.transform.parent = border.transform;
		a = Instantiate(BorderCollider, new Vector3(0, 0, Bounds.height / 2), Quaternion.identity);
		a.transform.localScale = new Vector3(1, 1, Bounds.height);
		a.transform.parent = border.transform;
		a = Instantiate(BorderCollider, new Vector3(Bounds.width - 1, 0, Bounds.height / 2), Quaternion.identity);
		a.transform.localScale = new Vector3(1, 1, Bounds.height);
		a.transform.parent = border.transform;
		*/

		Quaternion r90OnY = Quaternion.Euler(0, 90, 0);
		for(int x = 0; x < VectorField.WidthByGrid; x++) {
			VectorField.Block(x, 0);
			VectorField.Block(x, VectorField.HeightByGrid - 1);

			GameObject a = Instantiate(BorderCollider, new Vector3(x, 0, 0), r90OnY);
			a.transform.parent = borderParentRef;
			a = Instantiate(BorderCollider, new Vector3(x, 0, Bounds.height - 1), r90OnY);
			a.transform.parent = borderParentRef;
		}
		for(int y = 0; y < VectorField.HeightByGrid; y++) {
			VectorField.Block(0, y);
			VectorField.Block(VectorField.WidthByGrid - 1, y);

			GameObject a = Instantiate(BorderCollider, new Vector3(0, 0, y), Quaternion.identity);
			a.transform.parent = borderParentRef;
			a = Instantiate(BorderCollider, new Vector3(Bounds.width - 1, 0, y), Quaternion.identity);
			a.transform.parent = borderParentRef;
		}

		foreach (Attractor attractor in _attractors)
		{
			_vectorField.AddAttractor(attractor);
		}

    }

	 void Update(){
		 _vectorField.UpdateForces();
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

		for(int x = 0; x < _bounds.width; x++) {
			Gizmos.color = Color.grey;
			Gizmos.DrawWireCube(new Vector3(x, 0.5f, 1), Vector3.one);
			Gizmos.DrawWireCube(new Vector3(x, 0.5f,  _bounds.height - 2), Vector3.one);
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(new Vector3(x, 0.5f, 0), Vector3.one);
			Gizmos.DrawWireCube(new Vector3(x, 0.5f,  _bounds.height - 1), Vector3.one);
		}
		for(int y = 0; y < _bounds.height; y++) {
			Gizmos.color = Color.grey;
			Gizmos.DrawWireCube(new Vector3(1, 0.5f, y), Vector3.one);
			Gizmos.DrawWireCube(new Vector3( _bounds.width - 2, 0.5f, y), Vector3.one);
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(new Vector3(0, 0.5f, y), Vector3.one);
			Gizmos.DrawWireCube(new Vector3( _bounds.width - 1, 0.5f, y), Vector3.one);
		}
    }
}
