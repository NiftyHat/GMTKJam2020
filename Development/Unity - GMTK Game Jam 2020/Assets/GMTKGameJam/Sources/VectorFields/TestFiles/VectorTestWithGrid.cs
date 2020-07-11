using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTestWithGrid : MonoBehaviour
{
	public VectorField vectorField;
	public GameObject demoObject;
	public GameObject solid;
	public int NumberOfParticles = 100;
	public Attractor[] Attractors;
	// Start is called before the first frame update
	void Start()
	{
		// Create our Vector Field
		vectorField = new VectorField(100, 100, 10);

		// Add some random bits to it

		for(int i = 0; i < 25; i++) {
			vectorField.Block(Random.Range(0, vectorField.WidthByGrid), Random.Range(0, vectorField.HeightByGrid));
		}

		for(int x = 0; x < vectorField.WidthByGrid; x++) {
			for(int y = 0; y < vectorField.HeightByGrid; y++) {
				if(vectorField.IsBlockedAt(x, y)) {
					Instantiate(solid, 
					new Vector3(
						x * 10 + 5, 
						y * 10 + 5, 0
					), Quaternion.identity);
				}
			}
		}

		// Add all our attractors to it.
		foreach(Attractor a in Attractors) {
			vectorField.AddAttractor(a);
		}
		

		// Make a load of cats
		for(int i = 0; i < NumberOfParticles; i++) {
			Instantiate(demoObject, new Vector3(Random.Range(0, 100), Random.Range(0, 100), 0), Quaternion.identity);
		}
	}

	// Update is called once per frame

	void Update() {
		foreach(Attractor a in Attractors) {
			a.dirty = true;
		}
		vectorField.UpdateForces();
	}


	void OnDrawGizmos() {
		// Attractors are classes, and thus references. Feel free to store them somewhere
		// Remove them from the VectorField with RemoveAttractor(id)
		foreach(Attractor a in Attractors) {
			Gizmos.color = a.force < 0 ? Color.red : Color.green;
			Gizmos.DrawWireSphere(new Vector3(a.x, a.y, 0), a.force);
		}
	}
}
