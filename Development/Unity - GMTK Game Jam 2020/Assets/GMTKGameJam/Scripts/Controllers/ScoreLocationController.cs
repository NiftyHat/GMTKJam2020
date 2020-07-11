using TMPro;
using UnityEngine;

public class ScoreLocationController : MonoBehaviour
{
    
    [SerializeField] protected Attractor _attractor;
    [SerializeField] protected TextMeshPro _scoreText;

    [SerializeField] protected int _radius = 5;
    private int _currentScoringItems = 0;
    private int _lastScoringItems = -1;
    private int _requiredScoringItems = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        _attractor = new Attractor();
        _attractor.force = _radius;
        _attractor.hardCutOff = true;
        _attractor.x = (int) position.x;
        _attractor.y = (int) position.y;
    }
    
    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(position.x, position.y, position.z), _radius);
    }
    
    private void UpdateScore()
    {
        Collider[] overlappingCollides = Physics.OverlapSphere(transform.position, _radius);
        _currentScoringItems = 0;
        foreach (var item in overlappingCollides)
        { 
            var scoringBehavior = item.GetComponent<ScoringBehavior>();
            if (scoringBehavior != null)
            {
                _currentScoringItems += scoringBehavior.pointValue;
            }
        }

        if (_lastScoringItems != _currentScoringItems)
        {
            _lastScoringItems = _currentScoringItems;
            if (_scoreText != null)
            {
                _scoreText.text = (_requiredScoringItems - _currentScoringItems).ToString();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        UpdateScore();
    }
}
