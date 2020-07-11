[System.Serializable]
public class Attractor {
	public int id;
	public int x;
	public int y;
	public float force;
	public bool hardCutOff = true;
	public bool dirty {get; set;}
	public AttractorBehaviour behaviour = AttractorBehaviour.THROUGH_WALLS;

	
}

public enum AttractorBehaviour {
	LINE_OF_SIGHT,
	AROUND_WALLS,
	THROUGH_WALLS,
	BOTH_LOS_AND_AROUND_WALLS
}