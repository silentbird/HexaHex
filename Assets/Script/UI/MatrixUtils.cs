// Decompiled with JetBrains decompiler
// Type: pure.utils.mathTools.MatrixUtils
// Assembly: pure, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3647E74B-3271-4E18-8589-B6FF96DC8026
// Assembly location: D:\W\UnityProj\mhgd\proj_mhgd\Assets\Plugins\core\pure\Win\pure.dll

using System;
using UnityEngine;

namespace pure.utils.mathTools {
	public static class MatrixUtils {
		private static readonly Vector3[] helpV = new Vector3[4];

		public static void Rotate(ref Matrix4x4 matrix, float degree) {
			Rotate(ref matrix, degree, Vector3.zero);
		}

		public static void Rotate(ref Matrix4x4 matrix, float degree, Vector3 pivot) {
			matrix =
				Matrix4x4.TRS(pivot, Quaternion.Euler(0.0f, 0.0f, degree), Vector3.one) *
				Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) * matrix;
		}

		public static void Rotate(ref Matrix4x4 matrix, Vector3 rotate, Vector3 pivot) {
			matrix =
				Matrix4x4.TRS(pivot, Quaternion.Euler(rotate), Vector3.one) *
				Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) * matrix;
		}

		public static void Scale(ref Matrix4x4 matrix, Vector3 scale) {
			Scale(ref matrix, scale, Vector3.zero);
		}

		public static void Scale(ref Matrix4x4 matrix, Vector3 scale, Vector3 pivot) {
			matrix =
				Matrix4x4.TRS(pivot, Quaternion.identity, scale) * Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) *
				matrix;
		}

		public static void Translate(ref Matrix4x4 matrix, Vector3 translate) {
			matrix = Matrix4x4.TRS(translate, Quaternion.identity, Vector3.one) * matrix;
		}

		private static void FillRect(Rect r) {
			helpV[0] = new Vector3(r.x, r.y, 0.0f);
			helpV[1] = new Vector3(r.xMax, r.y, 0.0f);
			helpV[2] = new Vector3(r.xMax, r.yMax, 0.0f);
			helpV[3] = new Vector3(r.x, r.yMax, 0.0f);
		}

		public static Matrix4x4 BuildGradientBox(Rect rect, float rotate) {
			FillRect(rect);
			return BuildGradientBox(helpV, rotate);
		}

		public static Matrix4x4 BuildGradientBox(Vector3[] vs, float rotate) {
			var rect = RectMath2F.CaculateBoundary(vs);
			var identity = Matrix4x4.identity;
			Rotate(ref identity, rotate, new Vector3(rect.width * 0.5f, rect.height * 0.5f, 0.0f));
			var length = vs.Length;
			for (var index = 0; index < length; ++index)
				vs[index] = identity.MultiplyPoint(vs[index]);
			var vector3_1 = new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0.0f);
			var vector3_2 = new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0.0f);
			for (var index = 0; index < length; ++index) {
				var v = vs[index];
				if (v.x < (double)vector3_1.x)
					vector3_1.x = v.x;
				if (v.x > (double)vector3_2.x)
					vector3_2.x = v.x;
				if (v.y < (double)vector3_1.y)
					vector3_1.y = v.y;
				if (v.y > (double)vector3_2.y)
					vector3_2.y = v.y;
			}

			var num1 = (float)(vector3_2.x - (double)vector3_1.x - 9.999999974752427E-07);
			var num2 = (float)(vector3_2.y - (double)vector3_1.y - 9.999999974752427E-07);
			var scale = new Vector3(num1 / rect.width, num2 / rect.height, 1f);
			Vector3 center = rect.center;
			identity = Matrix4x4.identity;
			Scale(ref identity, scale, center);
			Rotate(ref identity, rotate, center);
			return identity;
		}

		public static Matrix4x4 CalcInnerbound(Rect rect, float rotate) {
			FillRect(rect);
			return CalcInnerbound(helpV, rotate);
		}

		public static Matrix4x4 CalcInnerbound(Vector3[] vs, float rotate) {
			return BuildGradientBox(vs, rotate).inverse;
		}

		public static Matrix4x4 Skew(Vector2 skew) {
			return Skew(skew, Vector3.zero);
		}

		public static Matrix4x4 Skew(Vector2 skew, Vector3 pivot) {
			var matrix4x4 = Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one);
			var num1 = Mathf.Sin(skew.x);
			var num2 = (float)Math.Cos(skew.x);
			var num3 = Mathf.Sin(skew.y);
			var num4 = (float)Math.Cos(skew.y);
			var m00 = matrix4x4.m00;
			var m01 = matrix4x4.m01;
			var m10 = matrix4x4.m10;
			var m11 = matrix4x4.m11;
			var num5 = (float)(m00 * (double)num4 + m10 * (double)num3);
			var num6 = (float)(m01 * (double)num4 + m11 * (double)num3);
			var num7 = (float)(m10 * (double)num2 - m00 * (double)num1);
			var num8 = (float)(m11 * (double)num2 - m01 * (double)num1);
			matrix4x4.m00 = num5;
			matrix4x4.m01 = num6;
			matrix4x4.m10 = num7;
			matrix4x4.m11 = num8;
			return Matrix4x4.TRS(pivot, Quaternion.identity, Vector3.one) * matrix4x4;
		}
	}
}