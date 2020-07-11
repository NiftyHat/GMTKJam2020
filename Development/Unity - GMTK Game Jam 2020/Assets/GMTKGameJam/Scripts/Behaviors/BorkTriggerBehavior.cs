using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorkTriggerBehavior : MonoBehaviour
{
    [SerializeField] protected float _increase = 0.2f;

    public float Increase => _increase;
}
