using UnityEngine;
using UnityEngine.SceneManagement;

public class TELEPORMARIO : MonoBehaviour
{
	private void OnTriggerEnter(Collider myCollider)
	{
		if (myCollider.tag == "Player")
		{
			SceneManager.LoadScene("MARIO");
		}
	}
}
