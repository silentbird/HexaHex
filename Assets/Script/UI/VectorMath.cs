// Decompiled with JetBrains decompiler
// Type: pure.utils.mathTools.VectorMath
// Assembly: pure, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3647E74B-3271-4E18-8589-B6FF96DC8026
// Assembly location: D:\W\UnityProj\mhgd\proj_mhgd\Assets\Plugins\core\pure\Win\pure.dll

using System;
using System.Text;
using UnityEngine;

namespace pure.utils.mathTools
{
  public static class VectorMath
  {
    public static readonly Vector2[] uvs = new Vector2[4];
    public static readonly Vector3[] verts = new Vector3[4];
    public static readonly UIVertex[] vertex = new UIVertex[4];
    public static readonly int[] quadIndics = new int[6];
    public const float RAD_DEGREE = 57.295776f;
    public const float DEGREE_RAD = 0.017453292f;
    private static StringBuilder helper = new StringBuilder();

    public static Vector2 Truncate(this Vector2 v, float max) => Vector2.ClampMagnitude(v, max);

    public static float DistanceToSegment(this Vector2 v, Vector2 a, Vector2 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      if ((double) num < 0.0)
        return (v - a).magnitude;
      return (double) num > 1.0 ? (v - b).magnitude : (v - (a + num * (b - a))).magnitude;
    }

    public static float SqrDistanceToSegment(this Vector2 v, Vector2 a, Vector2 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      if ((double) num < 0.0)
        return (v - a).sqrMagnitude;
      return (double) num > 1.0 ? (v - b).sqrMagnitude : (v - (a + num * (b - a))).sqrMagnitude;
    }

    public static float DistanceToLine(this Vector2 v, Vector2 a, Vector2 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      return (v - (a + num * (b - a))).magnitude;
    }

    public static float SqrDistanceToLine(this Vector2 v, Vector2 a, Vector2 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      return (v - (a + num * (b - a))).sqrMagnitude;
    }

    public static Vector3 NearestToSegment(this Vector3 v, Vector3 a, Vector3 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      if ((double) num < 0.0)
        return a;
      return (double) num > 1.0 ? b : a + num * (b - a);
    }

    public static float DistanceToSegment(this Vector3 v, Vector3 a, Vector3 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      if ((double) num < 0.0)
        return (v - a).magnitude;
      return (double) num > 1.0 ? (v - b).magnitude : (v - (a + num * (b - a))).magnitude;
    }

    public static Vector3 NearestToLine(this Vector3 v, Vector3 a, Vector3 b)
    {
      float num = (v - a).Dot(b - a) / (b - a).sqrMagnitude;
      return a + num * (b - a);
    }

    public static float Dot(this Vector3 v, Vector3 a) => Vector3.Dot(v, a);

    public static float LeftOf(this Vector2 c, Vector2 a, Vector2 b) => (a - c).Cross(b - a);

    public static int Sign(this Vector2 a, Vector2 b)
    {
      float num = a.Cross(b);
      if ((double) num < 0.0)
        return -1;
      return !num.Equals(0.0f) ? 1 : 0;
    }

    public static bool IsParrallel(this Vector2 a, Vector2 b)
    {
      a.Normalize();
      b.Normalize();
      if (a.x.Equals(b.x) && a.y.Equals(b.y))
        return true;
      return a.x.Equals(-b.x) && a.y.Equals(-b.y);
    }

    public static Vector2 GetPerp(this Vector2 v) => new Vector2(-v.y, v.x);

    public static Vector3 Round(this Vector3 v, int digit) => new Vector3((float) Math.Round((double) v.x, digit), (float) Math.Round((double) v.y, digit), (float) Math.Round((double) v.z, digit));

    public static Vector2 Rotate(this Vector2 v, float degree)
    {
      float f = degree * ((float) Math.PI / 180f);
      float num1 = Mathf.Sin(f);
      float num2 = Mathf.Cos(f);
      return new Vector2((float) ((double) v.y * (double) num1 - (double) v.x * (double) num2), (float) ((double) v.x * (double) num1 + (double) v.y * (double) num2));
    }

    private static Vector3 rotate_to_rad(this Vector3 v, float rad)
    {
      float num1 = Mathf.Sin(rad);
      float num2 = Mathf.Cos(rad);
      return new Vector3((float) ((double) v.x * (double) num2 + (double) v.z * (double) num1), v.y, (float) (-(double) v.x * (double) num1 + (double) v.z * (double) num2));
    }

    public static Vector3 Rotate(this Vector3 v, float degree) => v.rotate_to_rad(degree * ((float) Math.PI / 180f));

    public static Vector3 RotateTo(this Vector3 v, Vector3 dir)
    {
      float rad = Mathf.Atan2(dir.x, dir.z);
      return v.rotate_to_rad(rad);
    }

    public static float Dot(this Vector2 a, Vector2 b) => Vector2.Dot(a, b);

    public static float Cross(this Vector2 a, Vector2 b) => (float) ((double) a.x * (double) b.y - (double) a.y * (double) b.x);

    public static float AngleTo(this Vector2 a, Vector2 b)
    {
      float magnitude1 = a.magnitude;
      float magnitude2 = b.magnitude;
      float f = a.Dot(b) / (magnitude1 * magnitude2);
      if ((double) f < -1.0)
        f = -1f;
      if ((double) f > 1.0)
        f = 1f;
      return Mathf.Acos(f);
    }

    public static float DegreeTo(this Vector2 a, Vector2 b) => a.AngleTo(b) * 57.295776f;

    public static float PerpDotOf(this Vector2 a, Vector2 b) => a.GetPerp().Dot(b);

    public static Vector2 Projection(this Vector2 a, Vector2 b) => b * (a.Dot(b) / b.Dot(b));

    public static bool Inside(this Vector2 a, Vector2 topLeft, Vector2 botRight) => (double) a.x >= (double) topLeft.x && (double) a.x <= (double) topLeft.x + (double) botRight.x && (double) a.y >= (double) topLeft.y && (double) a.y <= (double) topLeft.y + (double) botRight.y;

    public static float CalcOrientV2(Vector2 dir) => Mathf.Atan2(dir.x, dir.y) * 57.29578f;

    public static float CalcOrientation(Vector3 dir) => (double) dir.sqrMagnitude < 9.999999747378752E-06 ? 0.0f : Mathf.Atan2(dir.x, dir.z) * 57.29578f;

    public static bool InCamera(Camera cam, Vector3 position)
    {
      Vector3 viewportPoint = cam.WorldToViewportPoint(position);
      return new Rect(0.0f, 0.0f, 1f, 1f).Contains(viewportPoint) && (double) viewportPoint.z >= (double) cam.nearClipPlane && (double) viewportPoint.z <= (double) cam.farClipPlane;
    }

    public static bool InCamera(Camera cam, Bounds bound)
    {
      Vector3 viewportPoint1 = cam.WorldToViewportPoint(bound.min);
      Vector3 viewportPoint2 = cam.WorldToViewportPoint(bound.max);
      return new Rect((Vector2) viewportPoint1, (Vector2) (viewportPoint2 - viewportPoint1)).Overlaps(new Rect(0.0f, 0.0f, 1f, 1f)) && (double) viewportPoint1.z < (double) cam.farClipPlane && (double) viewportPoint2.z > (double) cam.nearClipPlane;
    }


    public static Vector3 Truncate(this Vector3 v, float min, float max)
    {
      if ((double) v.magnitude < (double) min)
        v = v.normalized * min;
      else if ((double) v.magnitude > (double) max)
        v = v.normalized * max;
      return v;
    }

    public static void CalcPolyCenter(Vector3[] vs, out Vector3 center)
    {
      center = new Vector3();
      int length = vs.Length;
      for (int index = 0; index < length; ++index)
        center += vs[index];
      center /= (float) length;
    }

    public static void PrintVectors(Vector3[] vs)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (Vector3 v in vs)
        stringBuilder.AppendLine(v.ToString());
      Debug.Log((object) stringBuilder.ToString());
    }

    public static Vector2 ToVec2(this Vector3 pos) => new Vector2(pos.x, pos.z);

    public static Vector3 ToVec3(this Vector2 pos) => new Vector3(pos.x, 0.0f, pos.y);

    public static bool IsNaN(this Vector3 v) => float.IsNaN(v.x) || float.IsNaN(v.y) || float.IsNaN(v.z);

    public static bool IsNaN(this Vector2 v) => float.IsNaN(v.x) || float.IsNaN(v.y);

    public static Vector3 RoundToInt(this Vector3 v) => new Vector3((float) Mathf.RoundToInt(v.x), (float) Mathf.RoundToInt(v.y), (float) Mathf.RoundToInt(v.z));

    public static Vector2 RoundToInt(this Vector2 v) => new Vector2((float) Mathf.RoundToInt(v.x), (float) Mathf.RoundToInt(v.y));

    public static void CalcUp(Vector3 dir, out Vector3 right, out Vector3 up)
    {
      up = (double) Mathf.Abs(dir.y) > 0.9990000128746033 ? Vector3.forward : Vector3.up;
      right = Vector3.Cross(up, dir).normalized;
      up = Vector3.Cross(dir, right);
    }

    public static bool IsEqual(this Vector3 a, Vector3 b, float sqrEpilon = 1E-05f) => (double) (a - b).sqrMagnitude < (double) sqrEpilon;

    public static bool IsEqual(this Vector2 a, Vector2 b, float sqrEpilon = 1E-05f) => (double) (a - b).sqrMagnitude < (double) sqrEpilon;

    public static bool IsEqual(this Bounds a, Bounds b, float sqrEpilon = 1E-05f) => a.center.IsEqual(b.center, sqrEpilon) && a.extents.IsEqual(b.extents, sqrEpilon);

    public static Vector2 ScreenToUI(Vector2 screen, RectTransform rect, Camera canvasCam)
    {
      Vector2 localPoint;
      if ((bool) (UnityEngine.Object) canvasCam && RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screen, canvasCam, out localPoint))
        return localPoint;
      Rect rect1 = rect.rect;
      screen.x -= (float) Screen.width * 0.5f;
      screen.y -= (float) Screen.height * 0.5f;
      localPoint = new Vector2()
      {
        x = screen.x / (float) Screen.width * rect1.width,
        y = screen.y / (float) Screen.height * rect1.height
      };
      return localPoint;
    }

    private static bool TryToScreenAnchor(
      Vector2 screenPos,
      RectTransform rect,
      Camera cam,
      out Vector2 anchor)
    {
      if ((bool) (UnityEngine.Object) rect)
      {
        if (!(bool) (UnityEngine.Object) cam)
        {
          Canvas componentInParent = rect.GetComponentInParent<Canvas>();
          cam = (bool) (UnityEngine.Object) componentInParent ? componentInParent.worldCamera : (Camera) null;
        }
        if ((bool) (UnityEngine.Object) cam && RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, cam, out anchor))
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
      out Vector2 anchor)
    {
      return !(bool) (UnityEngine.Object) uiCamera ? VectorMath.TryWorldToUI(position, worldCam, rect, out anchor) : VectorMath.TryToScreenAnchor((Vector2) worldCam.WorldToScreenPoint(position), rect, uiCamera, out anchor);
    }

    public static bool TryWorldToUI(
      Vector3 position,
      Camera worldCam,
      RectTransform rect,
      out Vector2 anchor)
    {
      if ((bool) (UnityEngine.Object) worldCam)
      {
        Rect rect1 = rect.rect;
        Vector2 screenPoint = (Vector2) worldCam.WorldToScreenPoint(position);
        screenPoint.x -= (float) Screen.width * 0.5f;
        screenPoint.y -= (float) Screen.height * 0.5f;
        anchor = new Vector2()
        {
          x = screenPoint.x / (float) Screen.width * rect1.width,
          y = screenPoint.y / (float) Screen.height * rect1.height
        };
        return true;
      }
      anchor = Vector2.one;
      return false;
    }

    public static bool TryWorldToUI(Vector3 position, RectTransform rect, out Vector2 anchor)
    {
      Camera worldCam = Camera.main;
      return VectorMath.TryWorldToUI(position, worldCam, rect, out anchor);
    }

    public static string VString(Vector3 v) => string.Format("({0},{1},{2})", (object) v.x, (object) v.y, (object) v.z);

    public static string ToString(Vector2[] vs)
    {
      VectorMath.helper.Length = 0;
      VectorMath.helper.Append("[ ");
      for (int index = 0; index < vs.Length; ++index)
      {
        VectorMath.helper.Append((object) vs[index]);
        VectorMath.helper.Append(index == vs.Length - 1 ? " ]" : " , ");
      }
      return VectorMath.helper.ToString();
    }

    public static string ToString(Vector3[] vs)
    {
      VectorMath.helper.Length = 0;
      VectorMath.helper.Append("[ ");
      for (int index = 0; index < vs.Length; ++index)
      {
        VectorMath.helper.Append((object) vs[index]);
        VectorMath.helper.Append(index == vs.Length - 1 ? " ]" : " , ");
      }
      return VectorMath.helper.ToString();
    }

    public static string ToString(Vector3 v) => string.Format("({0}f,{1}f,{2}f)", (object) v.x, (object) v.y, (object) v.z);

    public static string ToStringF(this Vector3 v) => string.Format("({0}f,{1}f,{2}f)", (object) v.x, (object) v.y, (object) v.z);

    public static string ToStringF(this Vector2 v) => string.Format("({0}f,{1}f)", (object) v.x, (object) v.y);

    public static string ToStringF(this Vector4 v) => string.Format("({0:F}f,{1:F}f,{2:F}f,{3:F}f)", (object) v.x, (object) v.y, (object) v.z, (object) v.w);

    public static string ToStringF(this Quaternion v) => string.Format("({0}f,{1}f,{2}f,{3}f)", (object) v.x, (object) v.y, (object) v.z, (object) v.w);

    public static bool IsNaN(ref Matrix4x4 matrix)
    {
      for (int index = 0; index < 16; ++index)
      {
        if (float.IsNaN(matrix[index]))
          return true;
      }
      return false;
    }

    public static bool IsNaN(this Matrix4x4 matrix) => VectorMath.IsNaN(ref matrix);

    public static int IntDegree(this float orient, int segments)
    {
      float num1 = 360f / (float) segments;
      float num2 = num1 * 0.5f;
      int num3 = (int) Mathf.Repeat((float) (int) ((double) Mathf.Repeat(orient + num2, 360f) / (double) num1), (float) segments);
      return (int) (num1 * (float) num3);
    }

    public static Vector3 IntDegree(this Vector3 dir, int segments)
    {
      float magnitude = dir.magnitude;
      dir = dir.normalized;
      double num = (double) ((float) (Math.Atan2((double) dir.z, (double) dir.x) * 57.2957763671875)).IntDegree(segments) * (Math.PI / 180.0);
      dir.x = (float) Math.Cos(num);
      dir.z = (float) Math.Sin(num);
      dir *= magnitude;
      return dir;
    }

    public static float LimitAngle(float orient, int angles, float zeroOffset)
    {
      float num = 360f / (float) angles;
      return Mathf.Floor((float) ((double) orient + (double) zeroOffset - (double) num * 0.5) / num) * num + zeroOffset;
    }
  }
}
