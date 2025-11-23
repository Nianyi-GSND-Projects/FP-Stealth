using UnityEngine;
using Nianyi.UnityPack;

public class SmallDoor : MonoBehaviour, ISelectable, IInteractable
{
	public GameObject door;
	public GameObject ui;

	public void OnDeselect()
	{
		ui.SetActive(false);
	}

	public void OnSelect()
	{
		ui.SetActive(true);
	}

	public void OnInteract()
	{
		door.SetActive(!door.activeSelf);
	}
}
