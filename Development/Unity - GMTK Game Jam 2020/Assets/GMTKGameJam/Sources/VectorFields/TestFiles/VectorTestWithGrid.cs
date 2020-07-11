using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VectorField;

public class VectorTestWithGrid : MonoBehaviour
{
	public VectorField vectorField;
	public GameObject demoObject;
	public GameObject solid;
	public int NumberOfParticles = 100;

	public int testX;
	public int testY;
	public Attractor[] Attractors;
	// Start is called before the first frame update
	void Awake()
	{
		// Create our Vector Field
		vectorField = new VectorField(100, 100, 10);

		int[,] testField = new int[,]{
			{1,1,1,1,1,1,1,1,1,1},
			{1,0,0,0,0,0,0,0,0,1},
			{1,0,1,0,0,0,0,0,0,1},
			{1,0,0,1,1,1,1,1,0,1},
			{1,0,0,0,1,0,0,1,0,1},
			{1,0,0,0,1,0,0,0,0,1},
			{1,0,0,0,1,0,0,1,0,1},
			{1,0,0,0,1,0,0,0,0,1},
			{1,0,0,0,0,0,0,1,0,1},
			{1,1,1,1,1,1,1,1,1,1}
		};

		// Add some random bits to it
		int GridSize = vectorField.GridSize;

		float GridSizeO2 = (float)(vectorField.GridSize / 2f);
		for(int x = 0; x < vectorField.WidthByGrid; x++) {
			for(int y = 0; y < vectorField.HeightByGrid; y++) {
				if(testField[y, x] == 1) vectorField.Block(x, y);
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
			if(a.behaviour == AttractorBehaviour.LINE_OF_SIGHT || a.behaviour == AttractorBehaviour.BOTH_LOS_AND_AROUND_WALLS) {
				
				if(vectorField != null) {
					Gizmos.color = Color.green;
					IEnumerable<Point> line = vectorField.GetPointsOnLine(testX, testY, a.x, a.y);
					IEnumerator<Point> enumer = line.GetEnumerator();
					while(enumer.MoveNext()) {
						Point p = enumer.Current;
						Gizmos.DrawWireCube(new Vector3(
							p.x * (vectorField.WidthByGrid + 0.5f), 
							p.y * (vectorField.HeightByGrid + 0.5f),
							0),
							new Vector3(vectorField.WidthByGrid, vectorField.HeightByGrid, 1)
						);
					}
				}
			}
		}
		
		if(vectorField != null) {
			Gizmos.color = Color.blue;
			Gizmos.DrawCube(new Vector3(
				testX, 
				testY,
				0),
				new Vector3(5, 5, 1)
			);
		}
		
	}
}
