using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections;

public class GameInstance : MonoBehaviour
{
	public static GameInstance Instance { get; private set; }
	public bool Ready { get; private set; } = false;

	void Awake()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Start()
	{
		foreach(var surface in FindObjectsByType<NavMeshSurface>(FindObjectsSortMode.None))
		{
			surface.RemoveData();
			surface.BuildNavMesh();
		}

		Ready = true;
	}
}
