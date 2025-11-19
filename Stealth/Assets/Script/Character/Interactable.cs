using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	public class Interactable : MonoBehaviour
	{
		[SerializeField] bool oneTime;
		[SerializeField] UnityEvent onInteract;
		[Min(0)] public float maxInteractDistance = 0.0f;
		public UnityEvent onFocused;
		public UnityEvent onLoseFocus;

		public void Interact()
		{
			if(oneTime)
				enabled = false;
			onInteract?.Invoke();
		}
	}
}
