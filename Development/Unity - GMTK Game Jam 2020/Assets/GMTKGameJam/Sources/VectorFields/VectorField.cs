using System.Collections.Generic;
using UnityEngine;

public class VectorField
{
	// Width of the map, in Map Units
	public bool isLocked {get; private set;}
	private readonly List<Attractor> _attractors;
	private bool[,] map;

	public VectorField()
	{
		_attractors = new List<Attractor>();
	}

	public void AddAttractor(Attractor a) 
	{
		_attractors.Add(a);
	}
	
	public void RemoveAttractor(Attractor a) 
	{
		_attractors.Remove(a);
	}

	public Vector2 GetPower(Vector2 from) {
		Vector2 result = new Vector2();
		float x = from.x;
		float y = from.y;
		foreach(Attractor attractor in _attractors) {
			result += getForce(x, y, attractor);
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

