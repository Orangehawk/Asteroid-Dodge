using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class AsteroidHinge : MonoBehaviour
{
    [SerializeField]
    Vector3 anchor;
    [SerializeField]
    Vector3 axis;

    [SerializeField]
    float massScale = 100;
    [SerializeField]
    float motorForce = 500;
    [SerializeField]
    float motorTargetVelocity = 50;

    Rigidbody rb;
    HingeJoint hinge;
    JointMotor motor;

    bool stop = false;


	void Awake()
	{
        rb = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();
        hinge.massScale = massScale;

        motor = new JointMotor();
        motor.force = motorForce;
        motor.targetVelocity = motorTargetVelocity;

        hinge.motor = motor;
        hinge.useMotor = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        hinge.anchor = transform.InverseTransformPoint(anchor);
        hinge.axis = transform.InverseTransformDirection(axis);
    }

    // Update is called once per frame
    void Update()
    {
        if(stop == false)
		{
            if (rb.velocity.magnitude > motor.targetVelocity)
            {
                hinge.useMotor = false;
                stop = true;
            }
		}
    }
}
