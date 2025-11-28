using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour
{
	#region Component references
	[SerializeField] protected Transform head;
	public CharacterController Controller { get; private set; }
	#endregion

	#region Unity life cycle
	protected void Awake()
	{
		Controller = GetComponent<CharacterController>();
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

		Controller.SimpleMove(DesiredVelocity);
	}

#if UNITY_EDITOR
	protected void OnDrawGizmos()
	{
		if(!Application.isPlaying)
			return;

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + DesiredVelocity);
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
		get => transform.eulerAngles.y;
		set
		{
			var euler = transform.eulerAngles;
			euler.y = value;
			transform.eulerAngles = euler;
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
