using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
	#region Component references
	protected Transform body;
	[SerializeField] protected Transform head;
	protected CharacterController controller;
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

		controller.SimpleMove(DesiredVelocity);
	}

#if UNITY_EDITOR
	protected void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(body.position, body.position + DesiredVelocity);
	}
#endif
	#endregion

	#region Movement
	[SerializeField, Range(0, 10)] protected float moveSpeed = 3.0f;
	public Vector3 DesiredVelocity { get; set; } = default;
	public float MoveSpeed
	{
		get => moveSpeed;
		set => moveSpeed = value;
	}
	public bool IsWalking => DesiredVelocity.sqrMagnitude > .1f;

	[SerializeField, Min(0f)] protected float orientSpeed = 1.0f;

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
