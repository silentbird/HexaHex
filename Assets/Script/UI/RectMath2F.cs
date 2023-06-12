// Decompiled with JetBrains decompiler
// Type: pure.utils.mathTools.RectMath2F
// Assembly: pure, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3647E74B-3271-4E18-8589-B6FF96DC8026
// Assembly location: D:\W\UnityProj\mhgd\proj_mhgd\Assets\Plugins\core\pure\Win\pure.dll

using System.Collections.Generic;
using UnityEngine;

namespace pure.utils.mathTools {
	public static class RectMath2F {
		private static readonly cw_comparer s_clock_wise = new();
		private static readonly Vector2[] rectV = new Vector2[4];

		public static Rect CaculateBoundary(IList<Vector3> points) {
			var x = float.PositiveInfinity;
			var y = float.PositiveInfinity;
			var num1 = float.NegativeInfinity;
			var num2 = float.NegativeInfinity;
			var index = 0;
			for (var count = points.Count; index < count; ++index) {
				var point = points[index];
				x = x > (double)point.x ? point.x : x;
				y = y > (double)point.y ? point.y : y;
				num1 = num1 < (double)point.x ? point.x : num1;
				num2 = num2 < (double)point.y ? point.y : num2;
			}

			return new Rect(x, y, num1 - x, num2 - y);
		}

		public static Rect CaculateBoundary(IList<Vector2> points) {
			if (points.Count == 0)
				return Rect.zero;
			var x = float.PositiveInfinity;
			var y = float.PositiveInfinity;
			var num1 = float.NegativeInfinity;
			var num2 = float.NegativeInfinity;
			var index = 0;
			for (var count = points.Count; index < count; ++index) {
				Vector3 point = points[index];
				x = x > (double)point.x ? point.x : x;
				y = y > (double)point.y ? point.y : y;
				num1 = num1 < (double)point.x ? point.x : num1;
				num2 = num2 < (double)point.y ? point.y : num2;
			}

			return new Rect(x, y, num1 - x, num2 - y);
		}

		public static Rect CaculateBoundary(IList<UIVertex> verts) {
			var x = float.PositiveInfinity;
			var y = float.PositiveInfinity;
			var num1 = float.NegativeInfinity;
			var num2 = float.NegativeInfinity;
			var index = 0;
			for (var count = verts.Count; index < count; ++index) {
				var position = verts[index].position;
				x = x > (double)position.x ? position.x : x;
				y = y > (double)position.y ? position.y : y;
				num1 = num1 < (double)position.x ? position.x : num1;
				num2 = num2 < (double)position.y ? position.y : num2;
			}

			return new Rect(x, y, num1 - x, num2 - y);
		}

		public static Rect CaculateUVArea(IList<UIVertex> verts) {
			var x = float.PositiveInfinity;
			var y = float.PositiveInfinity;
			var num1 = float.NegativeInfinity;
			var num2 = float.NegativeInfinity;
			var index = 0;
			for (var count = verts.Count; index < count; ++index) {
				Vector2 uv0 = verts[index].uv0;
				x = x > (double)uv0.x ? uv0.x : x;
				y = y > (double)uv0.y ? uv0.y : y;
				num1 = num1 < (double)uv0.x ? uv0.x : num1;
				num2 = num2 < (double)uv0.y ? uv0.y : num2;
			}

			return new Rect(x, y, num1 - x, num2 - y);
		}

		public static Rect Union(Rect a, Rect b) {
			return Rect.MinMaxRect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Max(a.xMax, b.xMax), Mathf.Max(a.yMax, b.yMax));
		}

		public static Rect GetDrawRect(Rect image, Rect area) {
			var num1 = image.width / image.height;
			var num2 = area.width / area.height;
			if (num1 > (double)num2) {
				var height = area.height;
				area.height = area.width * (1f / num1);
				area.y += (float)((height - (double)area.height) * 0.5);
			}
			else {
				var width = area.width;
				area.width = area.height * num1;
				area.x += (float)((width - (double)area.width) * 0.5);
			}

			return area;
		}

		public static bool Contains(Vector2[] controus, Vector2 p) {
			var length = controus.Length;
			for (var index = 0; index < length; ++index) {
				var controu1 = controus[index];
				var controu2 = controus[(index + 1) % length];
				if ((p - controu1).Sign(controu2 - controu1) > 0)
					return false;
			}

			return true;
		}

		private static void clock_wise(List<Vector2> list, Vector2 center) {
			s_clock_wise.center = center;
			list.Sort(s_clock_wise);
		}

		public static Rect Extend(this Rect rect, float w, float h) {
			rect.xMin -= w;
			rect.xMax += w;
			rect.yMin -= h;
			rect.yMax += h;
			return rect;
		}

		private static void fill(Rect rect) {
			rectV[0].Set(rect.x, rect.y);
			rectV[1].Set(rect.xMax, rect.y);
			rectV[2].Set(rect.xMax, rect.yMax);
			rectV[3].Set(rect.x, rect.yMax);
		}

		private static void AddVertex(IList<Vector2> list, Vector2 p, ref Vector2 center) {
			for (var index = 0; index < list.Count; ++index)
				if (list[index] == p)
					return;
			list.Add(p);
			center += p;
		}

		private class cw_comparer : IComparer<Vector2> {
			internal Vector2 center;

			int IComparer<Vector2>.Compare(Vector2 a, Vector2 b) {
				return (a - center).Sign(b - center) * -1;
			}
		}
	}
}