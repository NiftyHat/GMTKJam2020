using System;
using System.Collections.Generic;
using UnityEngine;

public class VectorField
{
	// Width of the map, in Map Units
	public bool isLocked {get; private set;}
	public int Width { get; }
	public int Height { get; }
	public int GridSize {get; }

	public int WidthByGrid {get;}
	public int HeightByGrid {get;}

	public bool IsBlockedAt(int x, int y) { 
		x = Mathf.Clamp(x, 0, WidthByGrid);
		y = Mathf.Clamp(y, 0, HeightByGrid);
		return map[x, y];
	}

	private readonly List<Attractor> _attractors;
	private bool[,] map;
	private Dictionary<Attractor, Vector2[,]> _attractorMap;

	public VectorField(int width = 100, int height = 100, int gridSize = 10)
	{
		_attractors = new List<Attractor>();
		_attractorMap = new Dictionary<Attractor, Vector2[,]>();
		Width = width;
		Height = height;
		GridSize = gridSize;

		WidthByGrid = Width / GridSize;
		HeightByGrid = Height / GridSize;
		map = new bool[WidthByGrid, HeightByGrid];
	}

	public void Block(int x, int y) {
		map[x, y] = true;
		UpdateAttractorMap();
	}

	public void Unblock(int x, int y) {
		map[x, y] = false;
		UpdateAttractorMap();
	}

	public void UpdateForces() {
		List<Attractor> Atts = new List<Attractor>(_attractorMap.Keys);
		for(int i = 0; i <  Atts.Count; i++){
			Attractor att = Atts[i];
			if(att.dirty) {
				RecalculateAttractorGridForce(att);
				att.dirty = false;
			}
		}
	}

	public void AddAttractor(Attractor a) 
	{
		_attractors.Add(a);
		if(a.behaviour == AttractorBehaviour.AROUND_WALLS) {
			RecalculateAttractorGridForce(a);
		}
	}
	
	public void RemoveAttractor(Attractor a) 
	{
		_attractors.Remove(a);
		if(_attractorMap.ContainsKey(a)) _attractorMap.Remove(a);
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

	#region protected / private methods
	private void UpdateAttractorMap () {
		foreach(KeyValuePair<Attractor, Vector2[,]> kvp in _attractorMap){
			RecalculateAttractorGridForce(kvp.Key);
		}
	}

	protected void RecalculateAttractorGridForce(Attractor a)
	{
		bool hardCutOff = a.hardCutOff;
		int force = (int)(a.force / GridSize);
		int maxWeight = hardCutOff ? (int)(force * force) : WidthByGrid * HeightByGrid;
		
		Vector2[,] values = new Vector2[WidthByGrid, HeightByGrid];
		int[,] heatmap = new int[WidthByGrid, HeightByGrid];
		for(int x = 0; x < WidthByGrid; x++) { for(int y = 0; y < HeightByGrid; y++) {
			values[x, y] = Vector2.zero;
			heatmap[x, y] = maxWeight;
		}}

		// if force is 0 then everything is zero. Done
		if(maxWeight == 0) {
			_attractorMap[a] = values;
			return;
		}

		int sX = Mathf.Clamp(a.x / GridSize, 0, WidthByGrid - 1);
		int sY = Mathf.Clamp(a.y / GridSize, 0, HeightByGrid - 1);
		
		Point origin = new Point(sX, sY);
		heatmap[sX, sY] = 0;

		HashSet<Point> visitedPoints = new HashSet<Point>();
		Queue<Point> floodFill = new Queue<Point>();
		visitedPoints.Add(origin);
		floodFill.Enqueue(origin);

		int wPlus1 = WidthByGrid - 1;
		int hPlus1 = HeightByGrid - 1;
		
		// Create Heatmap (floodfill from point)
		while(floodFill.Count > 0) {
			Point current = floodFill.Dequeue();
			int x = current.x;
			int y = current.y;

			if(map[x, y]) continue;

			int newWeight = heatmap[x, y] + 1;
			if(newWeight > maxWeight) continue;

			// L
			if(x > 0 && heatmap[x - 1, y] > newWeight) {
				floodFill.Enqueue(new Point(x - 1, y));
				heatmap[x - 1, y] = newWeight;
			}
			// R
			if(x < wPlus1 && heatmap[x + 1, y] > newWeight) {
				floodFill.Enqueue(new Point(x + 1, y));
				heatmap[x + 1, y] = newWeight;
			}
			// D
			if(y > 0 && heatmap[x, y - 1] > newWeight) {
				floodFill.Enqueue(new Point(x, y - 1));
				heatmap[x, y - 1] = newWeight;
			}
			// U
			if(y < hPlus1 && heatmap[x, y + 1] > newWeight) {
				floodFill.Enqueue(new Point(x, y + 1));
				heatmap[x, y + 1] = newWeight;
			}
		}

		float floatMaxWeight = (float)maxWeight;
		for(int y = 0; y < HeightByGrid; y++) { for(int x = 0; x < WidthByGrid; x++) { 
			if(heatmap[x, y] == maxWeight) continue;

			int L = x == 0 ? maxWeight : heatmap[x - 1, y];
			int R = x == wPlus1 ? maxWeight : heatmap[x + 1, y];
			int D = y == 0 ? maxWeight : heatmap[x, y - 1];
			int U = y == hPlus1 ? maxWeight : heatmap[x , y + 1];

			values[x, y] = new Vector2(L - R, D - U).normalized * ((float)heatmap[x,y] / floatMaxWeight);
		}}

		_attractorMap[a] = values;
	}

	Vector2 getForce(float x, float y, Attractor attractor) {
		switch(attractor.behaviour) {
			
			// Attractor functions through walls (sound) and path directly
			case AttractorBehaviour.THROUGH_WALLS:
			float magnitude = GetMagnitude(attractor, x, y);
			Vector2 direction = new Vector2(attractor.x - x, attractor.y - y);
			direction.Normalize();
			direction.x *= magnitude;
			direction.y *= magnitude;
			return direction;

			// Attractor functions only if direct line can be drawn (sight) with no obstacles
			case AttractorBehaviour.LINE_OF_SIGHT:
			return new Vector2();

			// Attractor functions as pathfindable around walls (smell) pathing around obstacles
			case AttractorBehaviour.AROUND_WALLS:
			int cX = Mathf.Clamp((int)(x / GridSize), 0, WidthByGrid - 1);
			int cY = Mathf.Clamp((int)(y / GridSize), 0, HeightByGrid - 1);
			return _attractorMap[attractor][cX, cY];
		}

		return new Vector2();
	}

	float GetMagnitude(Attractor attractor, float x, float y) {
		float force = attractor.force;
		
		if(force == 0) return 0;
		float v = SquareDistanceTo(attractor, x, y);
		if(v == 0) return 0;

		float forceSquared = force * force;
		if(attractor.hardCutOff && Mathf.Abs(v) > forceSquared) return 0;
		return force / v;
	}

	float SquareDistanceTo(Attractor attractor, float x, float y) {
		float ex = (attractor.x - x);
		float ey = (attractor.y - y);
		return (ex * ex) + (ey * ey);
	}

	#endregion

	internal struct Point {
		public int x;
		public int y;
		public Point(int x, int y) { this.x = x; this.y = y;}

		public override bool Equals(object obj)
		{
			return obj is Point point &&
					 x == point.x &&
					 y == point.y;
		}
	}
	IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
	{
		bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
		if (steep)
		{
			int t;
			t = x0; // swap x0 and y0
			x0 = y0;
			y0 = t;
			t = x1; // swap x1 and y1
			x1 = y1;
			y1 = t;
		}
		if (x0 > x1)
		{
			int t;
			t = x0; // swap x0 and x1
			x0 = x1;
			x1 = t;
			t = y0; // swap y0 and y1
			y0 = y1;
			y1 = t;
		}
		int dx = x1 - x0;
		int dy = Math.Abs(y1 - y0);
		int error = dx / 2;
		int ystep = (y0 < y1) ? 1 : -1;
		int y = y0;
		for (int x = x0; x <= x1; x++)
		{
			yield return new Point((steep ? y : x), (steep ? x : y));
			error = error - dy;
			if (error < 0)
			{
					y += ystep;
					error += dx;
			}
		}
		yield break;
	}
}

