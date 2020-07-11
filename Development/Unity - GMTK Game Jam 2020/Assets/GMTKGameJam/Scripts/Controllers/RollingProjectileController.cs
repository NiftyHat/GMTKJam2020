using UnityEngine;

public class RollingProjectileController : MonoBehaviour
{

    [SerializeField] private float _initialImpulseMin = 10f;
    [SerializeField][NonNull] private Rigidbody _rigidbody;
    [SerializeField][NonNull] private Collider _collider;

    [SerializeField] private float _initialLifespan = 100.0f;

    [SerializeField] private AnimationCurve _scaleCurve;

    protected float _lifeSpan;

    protected Vector3 _initialScale;
    // Start is called before the first frame update
    void Start()
    {
        _lifeSpan = _initialLifespan;
        _initialScale = _collider.transform.lossyScale;
    }

    // Update is called once per frame
    public void Fire(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force, ForceMode.VelocityChange);
    }

    void Update()
    {
        if (_rigidbody.velocity.magnitude > 0.1f)
        {
            _lifeSpan -= Mathf.Clamp01(_rigidbody.velocity.magnitude) * Time.deltaTime;
            //_lifespan -= Mathf.Clamp(_rigidbody.velocity.magnitude * 0.1f * Time.deltaTime, _lifespanMaxLoss, _lifespanMinLoss);
        }

        if (_lifeSpan < 0)
        {
            _lifeSpan = 0;
            GameObject.Destroy(this);
        }
        
        float curveScaleValue = _scaleCurve.Evaluate(_lifeSpan / _initialLifespan);
        _collider.transform.localScale = _initialScale * curveScaleValue;
    }
    
}
