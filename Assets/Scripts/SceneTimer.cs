using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTimer : MonoBehaviour
{
	[SerializeField]
	private string nextSceneName;

	[SerializeField]
	private float timerDuration = 8f;

	private float currentTime;

	private void Start()
	{
		currentTime = timerDuration;
	}

	private void Update()
	{
		currentTime -= Time.deltaTime;
		if (currentTime <= 0f)
		{
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
