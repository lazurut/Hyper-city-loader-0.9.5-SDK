using UnityEngine;

public class TOGGLEBUTTON : MonoBehaviour
{
	public GameObject panel;

	private void Start()
	{
		if (panel != null)
		{
			panel.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && panel != null)
		{
			panel.SetActive(!panel.activeSelf);
		}
	}
}
