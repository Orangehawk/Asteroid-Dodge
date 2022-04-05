using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpaceDustController : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    ParticleSystem particles;

    [SerializeField]
    float activeSpeed = 5;

    ParticleSystem.EmissionModule emission;


    // Start is called before the first frame update
    void Start()
    {
        if(rb == null)
            if(!TryGetComponent(out rb))
                Debug.LogError("SpaceDustController: Failed to find a Rigidbody component");

        if(particles == null)
            if(!TryGetComponent(out particles))
                Debug.LogError("SpaceDustController: Failed to find a ParticleSystem component");

        emission = particles.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if(Math.Round(rb.velocity.magnitude, 2) >= activeSpeed)
		{
            emission.enabled = true;
		}
        else
		{
            emission.enabled = false;
		}
    }
}
