using System.Collections;
using UnityEngine;

namespace ClumsyWizard
{
	public class MathUtility
	{
		public static bool IsPointInsideOval(Transform origin, Vector3 point, bool isPhysicsScale)
		{
			return IsPointInsideOval(origin.position, origin.localScale, point, isPhysicsScale);
		}

		public static bool IsPointInsideOval(Vector3 origin, float scale, Vector3 point, bool isPhysicsScale)
		{
			return IsPointInsideOval(origin, Vector3.one * scale, point, isPhysicsScale);
		}

		public static bool IsPointInsideOval(Vector3 origin, Vector3 scale, Vector3 point, bool isPhysicsScale)
		{
			Vector3 physcisRadius = isPhysicsScale ? scale : ConvertTransformScaleToPhysicsScale(scale);

			float xh = Mathf.Pow(point.x - origin.x, 2) / Mathf.Pow(physcisRadius.x, 2);
			float yk = Mathf.Pow(point.y - origin.y, 2) / Mathf.Pow(physcisRadius.y, 2);

			return (xh + yk <= 1);
		}

		public static Vector3 ConvertTransformScaleToPhysicsScale(Vector3 scale)
		{
			float x = scale.x / 0.42f;
			float y = scale.y / 0.42f;
			float z = scale.z / 0.42f;

			return new Vector3(x, y, z);
		}

		public static Vector3 ConvertPhysicsScaleToTransformScale(Vector3 scale)
		{
			float x = scale.x * 0.66f;
			float y = scale.y * 0.4f;
			float z = scale.z * 0.66f;

			return new Vector3(x, y, z);
		}

		public static Vector3 ConvertPhysicsScaleToTransformScale(float scale)
		{
			float x = scale * 0.4f;
			float y = scale * 0.24f;
			float z = scale * 0.4f;

			return new Vector3(x, y, z);
		}

		public static float GetZRotationFromVector(Vector2 from, Vector2 to)
		{
			Vector2 diff = to - from;
			return GetZRotationFromVector(diff);
		}

		public static float GetZRotationFromVector(Vector2 diff)
		{
			float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			return rotZ;
		}

		public static Vector2 GetDirectionFromAngle(float angleInDegrees, Transform rotateAround)
		{
			angleInDegrees -= rotateAround.eulerAngles.z;
			return GetDirectionFromAngle(angleInDegrees);
		}

		public static Vector2 GetDirectionFromAngle(float angleInDegrees)
		{
			return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)).normalized;
		}
	}
}