using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddColliderToVectorField : MonoBehaviour
{
	VectorField referenceToVectorField;
	void Start() {
		referenceToVectorField = FindObjectOfType<VectorFieldController>().VectorField;
		referenceToVectorField.Block(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(new Vector3(Mathf.RoundToInt(transform.position.x), 0.5f, Mathf.RoundToInt(transform.position.z)), Vector3.one);
	}
}
