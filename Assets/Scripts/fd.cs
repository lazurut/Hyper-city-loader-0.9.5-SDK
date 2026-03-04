using UnityEngine;
using UnityEngine.SceneManagement;

public class fd : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("Wese");
		}
	}
}
