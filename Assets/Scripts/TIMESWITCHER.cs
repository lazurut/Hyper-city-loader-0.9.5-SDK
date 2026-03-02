using UnityEngine;
using UnityEngine.SceneManagement;

public class TIMESWITCHER : MonoBehaviour
{
	[SerializeField]
	private string sceneName;

	private static Vector3 lastPosition;

	private void Start()
	{
		if (!string.IsNullOrEmpty(PlayerPrefs.GetString("LastScene")) && PlayerPrefs.GetString("LastScene") == SceneManager.GetActiveScene().name)
		{
			base.transform.position = new Vector3(PlayerPrefs.GetFloat("LastPosX"), PlayerPrefs.GetFloat("LastPosY"), PlayerPrefs.GetFloat("LastPosZ"));
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SavePosition();
			LoadScene();
		}
	}

	private void SavePosition()
	{
		lastPosition = base.transform.position;
		PlayerPrefs.SetString("LastScene", SceneManager.GetActiveScene().name);
		PlayerPrefs.SetFloat("LastPosX", lastPosition.x);
		PlayerPrefs.SetFloat("LastPosY", lastPosition.y);
		PlayerPrefs.SetFloat("LastPosZ", lastPosition.z);
		PlayerPrefs.Save();
	}

	private void LoadScene()
	{
		if (!string.IsNullOrEmpty(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
		else
		{
			Debug.LogError("Scene name is not set in the inspector!");
		}
	}
}
