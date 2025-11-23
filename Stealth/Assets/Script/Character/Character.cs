using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
	#region Component references
	Transform body;
	[SerializeField] Transform head;
	CharacterController controller;
	#endregion

	#region Unity life cycle
	protected void Awake()
	{
		body = transform;
		controller = GetComponent<CharacterController>();
	}

	protected void OnEnable()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}
	protected void OnDisable()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	protected void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;
		if(bufferMovementInput.sqrMagnitude > controller.contactOffset)
		{
			Vector3 worldVelocity = body.localToWorldMatrix.MultiplyVector(bufferMovementInput).normalized * moveSpeed;
			MoveConstrained(body.position + worldVelocity * dt);
		}
	}
	#endregion

	#region Movement
	[Range(0, 10)] public float moveSpeed = 3.0f;
	protected Vector3 bufferMovementInput = default;

	void MoveConstrained(Vector3 targetPos)
	{
		if(!NavMesh.SamplePosition(targetPos, out var hit, controller.stepOffset, 1))
			return;
		targetPos = hit.position;

		controller.Move(targetPos - body.position);
	}

	[Range(0, 1)] public float orientSpeed = 1.0f;
	protected float Azimuth
	{
		get => body.eulerAngles.y;
		set
		{
			var euler = body.eulerAngles;
			euler.y = value;
			body.eulerAngles = euler;
		}
	}
	protected float Zenith
	{
		get => head.eulerAngles.x;
		set
		{
			var euler = head.eulerAngles;
			euler.x = value;
			head.eulerAngles = euler;
		}
	}
	#endregion
}
