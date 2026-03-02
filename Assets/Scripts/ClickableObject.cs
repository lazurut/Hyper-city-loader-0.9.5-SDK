using UnityEngine;

public class ClickableObject : MonoBehaviour
{
	private void OnMouseDown()
	{
		Object.Destroy(base.gameObject);
	}
}
