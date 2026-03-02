using UnityEngine;

public class pause : MonoBehaviour
{
	public GameObject panel;

	public void Pause()
	{
		panel.SetActive(value: true);
		Time.timeScale = 0f;
	}
}
