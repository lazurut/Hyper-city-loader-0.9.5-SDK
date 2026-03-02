using UnityEngine;

public class SKYBOX : MonoBehaviour
{
	public Material skybox1;

	public Material skybox2;

	private bool isSkybox1Active = true;

	private void Start()
	{
		RenderSettings.skybox = skybox1;
	}

	public void ChangeSkybox()
	{
		if (isSkybox1Active)
		{
			RenderSettings.skybox = skybox2;
		}
		else
		{
			RenderSettings.skybox = skybox1;
		}
		isSkybox1Active = !isSkybox1Active;
	}
}
