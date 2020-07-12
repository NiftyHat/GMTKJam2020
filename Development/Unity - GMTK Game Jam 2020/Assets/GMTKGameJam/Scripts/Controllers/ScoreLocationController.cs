using TMPro;
using UnityEngine;

public class ScoreLocationController : MonoBehaviour
{
    
    [SerializeField] protected Attractor _attractor;
    [SerializeField] protected TextMeshPro _scoreText;
    [SerializeField] protected TextMeshPro _labelText;

    [SerializeField] [NonNull] protected CountComponentsInRange _countComponents;
    [SerializeField] private float _radius = 5;
    [SerializeField] [NonNull] private FriendBehaviour _friendBehaviour;
    
    private int _currentScoringItems = 0;
    private int _lastScoringItems = -1;
    [SerializeField] private int _requiredScoringItems = 2;

	 [SerializeField] private  float timeRequired = 3.0f;
	 private float currentTimeRemaining;
	 public bool levelComplete = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        _attractor = new Attractor();
        _attractor.force = _radius;
        _attractor.hardCutOff = true;
        _attractor.x = (int) position.x;
        _attractor.y = (int) position.y;
        _countComponents.SetRadius(_radius);

		  currentTimeRemaining = timeRequired;
    }

    private void UpdateScore()
    {
		_currentScoringItems = _countComponents.Count<ScoringBehavior>();



		if (_lastScoringItems != _currentScoringItems)
		{
		float normalizedScore = (float)_currentScoringItems / (float)_requiredScoringItems;
		_friendBehaviour.AmountOfCat = normalizedScore;

		if(_currentScoringItems < _requiredScoringItems) {
			_labelText.text = "kitties needed";
			_scoreText.gameObject.SetActive(true);
		} else if (timeRequired > 0) {
			_labelText.text = "hold...";
			_scoreText.gameObject.SetActive(false);
		}

			_lastScoringItems = _currentScoringItems;
			if (_scoreText != null)
			{
				_scoreText.text = Mathf.Max(_requiredScoringItems - _currentScoringItems, 0).ToString();
			}
		}
    }
    // Update is called once per frame
    void Update()
    {
		 if(levelComplete) return;

        UpdateScore();

		  if(_requiredScoringItems - _lastScoringItems == 0) { 
			  currentTimeRemaining -= Time.deltaTime;
			  if(currentTimeRemaining <= 0) {
				  levelComplete = true;
				  _labelText.text = "level complete";
			  }
			}
		  else {
			  currentTimeRemaining += Time.deltaTime;
			  if(currentTimeRemaining > timeRequired) currentTimeRemaining = timeRequired;
		  }
    }
}
