using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject followObject;
    [SerializeField]
    Transform followPosition;
    [SerializeField]
    float heightOffset;

    [SerializeField]
    float rotationSpeed = 1;
    [SerializeField]
    float followDistance = 0;

    Vector3 followObjectPrevPos, followObjectMoveDir;
    Vector3 offset;


	private void OnValidate()
	{
        offset = new Vector3(0, heightOffset, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, heightOffset, 0);
    }

    void Follow()
	{
        //transform.position = followObject.transform.position - followObject.transform.forward + offset;


        //followObjectMoveDir = followObject.transform.position - followObjectPrevPos;
        //followObjectMoveDir.Normalize();
        //transform.position = followObject.transform.position - followObjectMoveDir * followDistance;
        //followObjectPrevPos = followObject.transform.position;


        transform.position = followPosition.position;
    }

    void Rotate()
	{
        transform.rotation = Quaternion.Slerp(transform.rotation, followObject.transform.rotation, rotationSpeed * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
        Rotate();
    }
}
