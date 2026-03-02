using UnityEngine;

public class bullet : MonoBehaviour
{
	public float lifetime = 2f;

	private void Start()
	{
		Object.Destroy(base.gameObject, lifetime);
	}
}
