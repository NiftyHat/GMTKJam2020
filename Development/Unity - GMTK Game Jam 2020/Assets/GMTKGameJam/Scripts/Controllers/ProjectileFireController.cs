using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireController : MonoBehaviour
{
    [SerializeField] private GameObject _firedProjectilePrefab;
    
    [SerializeField] private AnimationCurve _powerCurve;
    [SerializeField] private float _powerMultiplier = 1.0f;
    
    // Start is called before the first frame update
    public void Fire(float powerPercentage)
    {
        GameObject projectile= Instantiate(_firedProjectilePrefab, transform.position, transform.rotation);
        RollingProjectileController controller = projectile.GetComponent<RollingProjectileController>();

        float force = _powerCurve.Evaluate(powerPercentage) * _powerMultiplier;
        controller.Fire(transform.transform.forward, force);
    }
}
