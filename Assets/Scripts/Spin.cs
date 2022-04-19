using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField]
    Vector3 axis = new Vector3(0, 1, 0);
    [SerializeField]
    float speed = 0.1f;

    Rigidbody rb;
    [SerializeField]
    bool useRB;


	private void Awake()
	{
        useRB = TryGetComponent(out rb);
	}

	// Start is called before the first frame update
	void OnEnable()
    {
        //if (useRB)
            //rb.AddTorque(axis * speed, ForceMode.VelocityChange);
    }

	void OnDisable()
    { 
        if (useRB)
            rb.Sleep();
    }

	// Update is called once per frame
	void FixedUpdate()
    {
        if (rb == null)
            useRB = TryGetComponent(out rb);

        if (!useRB)
            transform.Rotate(axis, speed * Time.fixedDeltaTime);
        if (useRB)
            rb.MoveRotation(rb.rotation * Quaternion.Euler(axis * speed * Time.fixedDeltaTime));
    }
}
