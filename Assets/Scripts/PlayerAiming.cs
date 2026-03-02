using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
	[Header("References")]
	public Transform bodyTransform;

	[Header("Sensitivity")]
	public float sensitivityMultiplier = 1f;

	public float horizontalSensitivity = 1f;

	public float verticalSensitivity = 1f;

	[Header("Restrictions")]
	public float minYRotation = -90f;

	public float maxYRotation = 90f;

	private Vector3 realRotation;

	[Header("Aimpunch")]
	[Tooltip("bigger number makes the response more damped, smaller is less damped, currently the system will overshoot, with larger damping values it won't")]
	public float punchDamping = 9f;

	[Tooltip("bigger number increases the speed at which the view corrects")]
	public float punchSpringConstant = 65f;

	[HideInInspector]
	public Vector2 punchAngle;

	[HideInInspector]
	public Vector2 punchAngleVel;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
	{
		if (!(Mathf.Abs(Time.timeScale) <= 0f))
		{
			DecayPunchAngle();
			float num = Input.GetAxisRaw("Mouse X") * horizontalSensitivity * sensitivityMultiplier;
			float num2 = (0f - Input.GetAxisRaw("Mouse Y")) * verticalSensitivity * sensitivityMultiplier;
			realRotation = new Vector3(Mathf.Clamp(realRotation.x + num2, minYRotation, maxYRotation), realRotation.y + num, realRotation.z);
			realRotation.z = Mathf.Lerp(realRotation.z, 0f, Time.deltaTime * 3f);
			bodyTransform.eulerAngles = Vector3.Scale(realRotation, new Vector3(0f, 1f, 0f));
			Vector3 eulerAngles = realRotation;
			eulerAngles.x += punchAngle.x;
			eulerAngles.y += punchAngle.y;
			base.transform.eulerAngles = eulerAngles;
		}
	}

	public void ViewPunch(Vector2 punchAmount)
	{
		punchAngle = Vector2.zero;
		punchAngleVel -= punchAmount * 20f;
	}

	private void DecayPunchAngle()
	{
		if ((double)punchAngle.sqrMagnitude > 0.001 || (double)punchAngleVel.sqrMagnitude > 0.001)
		{
			punchAngle += punchAngleVel * Time.deltaTime;
			float num = 1f - punchDamping * Time.deltaTime;
			if (num < 0f)
			{
				num = 0f;
			}
			punchAngleVel *= num;
			float num2 = punchSpringConstant * Time.deltaTime;
			punchAngleVel -= punchAngle * num2;
		}
		else
		{
			punchAngle = Vector2.zero;
			punchAngleVel = Vector2.zero;
		}
	}
}
