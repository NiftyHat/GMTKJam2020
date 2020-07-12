using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;
    private bool _isLimitedByMax = true;
    private float _waitTime;
    private static int _maxClips = 3;
    private static int _currentClips = 0;

    // Start is called before the first frame update
    public bool Play()
    {
        if (_waitTime <= 0)
        {
            if (_isLimitedByMax && _currentClips >= _maxClips)
            {
                return false;
            }
            int index = Random.Range(0, _audioClips.Length - 1);
            var nextClip = _audioClips[index];
            _waitTime = nextClip.length * 0.8f;
            _audioSource.PlayOneShot(nextClip);
            _currentClips += 1;
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
        }
        else
        {
            if (_currentClips > 0)
            {
                _currentClips -= 1;
            }
            _waitTime = 0;
        }
    }
}
