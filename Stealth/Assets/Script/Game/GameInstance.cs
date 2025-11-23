using UnityEngine;
using Unity.AI.Navigation;

public class GameInstance : MonoBehaviour
{
	#region Unity life cycle
	void Awake()
	{
		foreach(var surface in FindObjectsByType<NavMeshSurface>(FindObjectsSortMode.None))
		{
			surface.RemoveData();
			surface.BuildNavMesh();
		}
	}
	#endregion
}
