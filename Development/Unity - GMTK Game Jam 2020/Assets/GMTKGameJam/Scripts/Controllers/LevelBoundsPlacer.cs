using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBoundsPlacer : MonoBehaviour
{
	VectorField VectorField;
	// Start is called before the first frame update
	public GameObject BasicCollider;
	void Start()
	{

		VectorField = FindObjectOfType<VectorFieldController>().VectorField;
		for(int x = 0; x < VectorField.WidthByGrid; x++) {
			for(int y = 0; y < VectorField.HeightByGrid; y++) {
				if(VectorField.IsBlockedAt(x, y)) Block(x, y);
			}
		}

		BasicCollider.gameObject.SetActive(false);
	}

	private void Block(int x, int y)
	{
		//VectorField.Block(x, y);
		GameObject b = Instantiate(BasicCollider);
		b.transform.localScale = new Vector3(VectorField.GridSize, VectorField.GridSize, VectorField.GridSize);
		b.transform.position = new Vector3(x * VectorField.GridSize, VectorField.GridSize * 0.5f, y * VectorField.GridSize);
		b.transform.parent = transform;
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
