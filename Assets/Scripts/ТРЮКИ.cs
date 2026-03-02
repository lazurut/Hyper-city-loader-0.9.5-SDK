using UnityEngine;

public class ТРЮКИ : MonoBehaviour
{
	private bool isSpinning;

	private float spinTime = 5f;

	private float spinDuration = 5f;

	private int keyPressCount;

	private bool isSpinningActivated;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.Space))
		{
			keyPressCount++;
			if (keyPressCount == 4)
			{
				isSpinningActivated = true;
				keyPressCount = 0;
			}
		}
		if (isSpinningActivated && !isSpinning)
		{
			isSpinning = true;
			isSpinningActivated = false;
		}
		if (isSpinning)
		{
			if (spinTime > 0f)
			{
				base.transform.Rotate(0f, 360f * Time.deltaTime, 0f);
				spinTime -= Time.deltaTime;
			}
			else
			{
				isSpinning = false;
				spinTime = spinDuration;
				base.transform.rotation = Quaternion.identity;
			}
		}
	}
}
