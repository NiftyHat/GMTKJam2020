﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorParticleTest : MonoBehaviour
{
	VectorTest ReferenceToTheVectorTestBehaviour;
	public float speed = 10;
	void Start()
	{
		ReferenceToTheVectorTestBehaviour = FindObjectOfType<VectorTest>();
	}

	// Update is called once per frame
	void Update()
	{
		// Lerp from the Current Velocity to the Vector Grid's velocity
		Vector2 desiredVelocity = ReferenceToTheVectorTestBehaviour.vectorField.GetPower(transform.position) * speed;
		Vector2 currentVelocity = transform.GetComponent<Rigidbody>().velocity;
		transform.GetComponent<Rigidbody>().velocity = Vector2.Lerp(currentVelocity, desiredVelocity, 0.01f);
		
		// Just keep it within bounds for now
		Vector3 m = transform.position;
		m.x = Mathf.Clamp(m.x, -100, 100);
		m.y = Mathf.Clamp(m.y, -100, 100);
		transform.position = m;
	}
}
