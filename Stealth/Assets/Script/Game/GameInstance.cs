using UnityEngine;
using Unity.AI.Navigation;

public class GameInstance : MonoBehaviour
{
	public static GameInstance Instance { get; private set; }
	public bool Ready { get; private set; } = false;

	public Player Player { get; private set; }
	[SerializeField] Canvas ui;
	public Canvas Ui => ui;

	void Awake()
	{
		Instance = this;
		Player = FindObjectOfType<Player>();
		onPlayerSpotted += OnPlayerSpotted;
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

	public System.Action onPlayerSpotted;
	void OnPlayerSpotted()
	{
		Debug.Log("The player is spotted!");
		foreach(var npc in FindObjectsByType<Npc>(FindObjectsSortMode.None))
			npc.Follow(Player.transform);
	}
}
