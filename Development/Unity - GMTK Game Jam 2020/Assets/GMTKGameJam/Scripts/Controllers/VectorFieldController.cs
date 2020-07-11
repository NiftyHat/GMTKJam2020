﻿using System.Collections;
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
		int width = (int)Bounds.width;
		int height = (int)Bounds.height;
		_vectorField = new VectorField(100, 100, 1);

		for(int x = 0; x < VectorField.WidthByGrid; x++) {
			VectorField.Block(x, 0);
			VectorField.Block(x, VectorField.HeightByGrid - 1);
		}
		for(int y = 0; y < VectorField.HeightByGrid; y++) {
			VectorField.Block(0, y);
			VectorField.Block(VectorField.WidthByGrid - 1, y);
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

		Gizmos.color = Color.grey;
		for(int x = 0; x < _bounds.width; x++) {
			Gizmos.DrawWireCube(new Vector3(x, 0.5f, 0), Vector3.one);
			Gizmos.DrawWireCube(new Vector3(x, 0.5f,  _bounds.height), Vector3.one);
		}
		for(int y = 0; y < _bounds.height; y++) {
			Gizmos.DrawWireCube(new Vector3(0, 0.5f, y), Vector3.one);
			Gizmos.DrawWireCube(new Vector3( _bounds.width, 0.5f, y), Vector3.one);
		}
    }
}
