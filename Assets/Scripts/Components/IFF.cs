using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFF : MonoBehaviour
{
    public enum ObjectType
	{
        Ship,
        Drone,
        Asteroid,
        Container,
        Object //Is this needed?
	}

    public enum Hostility
	{
        Friendly,
        Neutral,
        Hostile,
        Unkown
    }

    [SerializeField]
    public ObjectType objectType;
    [SerializeField]
    public Hostility hostility;


	void Start()
	{
        UIManager.instance.AddTarget(gameObject, this);
	}
}
