using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class RaymarchCam : SceneViewFilter
{
	private struct ShapeData
	{
		public Vector4 position;

		public Vector4 scale;

		public Vector3 rotation;

		public Vector3 rotationW;

		public Vector3 colour;

		public int shapeType;

		public int operation;

		public float blendStrength;

		public int numChildren;

		public static int GetSize()
		{
			return 84;
		}
	}

	[SerializeField]
	[Header("Global Settings")]
	private Shader _shader;

	private List<ComputeBuffer> buffersToDispose;

	private Material _raymarchMat;

	private Camera _cam;

	private float _forceFieldRad;

	public Transform _directionalLight;

	public Transform _player;

	public float _precision;

	public float _max_iteration;

	[Header("Global Transform Settings")]
	public Vector3 _wRotation;

	public float _wPosition;

	[Header("Visual Settings")]
	public bool _useNormal;

	[Tooltip("the number of cellshading cascades, set 0 for smooth lighting")]
	[Range(0f, 10f)]
	public int _nrOfCascades;

	[Range(0f, 1f)]
	public float _lightIntensity;

	[Space(10f)]
	public bool _useShadow;

	public bool _useSoftShadow;

	public float _shadowSoftness;

	public float _maxShadowDistance;

	[Range(0f, 1f)]
	public float _shadowIntensity;

	[Space(10f)]
	[Range(0f, 1f)]
	public float _aoIntensity;

	[Space(10f)]
	[Tooltip("The color of the depthbuffer")]
	public Color _skyColor;

	[HideInInspector]
	public int _renderNr;

	[HideInInspector]
	public List<Shape4D> orderedShapes = new List<Shape4D>();

	public Material _raymarchMaterial
	{
		get
		{
			if (!_raymarchMat && (bool)_shader)
			{
				_raymarchMat = new Material(_shader);
				_raymarchMat.hideFlags = HideFlags.HideAndDontSave;
			}
			return _raymarchMat;
		}
	}

	public Camera _camera
	{
		get
		{
			if (!_cam)
			{
				_cam = GetComponent<Camera>();
			}
			return _cam;
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		buffersToDispose = new List<ComputeBuffer>();
		CreateScene();
		SetParameters();
		if (!_raymarchMaterial)
		{
			Graphics.Blit(source, destination);
			return;
		}
		RenderTexture.active = destination;
		_raymarchMaterial.SetTexture("_MainTex", source);
		GL.PushMatrix();
		GL.LoadOrtho();
		_raymarchMaterial.SetPass(0);
		GL.Begin(7);
		GL.MultiTexCoord2(0, 0f, 0f);
		GL.Vertex3(0f, 0f, 3f);
		GL.MultiTexCoord2(0, 1f, 0f);
		GL.Vertex3(1f, 0f, 2f);
		GL.MultiTexCoord2(0, 1f, 1f);
		GL.Vertex3(1f, 1f, 1f);
		GL.MultiTexCoord2(0, 0f, 1f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
		foreach (ComputeBuffer item in buffersToDispose)
		{
			item.Dispose();
		}
	}

	private void SetParameters()
	{
		if (_useNormal)
		{
			_raymarchMaterial.SetInt("_useNormal", 1);
			_raymarchMaterial.SetInt("_nrOfCascades", _nrOfCascades);
		}
		else
		{
			_raymarchMaterial.SetInt("_useNormal", 0);
		}
		if (_useShadow)
		{
			_raymarchMaterial.SetInt("_useShadow", 1);
			if (_useSoftShadow)
			{
				_raymarchMaterial.SetInt("_useShadow", 2);
				_raymarchMaterial.SetFloat("_shadowSoftness", _shadowSoftness);
			}
		}
		else
		{
			_raymarchMaterial.SetInt("_useShadow", 0);
		}
		_raymarchMaterial.SetMatrix("_CamFrustrum", CamFrustrum(_camera));
		_raymarchMaterial.SetMatrix("_CamToWorld", _camera.cameraToWorldMatrix);
		_raymarchMaterial.SetFloat("_maxDistance", Camera.main.farClipPlane);
		_raymarchMaterial.SetFloat("_precision", _precision);
		_raymarchMaterial.SetFloat("_max_iteration", _max_iteration);
		_raymarchMaterial.SetFloat("_maxShadowDistance", _maxShadowDistance);
		_raymarchMaterial.SetFloat("_lightIntensity", _lightIntensity);
		_raymarchMaterial.SetFloat("_shadowIntensity", _shadowIntensity);
		_raymarchMaterial.SetFloat("_aoIntensity", _aoIntensity);
		_raymarchMaterial.SetVector("_lightDir", _directionalLight ? _directionalLight.forward : Vector3.down);
		_raymarchMaterial.SetVector("_player", _player ? _player.position : Vector3.zero);
		_raymarchMaterial.SetColor("_skyColor", _skyColor);
		_raymarchMaterial.SetVector("_wRotation", _wRotation * (MathF.PI / 180f));
		_raymarchMaterial.SetFloat("w", _wPosition);
		_raymarchMaterial.SetInt("_renderNr", _renderNr);
	}

	private Matrix4x4 CamFrustrum(Camera cam)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		float num = Mathf.Tan(cam.fieldOfView * 0.5f * (MathF.PI / 180f));
		Vector3 vector = Vector3.up * num;
		Vector3 vector2 = Vector3.right * num * cam.aspect;
		Vector3 vector3 = -Vector3.forward - vector2 + vector;
		Vector3 vector4 = -Vector3.forward + vector2 + vector;
		Vector3 vector5 = -Vector3.forward - vector2 - vector;
		Vector3 vector6 = -Vector3.forward + vector2 - vector;
		identity.SetRow(0, vector3);
		identity.SetRow(1, vector4);
		identity.SetRow(2, vector6);
		identity.SetRow(3, vector5);
		return identity;
	}

	private void CreateScene()
	{
		List<Shape4D> list = new List<Shape4D>(UnityEngine.Object.FindObjectsOfType<Shape4D>());
		list.Sort((Shape4D a, Shape4D b) => a.operation.CompareTo(b.operation));
		orderedShapes = new List<Shape4D>();
		for (int num = 0; num < list.Count; num++)
		{
			if (!(list[num].transform.parent == null))
			{
				continue;
			}
			Transform transform = list[num].transform;
			orderedShapes.Add(list[num]);
			list[num].numChildren = transform.childCount;
			for (int num2 = 0; num2 < transform.childCount; num2++)
			{
				if (transform.GetChild(num2).TryGetComponent<Shape4D>(out var component))
				{
					orderedShapes.Add(component);
					orderedShapes[orderedShapes.Count - 1].numChildren = 0;
				}
			}
		}
		ShapeData[] array = new ShapeData[orderedShapes.Count];
		for (int num3 = 0; num3 < orderedShapes.Count; num3++)
		{
			Shape4D shape4D = orderedShapes[num3];
			Vector3 colour = new Vector3(shape4D.colour.r, shape4D.colour.g, shape4D.colour.b);
			array[num3] = new ShapeData
			{
				position = shape4D.Position(),
				scale = shape4D.Scale(),
				rotation = shape4D.Rotation(),
				rotationW = shape4D.RotationW(),
				colour = colour,
				shapeType = (int)shape4D.shapeType,
				operation = (int)shape4D.operation,
				blendStrength = shape4D.smoothRadius * 3f,
				numChildren = shape4D.numChildren
			};
		}
		ComputeBuffer computeBuffer = new ComputeBuffer(array.Length, ShapeData.GetSize());
		computeBuffer.SetData(array);
		_raymarchMaterial.SetBuffer("shapes", computeBuffer);
		_raymarchMaterial.SetInt("numShapes", array.Length);
		buffersToDispose.Add(computeBuffer);
	}
}
