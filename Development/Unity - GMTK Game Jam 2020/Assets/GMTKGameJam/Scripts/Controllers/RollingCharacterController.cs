using NiftyFramework.ScreenInput;
using UnityEngine;

public class RollingCharacterController : MonoBehaviour
{
    [SerializeField][NonNull] private ScreenInputController _inputController;
    
    [SerializeField] private float _impulseAmount = 10f;
    [SerializeField][NonNull] private Rigidbody _rigidbody;
    [SerializeField] [NonNull] private ProjectileFireController _fireController;
    public float _raycastMaxDistance = 100f;
    public float _fireControllerDistance = 2f;
    protected bool _isPrimaryInputDown;

    protected Vector3 _moveDirection;
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
        float clampedTime = Mathf.Clamp(time, 0, 2f);
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
        //Vector3 velocity =  _moveDirection * (_currentSpeed * Time.deltaTime);
        if (_isPrimaryInputDown)
        {
            _rigidbody.AddForce(_moveDirection * _impulseAmount, ForceMode.Force);
        }
    }
}
