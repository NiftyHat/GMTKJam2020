using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorParticleTest : MonoBehaviour
{
	VectorField ReferenceToTheVectorField;
	public float speed = 10;
	void Start()
	{
		ReferenceToTheVectorField = FindObjectOfType<VectorTestWithGrid>().vectorField;
	}

	// Update is called once per frame
	void Update()
	{
		// Lerp from the Current Velocity to the Vector Grid's velocity
		Vector2 desiredVelocity = ReferenceToTheVectorField.GetPower(transform.position) * speed;
		Vector2 currentVelocity = transform.GetComponent<Rigidbody>().velocity;
		transform.GetComponent<Rigidbody>().velocity = Vector2.Lerp(currentVelocity, desiredVelocity, 0.01f);
		
		// Just keep it within bounds for now
		Vector3 m = transform.position;
		m.x = Mathf.Clamp(m.x, 0, 100);
		m.y = Mathf.Clamp(m.y, 0, 100);
		transform.position = m;
	}
}
