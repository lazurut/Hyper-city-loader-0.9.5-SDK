using UnityEngine;

public class Skyboxdark : MonoBehaviour
{
	public Material skyboxMaterial;

	public float darkenSpeed = 0.1f;

	private float targetExposure = 0.5f;

	private void Update()
	{
		if (skyboxMaterial != null)
		{
			float value = Mathf.Lerp(skyboxMaterial.GetFloat("_Exposure"), targetExposure, Time.deltaTime * darkenSpeed);
			skyboxMaterial.SetFloat("_Exposure", value);
		}
	}
}
