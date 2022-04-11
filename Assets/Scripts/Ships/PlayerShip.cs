using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
	static public PlayerShip instance;

	[SerializeField]
	float moveSpeed = 80;
	[SerializeField]
	float boostMultiplier = 1.6f;
	[SerializeField]
	float turnSpeed = 80; //4000
	[SerializeField]
	float rollSpeed = 10.0f; //1000
	[SerializeField]
	float minDamageSpeed = 30f; //Hitting something faster then this will cause damage to your ship

	Rigidbody rb;
	Vector3 movementInput;
	Vector3 rotationInput;

	bool intertialDampners;
	bool allowControl = true;
	float defaultDrag;
	float defaultAngularDrag;
	int lives = 3;


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

		movementInput = new Vector3();
		rotationInput = new Vector3();
		intertialDampners = true;
		defaultDrag = rb.drag;
		defaultAngularDrag = rb.angularDrag;
	}

	private void Start()
	{
		UIManager.instance.UpdateLives(lives);
	}

	public void SetAllowControl(bool allow)
	{
		allowControl = allow;
	}

	public void ToggleAllowControl()
	{
		allowControl = !allowControl;
		Logger.Log($"Set allow control to {allowControl}");
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
			//Toggle pause menu
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
			{
				GameManager.instance.TogglePause();
			}

			//While game not paused
			if (GameManager.instance.GetGameState() != GameManager.GameState.Paused)
			{
				//Move forward and backward
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

				//Move right and left
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

				//Move up and down
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

				//Roll left and right
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

				//DEBUG Change target framerate between 144 and 60
				if (Input.GetKeyDown(KeyCode.T))
				{
					GameManager.DebugToggleFrameRate();
					Logger.Log($"Set target framerate to {Application.targetFrameRate}");
				}

				//DEBUG? disable drag/speed limiter
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

	public int GetLives()
	{
		return lives;
	}

	public void Respawn()
	{
		SetAllowControl(true);
		transform.position = Vector3.zero;
		rb.Sleep();
	}

	public void Kill()
	{
		if (lives-- > 0)
		{
			UIManager.instance.UpdateLives(lives);
			Respawn();
		}
		else
		{
			SetAllowControl(false);
			GameManager.instance.GameOver();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (Vector3.Project(collision.relativeVelocity, collision.GetContact(0).normal).magnitude > minDamageSpeed)
		{
			Kill();
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
