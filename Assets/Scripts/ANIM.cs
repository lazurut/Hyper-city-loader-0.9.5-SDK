using UnityEngine;

public class ANIM : MonoBehaviour
{
	public string animationNameE;

	public string animationNameZ;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		if (animator == null)
		{
			Debug.LogError("Animator компонент не найден!");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E) && animator != null)
		{
			animator.Play(animationNameE);
		}
		if (Input.GetKeyDown(KeyCode.Z) && animator != null)
		{
			animator.Play(animationNameZ);
		}
	}
}
