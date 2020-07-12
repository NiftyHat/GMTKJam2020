using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private bool _isFinished = false;
    [SerializeField] private float _levelEndDelay;

    protected List<ScoreLocationController> _scoreLocations = new List<ScoreLocationController>();
    protected float _levelEndCountdown;

    public void TrackScoreLocation(ScoreLocationController controller)
    {
        if (!_scoreLocations.Contains(controller))
        {
            _scoreLocations.Add(controller);
        }
    }

    private void Update()
    {
        if (!_isFinished && _scoreLocations.Count > 0)
        {
            foreach (var item in _scoreLocations)
            {
                if (!item.HasRequired)
                {
                    _levelEndCountdown = _levelEndDelay;
                    return;
                }
            }

            _levelEndCountdown -= Time.deltaTime;
            if (_levelEndCountdown <= 0)
            {
                Finish();
            }
        }
    }
    public void Finish()
    {
        if (!_isFinished)
        {
            int firstLevelScene = 1;
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = firstLevelScene;
            }
            SceneManager.LoadScene(nextSceneIndex);
            _isFinished = true;
        }
    }
}