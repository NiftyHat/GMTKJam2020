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
	void Awake()
	{
		// Create our Vector Field
		vectorField = new VectorField(50, 50, 2);

		// Add some random bits to it
		int GridSize = vectorField.GridSize;

		for(int i = 0; i < 3 * Mathf.Sqrt(vectorField.WidthByGrid * vectorField.HeightByGrid); i++) {
			vectorField.Block(Random.Range(0, vectorField.WidthByGrid), Random.Range(0, vectorField.HeightByGrid));
		}

		float GridSizeO2 = (float)(vectorField.GridSize / 2f);
		for(int x = 0; x < vectorField.WidthByGrid; x++) {
			for(int y = 0; y < vectorField.HeightByGrid; y++) {
				if(vectorField.IsBlockedAt(x, y)) {
					GameObject b = Instantiate(solid, 
					new Vector3(
						x * GridSize + GridSizeO2, 
						y * GridSize + GridSizeO2, 0
					), Quaternion.identity);
					b.transform.localScale = new Vector3(GridSize, GridSize, 10);
				}
			}
		}

		// Add all our attractors to it.
		foreach(Attractor a in Attractors) {
			vectorField.AddAttractor(a);
		}
		

		// Make a load of cats
		for(int i = 0; i < NumberOfParticles; i++) {
			Instantiate(demoObject, new Vector3(Random.Range(0, vectorField.Width), Random.Range(0, vectorField.Height), 0), Quaternion.identity);
		}
		Destroy(demoObject);
		Destroy(solid);
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
