using UnityEngine;

public class ScrollBPanel : MonoBehaviour
{
	public RectTransform panelContainer;

	public float scrollSpeed = 10f;

	private void Update()
	{
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			Vector3 localPosition = panelContainer.localPosition;
			localPosition.y += axis * scrollSpeed;
			panelContainer.localPosition = localPosition;
		}
	}
}
