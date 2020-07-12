using UnityEngine;

public class RollingProjectileController : MonoBehaviour
{
    [SerializeField][NonNull] private Rigidbody _rigidbody;
    [SerializeField][NonNull] private Collider _collider;

    [SerializeField] private float _initialLifespan = 100.0f;
    [SerializeField] private AnimationCurve _scaleCurve;

    private float _lifeSpan;
    private Vector3 _initialScale;
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
        _lifeSpan -= Mathf.Max( Mathf.Clamp01(_rigidbody.velocity.magnitude)  * Time.deltaTime, Time.deltaTime);

        if (_lifeSpan <= 0)
        {
            _lifeSpan = 0;
            Destroy(gameObject);
        }
        
        float curveScaleValue = _scaleCurve.Evaluate(_lifeSpan / _initialLifespan);
        _collider.transform.localScale = _initialScale * curveScaleValue;
    }
    
}
