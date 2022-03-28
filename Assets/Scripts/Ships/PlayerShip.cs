using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
	static public PlayerShip instance;

	public bool allowControl = true;

	[SerializeField]
	float moveSpeed = 80;
	[SerializeField]
	float boostMultiplier = 1.6f;
	[SerializeField]
	float turnSpeed = 80;
	[SerializeField]
	float rollSpeed = 10.0f;
	[SerializeField]
	float minDamageSpeed = 30f; //Hitting something faster then this will cause damage to your ship

	Rigidbody rb;
	Light frontLight;
	Vector3 movementInput;
	Vector3 rotationInput;

	bool intertialDampners;
	float defaultDrag;
	float defaultAngularDrag;


	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
			return;
		}
		else
			instance = this;

		rb = GetComponent<Rigidbody>();
		frontLight = GetComponentInChildren<Light>();
		movementInput = new Vector3();
		rotationInput = new Vector3();
		intertialDampners = true;
		defaultDrag = rb.drag;
		defaultAngularDrag = rb.angularDrag;
	}

	public void SetAllowControl(bool allow)
	{
		allowControl = allow;
	}

	public void ToggleAllowControl()
	{
		allowControl = !allowControl;
		Debug.Log($"Set allow control to {allowControl}");
	}

	void HandleInput()
	{
		//DEBUG
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			ToggleAllowControl();
		}

		if (allowControl)
		{
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
			{
				GameManager.instance.TogglePause();
			}

			if (GameManager.instance.GetGameState() != GameManager.GameState.Paused)
			{
				if (Input.GetKey(KeyCode.W))
				{
					if (Input.GetKey(KeyCode.LeftShift))
						movementInput.z = moveSpeed * boostMultiplier;
					else
						movementInput.z = moveSpeed;
				}
				else if (Input.GetKey(KeyCode.S))
				{
					movementInput.z = -moveSpeed;
				}
				else
				{
					movementInput.z = 0;
				}

				if (Input.GetKey(KeyCode.D))
				{
					movementInput.x = moveSpeed;
				}
				else if (Input.GetKey(KeyCode.A))
				{
					movementInput.x = -moveSpeed;
				}
				else
				{
					movementInput.x = 0;
				}

				if (Input.GetKey(KeyCode.Space))
				{
					movementInput.y = moveSpeed;
				}
				else if (Input.GetKey(KeyCode.C))
				{
					movementInput.y = -moveSpeed;
				}
				else
				{
					movementInput.y = 0;
				}

				if (Input.GetKey(KeyCode.Q))
				{
					rotationInput.z = rollSpeed;
				}
				else if (Input.GetKey(KeyCode.E))
				{
					rotationInput.z = -rollSpeed;
				}
				else
				{
					rotationInput.z = 0;
				}

				if (Input.GetMouseButton(0))
				{
					
				}

				if (Input.GetMouseButton(1))
				{
					
				}

				if (Input.GetKeyDown(KeyCode.F))
				{
					
				}

				if (Input.GetKeyDown(KeyCode.R))
				{
					
				}

				if (Input.GetKeyDown(KeyCode.L))
				{
					frontLight.enabled = !frontLight.enabled;
				}

				if (Input.GetKeyDown(KeyCode.T))
				{
					
				}

				if (Input.GetKeyDown(KeyCode.G))
				{
					
				}

				if (Input.GetKeyDown(KeyCode.Z))
				{
					intertialDampners = !intertialDampners;
					if (intertialDampners)
					{
						rb.drag = defaultDrag;
						rb.angularDrag = defaultAngularDrag;
					}
					else
					{
						rb.drag = 0;
						rb.angularDrag = 0;
					}
				}

				rotationInput.x = -Input.GetAxis("Mouse Y") * turnSpeed;
				rotationInput.y = Input.GetAxis("Mouse X") * turnSpeed;
			}
		}
		else
		{
			movementInput = Vector3.zero;
			rotationInput = Vector3.zero;
		}
	}

	void ApplyForce()
	{
		rb.AddRelativeForce(movementInput);
	}

	void ApplyRotation()
	{
		rb.AddRelativeTorque(rotationInput * Time.deltaTime);
	}

	public void Kill()
	{
		allowControl = false; //TEMP
		rb.drag = 0;
		rb.angularDrag = 0;
	}

	
	private void OnCollisionEnter(Collision collision)
	{
		if (Vector3.Project(collision.relativeVelocity, collision.GetContact(0).normal).magnitude > minDamageSpeed)
		{
			Kill();
			GameManager.instance.GameOver();
		}
	}

	// Update is called once per frame
	void Update()
	{
		HandleInput();
		ApplyRotation();
	}

	void FixedUpdate()
	{
		ApplyForce();
	}
}
