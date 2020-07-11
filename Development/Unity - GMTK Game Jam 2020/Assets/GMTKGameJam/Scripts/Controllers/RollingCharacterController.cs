using System;
using System.Collections.Generic;
using NiftyFramework.ScreenInput;
using UnityEngine;

public class RollingCharacterController : MonoBehaviour
{
    [SerializeField][NonNull] private ScreenInputController _inputController;
    
    [SerializeField] private float _impulseAmount = 10f;
    [SerializeField][NonNull] private Rigidbody _rigidbody;
    [SerializeField] [NonNull] private ProjectileFireController _fireController;
    [SerializeField][NonNull] private BorkingBehavior _borkingBehavior;
    [SerializeField] [NonNull] private CountComponentsInRange _countComponents;
    [SerializeField] private BorkAccumulator _borkAccumulator;
    
    public float _raycastMaxDistance = 100f;
    public float _fireControllerDistance = 2f;
    private bool _isPrimaryInputDown;

    public event BorkAccumulator.DelegateOnChange OnBorkChange
    {
        add => _borkAccumulator.OnChange += value;
        remove => _borkAccumulator.OnChange -= value;
    }
    protected Vector3 _moveDirection;

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
    // Start is called before the first frame update
    void Start()
    {
        if (_inputController == null)
        {
            _inputController = FindObjectOfType<ScreenInputController>();
        }
        _inputController.OnPrimaryInputMoved += HandlePrimaryInputMoved;
        _inputController.OnPrimaryInputEnd += HandlePrimaryInputEnd;
        _inputController.OnSecondaryInputMoved += HandleSecondaryInputMoved;
        _inputController.OnSecondaryInputEnd += HandleSecondaryInputEnd;
    }

    private void HandleSecondaryInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        float clampedTime = Mathf.Clamp(time, 0, 1f);
        _fireController.Fire(clampedTime);
    }

    private void HandleSecondaryInputMoved(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        bool isHit = Physics.Raycast(screenPointRay, out var hitInfo, _raycastMaxDistance);
        if (isHit)
        {
            Vector3 physicsPosition = _rigidbody.transform.position;
            Vector3 targetLocation = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
            Vector3 currentLocation= new Vector3(physicsPosition.x, 0, physicsPosition.z);
            Vector3 directionTowardsInput = (targetLocation - currentLocation).normalized;
            Vector3 fireControllerPosition = physicsPosition + directionTowardsInput * _fireControllerDistance;
            _fireController.transform.position = fireControllerPosition;
            _fireController.transform.LookAt(targetLocation);
        }
    }

    private void HandlePrimaryInputMoved(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        _isPrimaryInputDown = false;
        bool isHit = Physics.Raycast(screenPointRay, out var hitInfo, _raycastMaxDistance);
        if (isHit)
        {
            Vector3 physicsPosition = _rigidbody.transform.position;
            Vector3 targetLocation = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
            Vector3 currentLocation= new Vector3(physicsPosition.x, 0, physicsPosition.z);
            Vector3 directionTowardsInput = (targetLocation - currentLocation).normalized;
            _moveDirection = directionTowardsInput;
            _isPrimaryInputDown = true;
        }
    }

    private void HandlePrimaryInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        _isPrimaryInputDown = false;
        _moveDirection = Vector3.zero;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isPrimaryInputDown)
        {
            _rigidbody.AddForce(_moveDirection * _impulseAmount, ForceMode.Force);
        }
        _borkingBehavior.transform.position = _rigidbody.position;

        List<BorkTriggerBehavior> borkTriggerBehaviors = _countComponents.GetAll<BorkTriggerBehavior>();
        foreach (var item in borkTriggerBehaviors)
        {
            _borkAccumulator.Increase(item.Increase);
            if (_borkAccumulator.isMax)
            {
                _borkingBehavior.Fire(_borkAccumulator.Cooldown);
            }
        }
        _borkAccumulator.Update();
    }
}
