using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCatController : MonoBehaviour
{
    [SerializeField] private Texture _texture;
    [SerializeField] [NonNull] private SkinnedMeshRenderer _renderer;
    [SerializeField] [NonNull] private VectorFieldMovementBehavior _vectorFieldMovement;
    [SerializeField] [NonNull] private Rigidbody _rigidbody;
    [SerializeField] private AnimationCurve _randomWaitRange;
    [SerializeField] private float _waitRangeMultiply = 1.4f;
    [SerializeField] private float _waitRangeBase = 0.5f;

    [SerializeField] private AnimationCurve _randomVelocityRange;
    [SerializeField] private float _randomVelocityMultiplier = 1.0f;
    [SerializeField] private float _catSearchRadius = 3.0f;
    [SerializeField] private CatFeelings _feelings;
    [SerializeField] private RandomAudio _randomAudio;
    enum CatFeelings
    {
        Random,
        Likes,
        Hates,
        Neutral
    }

    public void Start()
    {
        if (_renderer != null)
        {
            _renderer.material.mainTexture = _texture;
        }

        if (_feelings == CatFeelings.Random)
        {
            float random = Random.Range(0f, 1f);
            if (random > 0.333)
            {
                _feelings = CatFeelings.Likes;
            }
            else if (random > 0.666)
            {
                _feelings = CatFeelings.Hates;
            }
            else
            {
                _feelings = CatFeelings.Neutral;
            }
        }
        StartCoroutine(RandomMill());
    }
    
    IEnumerator RandomMill()
    {
        while (true)
        {
            float waitRandom = Random.Range(0f, 1f);
            float nextWait = _randomWaitRange.Evaluate(waitRandom) * _waitRangeMultiply + _waitRangeBase;
            yield return new WaitForSeconds(nextWait);
            if (_vectorFieldMovement.VectorFieldVelocity.magnitude == 0)
            {
                DoRandomMove();
                DoRandomMeow();
            }
        }
    }

    private void DoRandomMeow()
    {
        if (_randomAudio != null)
        {
            float random = Random.Range(0f, 1f);
            if (random < 0.2f)
            {
                _randomAudio.Play();
            }
        }
    }

    public void DoRandomMove()
    {
        float velocityRandom = Random.Range(0f, 1f);
        float addedVelocity = _randomVelocityRange.Evaluate(velocityRandom) * _randomVelocityMultiplier;
        if (_feelings == CatFeelings.Likes || _feelings == CatFeelings.Hates)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _catSearchRadius );
            foreach (var item in colliders)
            {
                if (item.gameObject.GetComponent<RollingCatController>())
                {
                    Vector3 distanceFromCat = (transform.position - item.gameObject.transform.position).normalized;
                    if (_feelings == CatFeelings.Hates)
                    {
                        distanceFromCat = -distanceFromCat;
                    }
                    _rigidbody.AddForce(distanceFromCat * addedVelocity, ForceMode.Impulse);
                    return;
                }
            }
        }
        Vector2 randomCircleUnit = Random.insideUnitCircle;
        Vector3 randomDirection = new Vector3(randomCircleUnit.x, 0, randomCircleUnit.y);
        _rigidbody.AddForce(randomDirection * addedVelocity, ForceMode.Impulse);
    }
}
