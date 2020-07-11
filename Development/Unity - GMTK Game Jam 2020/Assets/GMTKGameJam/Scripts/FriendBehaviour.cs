using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendBehaviour : MonoBehaviour
{
	public float AmountOfCat = 0;
	public Animator animatorRef;
	protected Camera m_Camera;
	
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
		 animatorRef.SetFloat("AmountOfCat", AmountOfCat);
    }
	 void LateUpdate() {
		 /*
		 transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);*/
	 }
}
