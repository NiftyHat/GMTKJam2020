using NiftyFramework.ScreenInput;
using UnityEngine;

public class RollingCharacterController : MonoBehaviour
{
    [SerializeField][NonNull] private ScreenInputController _inputController;

    [SerializeField] private Collider _collider;
    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _speedLossLerp = 0.05f;
    [SerializeField] private float _rotationRate = 1f;
    private float _currentSpeed = 0;
    
    public float _raycastMaxDistance = 100f;

    [SerializeField] private GameObject _firedProjectilePrefab;


    public Vector3 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        if (_inputController == null)
        {
            _inputController = FindObjectOfType<ScreenInputController>();
        }
        _inputController.OnPrimaryInputMoved += HandlePrimaryInputMoved;
        _inputController.OnPrimaryInputEnd += HandlePrimaryInputEnd;
        _inputController.OnSecondaryInputStart += HandleSecondaryInputStart;
        _inputController.OnSecondaryInputEnd += HandleSecondaryInputEnd;
    }

    private void HandleSecondaryInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputid)
    {
      
    }

    private void HandleSecondaryInputStart(Vector2 position, Vector2 delta, Ray screenpointray, float time, int inputid)
    {
        //throw new System.NotImplementedException();
    }

    private void HandlePrimaryInputMoved(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        RaycastHit hitInfo;
        bool isHit = Physics.Raycast(screenPointRay, out hitInfo, _raycastMaxDistance);
        if (isHit)
        {
            Vector3 targetLocation = new Vector3(hitInfo.point.x, gameObject.transform.position.y, hitInfo.point.z);
            Vector3 directionTowardsInput = (targetLocation - gameObject.transform.position).normalized;
            _moveDirection = directionTowardsInput;
            _currentSpeed = _baseSpeed;
        }
    }

    private void HandlePrimaryInputEnd(Vector2 position, Vector2 delta, Ray screenPointRay, float time, int inputId)
    {
        _moveDirection = Vector3.zero;
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity =  _moveDirection * (_currentSpeed * Time.deltaTime);
        transform.position += velocity;
        if (_currentSpeed > 0)
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _speedLossLerp);
        }
        
        Vector3 rbVelocity = velocity;
        Vector3 correctedAxes = new Vector3(rbVelocity.z, 0, -rbVelocity.x) * _rotationRate;
        _collider.transform.Rotate(correctedAxes, Space.World);

        if (_currentSpeed <= 0.05f)
        {
            _currentSpeed = 0;
        }
    }
}
