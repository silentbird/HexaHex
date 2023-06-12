using System;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

namespace pure.utils.mathTools {
	public static class VectorMath {
		public const float RAD_DEGREE = 57.295776f;
		public const float DEGREE_RAD = 0.017453292f;
		public static readonly Vector2[] uvs = new Vector2[4];
		public static readonly Vector3[] verts = new Vector3[4];
		public static readonly UIVertex[] vertex = new UIVertex[4];
		public static readonly int[] quadIndics = new int[6];
		private static readonly StringBuilder helper = new();

		public static Vector2 Truncate(this Vector2 v, float max) {
			return Vector2.ClampMagnitude(v, max);
		}

		public static float DistanceToSegment(this Vector2 v, Vector2 a, Vector2 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			if (num < 0.0)
				return (v - a).magnitude;
			return num > 1.0 ? (v - b).magnitude : (v - (a + num * (b - a))).magnitude;
		}

		public static float SqrDistanceToSegment(this Vector2 v, Vector2 a, Vector2 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			if (num < 0.0)
				return (v - a).sqrMagnitude;
			return num > 1.0 ? (v - b).sqrMagnitude : (v - (a + num * (b - a))).sqrMagnitude;
		}

		public static float DistanceToLine(this Vector2 v, Vector2 a, Vector2 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			return (v - (a + num * (b - a))).magnitude;
		}

		public static float SqrDistanceToLine(this Vector2 v, Vector2 a, Vector2 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			return (v - (a + num * (b - a))).sqrMagnitude;
		}

		public static Vector3 NearestToSegment(this Vector3 v, Vector3 a, Vector3 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			if (num < 0.0)
				return a;
			return num > 1.0 ? b : a + num * (b - a);
		}

		public static float DistanceToSegment(this Vector3 v, Vector3 a, Vector3 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			if (num < 0.0)
				return (v - a).magnitude;
			return num > 1.0 ? (v - b).magnitude : (v - (a + num * (b - a))).magnitude;
		}

		public static Vector3 NearestToLine(this Vector3 v, Vector3 a, Vector3 b) {
			var num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
			return a + num * (b - a);
		}

		public static float Dot(this Vector3 v, Vector3 a) {
			return Vector3.Dot(v, a);
		}

		public static float LeftOf(this Vector2 c, Vector2 a, Vector2 b) {
			return (a - c).Cross(b - a);
		}

		public static int Sign(this Vector2 a, Vector2 b) {
			var num = a.Cross(b);
			if (num < 0.0)
				return -1;
			return !num.Equals(0.0f) ? 1 : 0;
		}

		public static bool IsParrallel(this Vector2 a, Vector2 b) {
			a.Normalize();
			b.Normalize();
			if (a.x.Equals(b.x) && a.y.Equals(b.y))
				return true;
			return a.x.Equals(-b.x) && a.y.Equals(-b.y);
		}

		public static Vector2 GetPerp(this Vector2 v) {
			return new Vector2(-v.y, v.x);
		}

		public static Vector3 Round(this Vector3 v, int digit) {
			return new Vector3((float)Math.Round(v.x, digit), (float)Math.Round(v.y, digit), (float)Math.Round(v.z, digit));
		}

		public static Vector2 Rotate(this Vector2 v, float degree) {
			var f = degree * ((float)Math.PI / 180f);
			var num1 = Mathf.Sin(f);
			var num2 = Mathf.Cos(f);
			return new Vector2((float)(v.y * (double)num1 - v.x * (double)num2), (float)(v.x * (double)num1 + v.y * (double)num2));
		}

		private static Vector3 rotate_to_rad(this Vector3 v, float rad) {
			var num1 = Mathf.Sin(rad);
			var num2 = Mathf.Cos(rad);
			return new Vector3((float)(v.x * (double)num2 + v.z * (double)num1), v.y, (float)(-(double)v.x * num1 + v.z * (double)num2));
		}

		public static Vector3 Rotate(this Vector3 v, float degree) {
			return v.rotate_to_rad(degree * ((float)Math.PI / 180f));
		}

		public static Vector3 RotateTo(this Vector3 v, Vector3 dir) {
			var rad = Mathf.Atan2(dir.x, dir.z);
			return v.rotate_to_rad(rad);
		}

		public static float Dot(this Vector2 a, Vector2 b) {
			return Vector2.Dot(a, b);
		}

		public static float Cross(this Vector2 a, Vector2 b) {
			return (float)(a.x * (double)b.y - a.y * (double)b.x);
		}

		public static float AngleTo(this Vector2 a, Vector2 b) {
			var magnitude1 = a.magnitude;
			var magnitude2 = b.magnitude;
			var f = a.Dot(b) / (magnitude1 * magnitude2);
			if (f < -1.0)
				f = -1f;
			if (f > 1.0)
				f = 1f;
			return Mathf.Acos(f);
		}

		public static float DegreeTo(this Vector2 a, Vector2 b) {
			return a.AngleTo(b) * 57.295776f;
		}

		public static float PerpDotOf(this Vector2 a, Vector2 b) {
			return a.GetPerp().Dot(b);
		}

		public static Vector2 Projection(this Vector2 a, Vector2 b) {
			return b * (a.Dot(b) / b.Dot(b));
		}

		public static bool Inside(this Vector2 a, Vector2 topLeft, Vector2 botRight) {
			return a.x >= (double)topLeft.x && a.x <= topLeft.x + (double)botRight.x && a.y >= (double)topLeft.y &&
			       a.y <= topLeft.y + (double)botRight.y;
		}

		public static float CalcOrientV2(Vector2 dir) {
			return Mathf.Atan2(dir.x, dir.y) * 57.29578f;
		}

		public static float CalcOrientation(Vector3 dir) {
			return dir.sqrMagnitude < 9.999999747378752E-06 ? 0.0f : Mathf.Atan2(dir.x, dir.z) * 57.29578f;
		}

		public static bool InCamera(Camera cam, Vector3 position) {
			var viewportPoint = cam.WorldToViewportPoint(position);
			return new Rect(0.0f, 0.0f, 1f, 1f).Contains(viewportPoint) && viewportPoint.z >= (double)cam.nearClipPlane && viewportPoint.z <= (double)cam.farClipPlane;
		}

		public static bool InCamera(Camera cam, Bounds bound) {
			var viewportPoint1 = cam.WorldToViewportPoint(bound.min);
			var viewportPoint2 = cam.WorldToViewportPoint(bound.max);
			return new Rect(viewportPoint1, viewportPoint2 - viewportPoint1).Overlaps(new Rect(0.0f, 0.0f, 1f, 1f)) && viewportPoint1.z < (double)cam.farClipPlane &&
			       viewportPoint2.z > (double)cam.nearClipPlane;
		}


		public static Vector3 Truncate(this Vector3 v, float min, float max) {
			if (v.magnitude < (double)min)
				v = v.normalized * min;
			else if (v.magnitude > (double)max)
				v = v.normalized * max;
			return v;
		}

		public static void CalcPolyCenter(Vector3[] vs, out Vector3 center) {
			center = new Vector3();
			var length = vs.Length;
			for (var index = 0; index < length; ++index)
				center += vs[index];
			center /= length;
		}

		public static void PrintVectors(Vector3[] vs) {
			var stringBuilder = new StringBuilder();
			foreach (var v in vs)
				stringBuilder.AppendLine(v.ToString());
			Debug.Log(stringBuilder.ToString());
		}

		public static Vector2 ToVec2(this Vector3 pos) {
			return new Vector2(pos.x, pos.z);
		}

		public static Vector3 ToVec3(this Vector2 pos) {
			return new Vector3(pos.x, 0.0f, pos.y);
		}

		public static bool IsNaN(this Vector3 v) {
			return float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);
		}

		public static bool IsNaN(this Vector2 v) {
			return float.IsNaN(v.x) || float.IsNaN(v.y);
		}

		public static Vector3 RoundToInt(this Vector3 v) {
			return new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
		}

		public static Vector2 RoundToInt(this Vector2 v) {
			return new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
		}

		public static void CalcUp(Vector3 dir, out Vector3 right, out Vector3 up) {
			up = Mathf.Abs(dir.y) > 0.9990000128746033 ? Vector3.forward : Vector3.up;
			right = Vector3.Cross(up, dir).normalized;
			up = Vector3.Cross(dir, right);
		}

		public static bool IsEqual(this Vector3 a, Vector3 b, float sqrEpilon = 1E-05f) {
			return (a - b).sqrMagnitude < (double)sqrEpilon;
		}

		public static bool IsEqual(this Vector2 a, Vector2 b, float sqrEpilon = 1E-05f) {
			return (a - b).sqrMagnitude < (double)sqrEpilon;
		}

		public static bool IsEqual(this Bounds a, Bounds b, float sqrEpilon = 1E-05f) {
			return a.center.IsEqual(b.center, sqrEpilon) && a.extents.IsEqual(b.extents, sqrEpilon);
		}

		public static Vector2 ScreenToUI(Vector2 screen, RectTransform rect, Camera canvasCam) {
			Vector2 localPoint;
			if ((bool)(Object)canvasCam && RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screen, canvasCam, out localPoint))
				return localPoint;
			var rect1 = rect.rect;
			screen.x -= Screen.width * 0.5f;
			screen.y -= Screen.height * 0.5f;
			localPoint = new Vector2 {
				x = screen.x / Screen.width * rect1.width,
				y = screen.y / Screen.height * rect1.height
			};
			return localPoint;
		}

		private static bool TryToScreenAnchor(
			Vector2 screenPos,
			RectTransform rect,
			Camera cam,
			out Vector2 anchor) {
			if ((bool)(Object)rect) {
				if (!(bool)(Object)cam) {
					var componentInParent = rect.GetComponentInParent<Canvas>();
					cam = (bool)(Object)componentInParent ? componentInParent.worldCamera : null;
				}

				if ((bool)(Object)cam && RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, cam, out anchor))
					return true;
			}

			anchor = Vector2.zero;
			return false;
		}

		public static bool TryWorldToUI(
			Vector3 position,
			Camera worldCam,
			RectTransform rect,
			Camera uiCamera,
			out Vector2 anchor) {
			return !(bool)(Object)uiCamera
				? TryWorldToUI(position, worldCam, rect, out anchor)
				: TryToScreenAnchor(worldCam.WorldToScreenPoint(position), rect, uiCamera, out anchor);
		}

		public static bool TryWorldToUI(
			Vector3 position,
			Camera worldCam,
			RectTransform rect,
			out Vector2 anchor) {
			if ((bool)(Object)worldCam) {
				var rect1 = rect.rect;
				Vector2 screenPoint = worldCam.WorldToScreenPoint(position);
				screenPoint.x -= Screen.width * 0.5f;
				screenPoint.y -= Screen.height * 0.5f;
				anchor = new Vector2 {
					x = screenPoint.x / Screen.width * rect1.width,
					y = screenPoint.y / Screen.height * rect1.height
				};
				return true;
			}

			anchor = Vector2.one;
			return false;
		}

		public static bool TryWorldToUI(Vector3 position, RectTransform rect, out Vector2 anchor) {
			var worldCam = Camera.main;
			return TryWorldToUI(position, worldCam, rect, out anchor);
		}

		public static string VString(Vector3 v) {
			return string.Format("({0},{1},{2})", v.x, v.y, v.z);
		}

		public static string ToString(Vector2[] vs) {
			helper.Length = 0;
			helper.Append("[ ");
			for (var index = 0; index < vs.Length; ++index) {
				helper.Append(vs[index]);
				helper.Append(index == vs.Length - 1 ? " ]" : " , ");
			}

			return helper.ToString();
		}

		public static string ToString(Vector3[] vs) {
			helper.Length = 0;
			helper.Append("[ ");
			for (var index = 0; index < vs.Length; ++index) {
				helper.Append(vs[index]);
				helper.Append(index == vs.Length - 1 ? " ]" : " , ");
			}

			return helper.ToString();
		}

		public static string ToString(Vector3 v) {
			return string.Format("({0}f,{1}f,{2}f)", v.x, v.y, v.z);
		}

		public static string ToStringF(this Vector3 v) {
			return string.Format("({0}f,{1}f,{2}f)", v.x, v.y, v.z);
		}

		public static string ToStringF(this Vector2 v) {
			return string.Format("({0}f,{1}f)", v.x, v.y);
		}

		public static string ToStringF(this Vector4 v) {
			return string.Format("({0:F}f,{1:F}f,{2:F}f,{3:F}f)", v.x, v.y, v.z, v.w);
		}

		public static string ToStringF(this Quaternion v) {
			return string.Format("({0}f,{1}f,{2}f,{3}f)", v.x, v.y, v.z, v.w);
		}

		public static bool IsNaN(ref Matrix4x4 matrix) {
			for (var index = 0; index < 16; ++index)
				if (float.IsNaN(matrix[index]))
					return true;
			return false;
		}

		public static bool IsNaN(this Matrix4x4 matrix) {
			return IsNaN(ref matrix);
		}

		public static int IntDegree(this float orient, int segments) {
			var num1 = 360f / segments;
			var num2 = num1 * 0.5f;
			var num3 = (int)Mathf.Repeat((int)(Mathf.Repeat(orient + num2, 360f) / (double)num1), segments);
			return (int)(num1 * num3);
		}

		public static Vector3 IntDegree(this Vector3 dir, int segments) {
			var magnitude = dir.magnitude;
			dir = dir.normalized;
			var num = ((float)(Math.Atan2(dir.z, dir.x) * 57.2957763671875)).IntDegree(segments) * (Math.PI / 180.0);
			dir.x = (float)Math.Cos(num);
			dir.z = (float)Math.Sin(num);
			dir *= magnitude;
			return dir;
		}

		public static float LimitAngle(float orient, int angles, float zeroOffset) {
			var num = 360f / angles;
			return Mathf.Floor((float)(orient + (double)zeroOffset - num * 0.5) / num) * num + zeroOffset;
		}
	}
}