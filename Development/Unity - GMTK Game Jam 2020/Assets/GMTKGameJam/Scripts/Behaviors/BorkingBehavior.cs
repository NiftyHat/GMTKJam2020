using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BorkingBehavior : MonoBehaviour
{
    [SerializeField] private VectorFieldDynamicUpdateBehavior _vectorFieldUpdate;
    [SerializeField] private AnimationCurve _repelRadiusCurve;
    [SerializeField] private float _repelRadiusMin;
    [SerializeField] private float _repelRadiusMax;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private bool _isBorking;
    private static readonly int NextBork = Animator.StringToHash("NextBork");
    private float _borkingTimeMax = 0f;
    private float _borkingTime = 0f;

    public void Fire(float maxBorkingTime)
    {
        _borkingTimeMax = maxBorkingTime;
        _borkingTime = 0f;
        _isBorking = true;
        StartCoroutine(RandomizeAnimation());
    }

    public IEnumerator RandomizeAnimation()
    {
        while (_isBorking && _borkingTime < _borkingTimeMax)
        {
            int nextBork = Random.Range(1, 3);
            float nextWait = Random.Range(0.3f, 0.7f);
            _animator.SetInteger(NextBork, nextBork);
            yield return new WaitForSeconds(nextWait);
        }
        yield return null;
    }

    public void Update()
    {
        if (_isBorking)
        {
            _borkingTime += Time.deltaTime;
            if (_borkingTime < _borkingTimeMax)
            {
                float normalizedTime = _borkingTime / _borkingTimeMax;
                float borkingCurveEval = _repelRadiusCurve.Evaluate(normalizedTime);
                _vectorFieldUpdate.SetRepelForce(_repelRadiusMin + ((_repelRadiusMax - _repelRadiusMin) * borkingCurveEval));
            }
            else
            {
                Stop();
            }
        }
    }

    public void Stop()
    {
        StopCoroutine(RandomizeAnimation());
        _animator.SetInteger(NextBork,0);
        _isBorking = false;
        _vectorFieldUpdate.SetRepelForce(0);
    }
}
