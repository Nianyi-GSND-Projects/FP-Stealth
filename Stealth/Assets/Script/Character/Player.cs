using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
	public class Player : MonoBehaviour
	{
		#region Unity life cycle
		void OnEnable()
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		void OnDisable()
		{
			Cursor.lockState = CursorLockMode.None;
		}

		void Update()
		{
			UpdateFocusedInteraction();
		}
		void FixedUpdate()
		{
			float dt = Time.fixedDeltaTime;
			if(bufferMovementInput.sqrMagnitude > 0.1f)
			{
				Vector3 worldVelocity = body.localToWorldMatrix.MultiplyVector(bufferMovementInput).normalized * moveSpeed;
				controller.SimpleMove(worldVelocity);
			}
		}
		#endregion

		#region Component references
		[SerializeField] Transform body;
		[SerializeField] Transform eye;
		[SerializeField] CharacterController controller;
		#endregion

		#region Control
		[Range(0, 10)] public float moveSpeed = 3.0f;
		Vector3 bufferMovementInput = default;

		[Range(0, 1)] public float orientSpeed = 1.0f;
		float Azimuth
		{
			get => body.eulerAngles.y;
			set
			{
				var euler = body.eulerAngles;
				euler.y = value;
				body.eulerAngles = euler;
			}
		}
		float Zenith
		{
			get => eye.eulerAngles.x;
			set
			{
				var euler = eye.eulerAngles;
				euler.x = value;
				eye.eulerAngles = euler;
			}
		}

		protected void OnMove(InputValue value)
		{
			var raw = value.Get<Vector2>();
			bufferMovementInput = new Vector3(raw.x, 0, raw.y);
		}

		protected void OnLook(InputValue value)
		{
			if(!enabled)
				return;

			var raw = value.Get<Vector2>();
			Azimuth = Azimuth + raw.x * orientSpeed;
			float zenith = Zenith + raw.y * orientSpeed;
			if(zenith < 0)
				zenith += 360;
			if(zenith < 180)
				zenith = Mathf.Clamp(zenith, 0, 90);
			else
				zenith = Mathf.Clamp(zenith, 270, 360);
			Zenith = zenith;
		}
		#endregion

		#region Interaction
		Interactable focusedInteraction = null;
		Interactable FocusedInteraction
		{
			get => focusedInteraction;
			set
			{
				if(value != null && !value.enabled)
					value = null;
				if(value == focusedInteraction)
					return;
				if(focusedInteraction)
					focusedInteraction.onLoseFocus?.Invoke();
				focusedInteraction = value;
				if(focusedInteraction)
					focusedInteraction.onFocused?.Invoke();
			}
		}
		[SerializeField][Min(0)] float maxInteractDistance = 3.0f;

		void UpdateFocusedInteraction()
		{
			var hits = Physics.RaycastAll(eye.position, eye.forward, maxInteractDistance);
			bool flag = false;
			foreach(var hit in hits)
			{
				Interactable result = hit.transform.gameObject.GetComponentInParent<Interactable>();
				if(result)
				{
					if(hit.distance > maxInteractDistance)
						result = null;
					if(result.maxInteractDistance > 0 && hit.distance > result.maxInteractDistance)
						result = null;
				}
				if(result != null)
				{
					FocusedInteraction = result;
					return;
				}
			}
			if(!flag)
				FocusedInteraction = null;
		}

		protected void OnInteract()
		{
			if(!FocusedInteraction)
				return;
			FocusedInteraction.Interact();
		}
		#endregion
	}
}
