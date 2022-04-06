using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject followObject;
    [SerializeField]
    Vector3 offset;

    [SerializeField]
    float rotationSpeed = 1;
    [SerializeField]
    float followDistance = 0;

    Vector3 followObjectPrevPos, followObjectMoveDir;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        followObjectMoveDir = followObject.transform.position - followObjectPrevPos;
        followObjectMoveDir.Normalize();
        transform.position = followObject.transform.position - followObjectMoveDir * followDistance;

        followObjectPrevPos = followObject.transform.position;

        //transform.position = followObject.transform.position - followObject.transform.forward + offset;
        transform.rotation = Quaternion.Slerp(transform.rotation, followObject.transform.rotation, rotationSpeed * Time.deltaTime);
    }
}
