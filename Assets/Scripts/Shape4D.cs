using System;
using UnityEngine;

public class Shape4D : MonoBehaviour
{
	public enum ShapeType
	{
		HyperSphere = 0,
		HyperCube = 1,
		DuoCylinder = 2,
		plane = 3,
		Cone = 4,
		FiveCell = 5,
		SixteenCell = 6
	}

	public enum Operation
	{
		Union = 0,
		Blend = 1,
		Substract = 2,
		Intersect = 3
	}

	[Header("Shape Settings")]
	public ShapeType shapeType;

	public Operation operation;

	[Header("4D Transform Settings")]
	public float positionW;

	[Tooltip("The rotation around the xw, yw and zw planes respectively")]
	public Vector3 rotationW;

	public float scaleW = 1f;

	[Header("Render Settings")]
	public Color colour;

	[Range(0f, 1f)]
	public float smoothRadius;

	[HideInInspector]
	public int numChildren;

	private Vector4 parentScale = Vector4.one;

	public Vector4 Position()
	{
		Vector3 position = base.transform.position;
		return new Vector4(position.x, position.y, position.z, positionW);
	}

	public Vector3 Rotation()
	{
		return base.transform.eulerAngles * (MathF.PI / 180f);
	}

	public Vector3 RotationW()
	{
		return rotationW * (MathF.PI / 180f);
	}

	public Vector4 Scale()
	{
		if (base.transform.parent != null && base.transform.parent.TryGetComponent<Shape4D>(out var component))
		{
			parentScale = component.Scale();
		}
		else
		{
			parentScale = Vector4.one;
		}
		Vector3 localScale = base.transform.localScale;
		return Vector4.Scale(new Vector4(localScale.x, localScale.y, localScale.z, scaleW), parentScale);
	}
}
