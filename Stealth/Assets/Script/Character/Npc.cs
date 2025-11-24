using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nianyi.UnityPack;

public enum NpcStatus { Idle, Patrolling, Chasing }

[RequireComponent(typeof(NavMeshAgent))]
public class Npc : Character
{
	NavMeshAgent agent;
	[SerializeField] Animator animator;

	#region Unity lyfe cycle
	protected new void Awake()
	{
		base.Awake();
		agent = GetComponent<NavMeshAgent>();
	}

	protected IEnumerator Start()
	{
		if(!GameInstance.Instance)
			yield break;
		yield return new WaitUntil(() => GameInstance.Instance.Ready);

		agent.enabled = true;
		switch(status)
		{
			case NpcStatus.Patrolling:
				Patrol(patrolPoints);
				break;
		}
	}

	protected void Update()
	{
		animator.SetBool("Walking", IsWalking);
	}
	#endregion

	#region Status
	[SerializeField] NpcStatus status = NpcStatus.Idle;
	public NpcStatus Status => status;

	[SerializeField] List<Transform> patrolPoints;
	#endregion

	#region Movement
	void NavigateTo(Vector3 destination)
	{
		StopCoroutine(nameof(NavigateToCoroutine));
		StartCoroutine(nameof(NavigateToCoroutine), destination);
	}

	bool navigating = false;

	IEnumerator NavigateToCoroutine(Vector3 destination)
	{
		NavMeshPath path = new();
		if(!agent.CalculatePath(destination, path))
		{
			Debug.LogWarning($"Unable to path find to {destination}.");
			yield break;
		}
		navigating = true;
		foreach(var point in path.corners)
			yield return GoToCoroutine(point);
		navigating = false;
	}

	IEnumerator GoToCoroutine(Vector3 destination)
	{
		for(; ; )
		{
			Vector3 delta = destination - body.transform.position;
			if(delta.magnitude < controller.radius)
				break;

			float deltaAzimuth = Quaternion.LookRotation(delta).eulerAngles.y - Azimuth;
			if(deltaAzimuth > 180f)
				deltaAzimuth -= 360f;
			if(deltaAzimuth < -180f)
				deltaAzimuth += 360f;
			float azimuthClamp = 360f * orientSpeed * Time.fixedDeltaTime;
			Azimuth += Mathf.Clamp(deltaAzimuth, -azimuthClamp, azimuthClamp);
			
			DesiredVelocity = body.forward * MoveSpeed;
			yield return new WaitForFixedUpdate();
		}
	}

	public void Patrol(IEnumerable<Transform> points)
	{
		points = points.ToArray();
		patrolPoints.Clear();
		patrolPoints.AddRange(points);

		StopCoroutine(nameof(PatrolCoroutine));
		StartCoroutine(nameof(PatrolCoroutine), patrolPoints.Select(p => p.position).ToArray());
	}

	IEnumerator PatrolCoroutine(Vector3[] points)
	{
		for(; ; )
		{
			foreach(var point in points)
			{
				NavigateTo(point);
				yield return new WaitWhile(() => navigating);
			}
		}
	}
	#endregion
}
