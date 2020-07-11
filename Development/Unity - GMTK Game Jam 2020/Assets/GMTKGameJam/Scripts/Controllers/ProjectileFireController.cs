using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireController : MonoBehaviour
{
    
    [SerializeField] private GameObject _firedProjectilePrefab;

    [SerializeField] private float forceMin = 5f;
    [SerializeField] private float forceMax = 50f;
    
    // Start is called before the first frame update
    public void Fire(float perc)
    {
        GameObject projectile= Instantiate(_firedProjectilePrefab, transform.position, transform.rotation);
        RollingProjectileController controller = projectile.GetComponent<RollingProjectileController>();

        float force = forceMin + ((forceMax - forceMin) * perc);
        controller.Fire(transform.transform.forward, force);
    }
}
