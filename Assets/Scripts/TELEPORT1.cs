using UnityEngine;

public class TELEPORT1 : MonoBehaviour
{
	public Transform targetLocation;

	public Transform player;

	private void OnMouseDown()
	{
		if (targetLocation != null && player != null)
		{
			TeleportPlayer();
		}
		else
		{
			Debug.LogWarning("Целевое местоположение или игрок не установлены!");
		}
	}

	private void TeleportPlayer()
	{
		player.position = targetLocation.position;
		player.rotation = targetLocation.rotation;
	}
}
