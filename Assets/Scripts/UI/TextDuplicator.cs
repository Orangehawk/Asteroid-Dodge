using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDuplicator : MonoBehaviour
{
    [SerializeField]
    Text target;

    Text text;


    void Awake()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target!= null)
		{
            text.text = target.text;
		}
    }
}
