// Decompiled with JetBrains decompiler
// Type: pure.utils.mathTools.MatrixUtils
// Assembly: pure, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3647E74B-3271-4E18-8589-B6FF96DC8026
// Assembly location: D:\W\UnityProj\mhgd\proj_mhgd\Assets\Plugins\core\pure\Win\pure.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace pure.utils.mathTools
{
    public static class MatrixUtils
    {
        private static readonly Vector3[] helpV = new Vector3[4];

        public static void Rotate(ref Matrix4x4 matrix, float degree) =>
            MatrixUtils.Rotate(ref matrix, degree, Vector3.zero);

        public static void Rotate(ref Matrix4x4 matrix, float degree, Vector3 pivot) => matrix =
            Matrix4x4.TRS(pivot, Quaternion.Euler(0.0f, 0.0f, degree), Vector3.one) *
            Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) * matrix;

        public static void Rotate(ref Matrix4x4 matrix, Vector3 rotate, Vector3 pivot) => matrix =
            Matrix4x4.TRS(pivot, Quaternion.Euler(rotate), Vector3.one) *
            Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) * matrix;

        public static void Scale(ref Matrix4x4 matrix, Vector3 scale) =>
            MatrixUtils.Scale(ref matrix, scale, Vector3.zero);

        public static void Scale(ref Matrix4x4 matrix, Vector3 scale, Vector3 pivot) => matrix =
            Matrix4x4.TRS(pivot, Quaternion.identity, scale) * Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one) *
            matrix;

        public static void Translate(ref Matrix4x4 matrix, Vector3 translate) =>
            matrix = Matrix4x4.TRS(translate, Quaternion.identity, Vector3.one) * matrix;

        private static void FillRect(Rect r)
        {
            MatrixUtils.helpV[0] = new Vector3(r.x, r.y, 0.0f);
            MatrixUtils.helpV[1] = new Vector3(r.xMax, r.y, 0.0f);
            MatrixUtils.helpV[2] = new Vector3(r.xMax, r.yMax, 0.0f);
            MatrixUtils.helpV[3] = new Vector3(r.x, r.yMax, 0.0f);
        }

        public static Matrix4x4 BuildGradientBox(Rect rect, float rotate)
        {
            MatrixUtils.FillRect(rect);
            return MatrixUtils.BuildGradientBox(MatrixUtils.helpV, rotate);
        }

        public static Matrix4x4 BuildGradientBox(Vector3[] vs, float rotate)
        {
            Rect rect = RectMath2F.CaculateBoundary((IList<Vector3>) vs);
            Matrix4x4 identity = Matrix4x4.identity;
            MatrixUtils.Rotate(ref identity, rotate, new Vector3(rect.width * 0.5f, rect.height * 0.5f, 0.0f));
            int length = vs.Length;
            for (int index = 0; index < length; ++index)
                vs[index] = identity.MultiplyPoint(vs[index]);
            Vector3 vector3_1 = new Vector3(float.PositiveInfinity, float.PositiveInfinity, 0.0f);
            Vector3 vector3_2 = new Vector3(float.NegativeInfinity, float.NegativeInfinity, 0.0f);
            for (int index = 0; index < length; ++index)
            {
                Vector3 v = vs[index];
                if ((double) v.x < (double) vector3_1.x)
                    vector3_1.x = v.x;
                if ((double) v.x > (double) vector3_2.x)
                    vector3_2.x = v.x;
                if ((double) v.y < (double) vector3_1.y)
                    vector3_1.y = v.y;
                if ((double) v.y > (double) vector3_2.y)
                    vector3_2.y = v.y;
            }

            float num1 = (float) ((double) vector3_2.x - (double) vector3_1.x - 9.999999974752427E-07);
            float num2 = (float) ((double) vector3_2.y - (double) vector3_1.y - 9.999999974752427E-07);
            Vector3 scale = new Vector3(num1 / rect.width, num2 / rect.height, 1f);
            Vector3 center = (Vector3) rect.center;
            identity = Matrix4x4.identity;
            MatrixUtils.Scale(ref identity, scale, center);
            MatrixUtils.Rotate(ref identity, rotate, center);
            return identity;
        }

        public static Matrix4x4 CalcInnerbound(Rect rect, float rotate)
        {
            MatrixUtils.FillRect(rect);
            return MatrixUtils.CalcInnerbound(MatrixUtils.helpV, rotate);
        }

        public static Matrix4x4 CalcInnerbound(Vector3[] vs, float rotate) =>
            MatrixUtils.BuildGradientBox(vs, rotate).inverse;

        public static Matrix4x4 Skew(Vector2 skew) => MatrixUtils.Skew(skew, Vector3.zero);

        public static Matrix4x4 Skew(Vector2 skew, Vector3 pivot)
        {
            Matrix4x4 matrix4x4 = Matrix4x4.TRS(-pivot, Quaternion.identity, Vector3.one);
            float num1 = Mathf.Sin(skew.x);
            float num2 = (float) Math.Cos((double) skew.x);
            float num3 = Mathf.Sin(skew.y);
            float num4 = (float) Math.Cos((double) skew.y);
            float m00 = matrix4x4.m00;
            float m01 = matrix4x4.m01;
            float m10 = matrix4x4.m10;
            float m11 = matrix4x4.m11;
            float num5 = (float) ((double) m00 * (double) num4 + (double) m10 * (double) num3);
            float num6 = (float) ((double) m01 * (double) num4 + (double) m11 * (double) num3);
            float num7 = (float) ((double) m10 * (double) num2 - (double) m00 * (double) num1);
            float num8 = (float) ((double) m11 * (double) num2 - (double) m01 * (double) num1);
            matrix4x4.m00 = num5;
            matrix4x4.m01 = num6;
            matrix4x4.m10 = num7;
            matrix4x4.m11 = num8;
            return Matrix4x4.TRS(pivot, Quaternion.identity, Vector3.one) * matrix4x4;
        }
    }
}