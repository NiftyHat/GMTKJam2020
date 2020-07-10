using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorField
{
	// Width of the map, in Map Units
	public bool isLocked {get; private set;}
	private Dictionary<int, Attractor> Attractors;
	private bool[,] map;

	public VectorField()
	{
		Attractors = new Dictionary<int, Attractor>();
	}

	public void AddOrReplaceAttractor(Attractor a) {
		Attractors[a.id] = a;
	}

	public void RemoveAttractor(int id) {
		if(Attractors.ContainsKey(id)) Attractors.Remove(id);
	}

	public Vector2 GetPower(Vector2 from) {
		Vector2 result = new Vector2();
		float x = from.x;
		float y = from.y;
		foreach(KeyValuePair<int, Attractor> a in Attractors) {
			result += getForce(x, y, a.Value);
		}
		return result.normalized;
	}

	Vector2 getForce(float x, float y, Attractor attractor) {
		float magnitude = GetMagnitude(attractor, x, y);
		Vector2 direction = new Vector2(attractor.x - x, attractor.y - y);
		direction.Normalize();
		direction.x *= magnitude;
		direction.y *= magnitude;
		return direction;
	}

	float GetMagnitude(Attractor attractor, float x, float y) {
		if(attractor.force == 0) return 0;
		float v = SquareDistanceTo(attractor, x, y);
		//if(attractor.force < 0) v = -v;
		if(attractor.hardCutOff && Mathf.Abs(v) > attractor.force * attractor.force) return 0;
		return attractor.force / v;
	}

	float SquareDistanceTo(Attractor attractor, float x, float y) {
		float ex = (attractor.x - x);
		float ey = (attractor.y - y);
		return (ex * ex) + (ey * ey);
	}
}

