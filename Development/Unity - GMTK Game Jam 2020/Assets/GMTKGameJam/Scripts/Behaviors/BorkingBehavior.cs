using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BorkingBehavior : MonoBehaviour
{
    [SerializeField] private VectorFieldDynamicUpdateBehavior _vectorFieldUpdate;
    [SerializeField] private AnimationCurve _repelRadiusCurve;
    [SerializeField] private float _repelRadiusMin;
    [SerializeField] private float _repelRadiusMax;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bounceVelocity;
    [SerializeField] private bool _isBorking;
    [SerializeField] [NonNull] private CountComponentsInRange _countComponents;

    [SerializeField] private BorkAccumulator _borkAccumulator;
        
    public event BorkAccumulator.DelegateOnChange OnBorkChange
    {
        add => _borkAccumulator.OnChange += value;
        remove => _borkAccumulator.OnChange -= value;
    }
    
    [Serializable]
    public class BorkAccumulator
    {
        [SerializeField] private float _max;
        [SerializeField] private float _cooldownTime;
        [SerializeField] private float _baseIncreaseSpeed;
        public bool isMax => _value >= _max;

        public float Cooldown => _cooldown;

        public delegate void DelegateOnChange(float value, float max, float cooldown);

        public delegate void DelegateOnMax(float cooldown);
        public event DelegateOnChange OnChange;
        public event DelegateOnMax OnMax;
        
        private float _value;

        private float _cooldown;

        public void Increase(float amount)
        {
            _value += amount * Time.deltaTime * _baseIncreaseSpeed;
            OnChange?.Invoke(_value, _max, _cooldown);
        }

        public void Update()
        {
            if (_cooldown <= 0)
            {
                if (_value > _max)
                {
                    _cooldown = _cooldownTime;
                    _value = _max;
                    OnMax?.Invoke(_cooldown);
                }
            }
            else
            {
                OnChange?.Invoke(_value, _max, _cooldown);
                _value = Mathf.Lerp(0, _max, _cooldown);
                _cooldown -= Time.deltaTime;
                if (_cooldown < 0)
                {
                    _cooldown = 0;
                }
            }
        }
    }
    
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
            if (_rigidbody != null && Math.Abs(_rigidbody.velocity.y) < 0.05f)
            {
                _rigidbody.AddForce(Vector3.up * (_bounceVelocity * Random.Range(0.3f,0.5f)), ForceMode.Impulse );
            }
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
        List<BorkTriggerBehavior> borkTriggerBehaviors = _countComponents.GetAll<BorkTriggerBehavior>();
        foreach (var item in borkTriggerBehaviors)
        {
            _borkAccumulator.Increase(item.Increase);
            if (_borkAccumulator.isMax)
            {
                Fire(_borkAccumulator.Cooldown);
            }
        }
        _borkAccumulator.Update();
    }

    public void Stop()
    {
        StopCoroutine(RandomizeAnimation());
        _animator.SetInteger(NextBork,0);
        _isBorking = false;
        _vectorFieldUpdate.SetRepelForce(0);
    }
}
