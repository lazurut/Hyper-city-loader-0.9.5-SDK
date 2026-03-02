using System;
using Fragsurf.Movement;
using UnityEngine;

namespace Fragsurf.TraceUtil
{
	public class Tracer
	{
		public static Trace TraceCollider(Collider collider, Vector3 origin, Vector3 end, int layerMask, float colliderScale = 1f)
		{
			if (collider is BoxCollider)
			{
				return TraceBox(origin, end, collider.bounds.extents, collider.contactOffset, layerMask, colliderScale);
			}
			if (collider is CapsuleCollider)
			{
				CapsuleCollider capsuleCollider = (CapsuleCollider)collider;
				SurfPhysics.GetCapsulePoints(capsuleCollider, origin, out var p, out var p2);
				return TraceCapsule(p, p2, capsuleCollider.radius, origin, end, capsuleCollider.contactOffset, layerMask, colliderScale);
			}
			throw new NotImplementedException("Trace missing for collider: " + collider.GetType());
		}

		public static Trace TraceCapsule(Vector3 point1, Vector3 point2, float radius, Vector3 start, Vector3 destination, float contactOffset, int layerMask, float colliderScale = 1f)
		{
			Trace result = new Trace
			{
				startPos = start,
				endPos = destination
			};
			float num = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
			radius *= 1f - contactOffset;
			Vector3 normalized = (destination - start).normalized;
			float num2 = Vector3.Distance(start, destination) + num;
			if (Physics.CapsuleCast(point1 - Vector3.up * colliderScale * 0.5f, point2 + Vector3.up * colliderScale * 0.5f, radius * colliderScale, normalized, out var hitInfo, num2, layerMask, QueryTriggerInteraction.Ignore))
			{
				result.fraction = hitInfo.distance / num2;
				result.hitCollider = hitInfo.collider;
				result.hitPoint = hitInfo.point;
				result.planeNormal = hitInfo.normal;
				result.distance = hitInfo.distance;
				Ray ray = new Ray(hitInfo.point - normalized * 0.001f, normalized);
				if (hitInfo.collider.Raycast(ray, out var hitInfo2, 0.002f))
				{
					result.planeNormal = hitInfo2.normal;
				}
			}
			else
			{
				result.fraction = 1f;
			}
			return result;
		}

		public static Trace TraceBox(Vector3 start, Vector3 destination, Vector3 extents, float contactOffset, int layerMask, float colliderScale = 1f)
		{
			Trace result = new Trace
			{
				startPos = start,
				endPos = destination
			};
			float num = Mathf.Sqrt(contactOffset * contactOffset + contactOffset * contactOffset);
			Vector3 normalized = (destination - start).normalized;
			float num2 = Vector3.Distance(start, destination) + num;
			extents *= 1f - contactOffset;
			if (Physics.BoxCast(start, extents * colliderScale, normalized, out var hitInfo, Quaternion.identity, num2, layerMask, QueryTriggerInteraction.Ignore))
			{
				result.fraction = hitInfo.distance / num2;
				result.hitCollider = hitInfo.collider;
				result.hitPoint = hitInfo.point;
				result.planeNormal = hitInfo.normal;
				result.distance = hitInfo.distance;
				Ray ray = new Ray(hitInfo.point - normalized * 0.001f, normalized);
				if (hitInfo.collider.Raycast(ray, out var hitInfo2, 0.002f))
				{
					result.planeNormal = hitInfo2.normal;
				}
			}
			else
			{
				result.fraction = 1f;
			}
			return result;
		}
	}
}
