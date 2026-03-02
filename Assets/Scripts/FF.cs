using UnityEngine;
using UnityEngine.SceneManagement;

public class FF : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("level 2");
		}
	}
}
