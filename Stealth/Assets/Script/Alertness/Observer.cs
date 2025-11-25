using UnityEngine;
using System.Collections.Generic;
using Nianyi.UnityPack;

public class Observer : MonoBehaviour
{
	[Min(0)] public float distance = 10f;
	[Range(0, 90)] public float angle = 45f;

	#region Player visibility
	LayerMask layerMask;
	Player player;

	bool seeingPlayer = false;
	public bool SeeingPlayer
	{
		get => seeingPlayer;
		private set
		{
			if(seeingPlayer == value)
				return;
			seeingPlayer = value;
		}
	}

	bool CanSeePlayer()
	{
		if(!player)
			return false;

		Vector3 center = player.transform.TransformPoint(player.Controller.center);
		Vector3 delta = center - transform.position;
		if(delta.magnitude > distance)
			return false;
		if(Vector3.Angle(delta, transform.forward) > angle)
			return false;

		PhysicsUtility.GetCapsuleCenters(player.Controller, out var head, out var foot);
		return CanSeePosition(head) || CanSeePosition(foot);
	}

	bool CanSeePosition(Vector3 position)
	{
		Vector3 delta = position - transform.position;
		float distance = delta.magnitude;
		Physics.Raycast(transform.position, delta, out var hit, distance, layerMask);
		return hit.collider.gameObject == player.gameObject;
	}
	#endregion

	#region Alertness
	[SerializeField] AlertnessIndicator indicator;
	[SerializeField, Min(0f)] float alertingInterval = 5f;
	bool alerted = false;
	float alertness = 0f;
	public float Alertness
	{
		get => alertness;
		set
		{
			alertness = Mathf.Clamp01(value);
			if(!alerted && alertness == 1)
			{
				alerted = true;
				OnAlerted();
			}
		}
	}

	void OnAlerted()
	{
		GameInstance.Instance?.onPlayerSpotted?.Invoke();
	}
	#endregion

	void Awake()
	{
		layerMask = LayerMask.GetMask("Default", "Ignore Raycast");
	}

	void Start()
	{
		player = GameInstance.Instance.Player;
	}

	void Update()
	{
		float dt = Time.deltaTime;

		SeeingPlayer = CanSeePlayer();
		if(!alerted)
		{
			float da = dt / alertingInterval;
			if(SeeingPlayer)
				Alertness += da;
			else
				Alertness -= da;
			indicator.Value = Alertness;
		}
	}
}
