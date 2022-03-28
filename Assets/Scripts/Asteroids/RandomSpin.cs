using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpin : MonoBehaviour
{
    [SerializeField]
    bool randomiseRotation = true;
    [SerializeField]
    Vector3 rotationAxis = Vector3.zero;
    [SerializeField]
    float rotationSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        if(randomiseRotation)
            rotationAxis = new Vector3(Random.value, Random.value, Random.value);

        //Ensure all 3 components add up to 1
        float div = rotationAxis.x + rotationAxis.y + rotationAxis.z - 1;
        div /= 3;

        if(div > 0)
		{
            rotationAxis.x -= div;
            rotationAxis.y -= div;
            rotationAxis.z -= div;
        }
    }

    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    private void FixedUpdate()
	{
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
	}

	// Update is called once per frame
	void Update()
    {

    }
}
