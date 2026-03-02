using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
	public void Play()
	{
		SceneManager.LoadScene("level manager");
	}
}
public class level : MonoBehaviour
{
	public void Play()
	{
		SceneManager.LoadScene("1 уровень");
	}

	public void Exit()
	{
		Application.Quit();
	}
}
