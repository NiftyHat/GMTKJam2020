using UnityEngine;
using Random = UnityEngine.Random;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] _audioClips;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private float _waitTime;
    // Start is called before the first frame update
    public bool Play(AudioSource targetSource)
    {
        if (_waitTime <= 0)
        {
            int index = Random.Range(0, _audioClips.Length - 1);
            var nextClip = _audioClips[index];
            _waitTime = nextClip.length;
            _audioSource.clip = nextClip;
        }
    }

    public void Update()
    {
        if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
        }
    }
}
