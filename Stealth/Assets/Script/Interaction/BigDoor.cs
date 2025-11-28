using UnityEngine;
using Nianyi.UnityPack;

public class BigDoor : MonoBehaviour, ISelectable, IInteractable
{
	public GameObject leftDoor, rightDoor;
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
		leftDoor.SetActive(!leftDoor.activeSelf);
		rightDoor.SetActive(!rightDoor.activeSelf);
	}
}