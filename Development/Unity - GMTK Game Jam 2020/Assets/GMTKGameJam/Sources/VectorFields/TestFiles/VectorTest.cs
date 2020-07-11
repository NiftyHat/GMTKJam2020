using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTest : MonoBehaviour
{
	public VectorField vectorField;
	public GameObject demoObject;
	public GameObject colliderObject;

	public int NumberOfParticles = 100;
	public int NumberOfColliders = 30;

	public Attractor[] Attractors;

	void Start() {
		// Create our Vector Field
		vectorField = new VectorField();

		// Add all our attractors to it.
		foreach(Attractor a in Attractors) {
			vectorField.AddAttractor(a);
		}

		// Make a load of cats and colliders
		for(int i = 0; i < NumberOfParticles; i++) {
			Instantiate(demoObject, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0), Quaternion.identity);
		}
		for(int i = 0; i < NumberOfColliders; i++) {
			Instantiate(colliderObject, new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
		}
		Destroy(demoObject);
		Destroy(colliderObject);

		// Our VectorParticleTest will run the "which way am I going?" on Update
	}
	void OnDrawGizmos () {

		// Attractors are classes, and thus references. Feel free to store them somewhere
		// Remove them from the VectorField with RemoveAttractor(id)
		foreach(Attractor a in Attractors) {
			Gizmos.color = a.force < 0 ? Color.red : Color.green;
			Gizmos.DrawWireSphere(new Vector3(a.x, a.y, 0), a.force);
		}
	}
}
