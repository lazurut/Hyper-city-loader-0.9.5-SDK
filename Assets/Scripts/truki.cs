using UnityEngine;

public class truki : MonoBehaviour
{
	public Animator animator;

	public string animationTrigger = "PlayAnimation";

	private void Update()
	{
		if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Z) && animator != null)
		{
			animator.SetTrigger(animationTrigger);
		}
	}
}
