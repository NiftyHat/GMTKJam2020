using UnityEngine;

public class VectorFieldMovementBehavior : MonoBehaviour
{
    [SerializeField] private VectorFieldController _vectorFieldController;
    private VectorField _vectorField;
    private Rigidbody _rigidbody;
    protected float _speed = 10;
    void Start()
    {
        _rigidbody = transform.GetComponent<Rigidbody>();
        GetVectorField();
    }

    // Update is called once per frame
    void Update()
    {
        if (_vectorField != null)
        {
            Vector2 positionVec2 = new Vector2(transform.position.x, transform.position.z);
            Vector2 vectorFieldPower = _vectorField.GetPower(positionVec2) * _speed;
            // Lerp from the Current Velocity to the Vector Grid's velocity
            Vector3 desiredVelocity = new Vector3(vectorFieldPower.x, 0, vectorFieldPower.y);
            Vector3 currentVelocity = _rigidbody.velocity;
            _rigidbody.velocity = Vector3.Lerp(currentVelocity, desiredVelocity, 0.01f);
        }
        else
        {
            GetVectorField();
        }
    }

    private void GetVectorField()
    {
        if (_vectorFieldController == null)
        {
            _vectorFieldController = FindObjectOfType<VectorFieldController>();
        }
        if (_vectorFieldController != null)
        {
            _vectorField = _vectorFieldController.VectorField;
        }
    }
}
