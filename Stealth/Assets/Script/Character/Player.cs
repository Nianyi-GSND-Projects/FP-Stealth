using UnityEngine;
using UnityEngine.InputSystem;
using Nianyi.UnityPack;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
public class Player : Character
{
	[SerializeField] Selector selector;

	protected new void FixedUpdate()
	{
		DesiredVelocity = transform.TransformVector(movementInput * MoveSpeed);
		bool moving = DesiredVelocity.sqrMagnitude > 0;
		if(!moving)
		{
			if(stepCoroutine != null)
			{
				StopCoroutine(stepCoroutine);
				stepCoroutine = null;
			}
		}
		else
		{
			if(stepCoroutine == null)
				stepCoroutine = StartCoroutine(StepCoroutine());
		}

			base.FixedUpdate();
	}

	Vector3 movementInput;
	protected void OnMove(InputValue value)
	{
		var raw = value.Get<Vector2>();
		movementInput = new Vector3(raw.x, 0, raw.y);
	}

	protected void OnLook(InputValue value)
	{
		var raw = value.Get<Vector2>();
		raw *= 360f * orientSpeed / Screen.width;

		Azimuth = Azimuth + raw.x;
		float zenith = Zenith + raw.y;
		if(zenith < 0)
			zenith += 360;
		if(zenith < 180)
			zenith = Mathf.Clamp(zenith, 0, 90);
		else
			zenith = Mathf.Clamp(zenith, 270, 360);
		Zenith = zenith;
	}

	protected void OnInteract()
	{
		foreach(var selected in selector.Selected)
			selected.SendMessage(nameof(IInteractable.OnInteract), SendMessageOptions.DontRequireReceiver);
	}

	[SerializeField] List<AudioClip> stepAudios;
	[SerializeField] AudioSource stepSource;
	[SerializeField, Min(0)] float stepInterval = 0.3f;
	Coroutine stepCoroutine;

	IEnumerator StepCoroutine()
	{
		for(; ; )
		{
			var step = stepAudios[Mathf.FloorToInt(Random.value * stepAudios.Count)];
			stepSource.PlayOneShot(step);
			yield return new WaitForSeconds(stepInterval);
		}
	}
}