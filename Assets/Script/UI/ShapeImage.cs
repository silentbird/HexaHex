using pure.ui.imageFiller;
using pure.utils.mathTools;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace pure.ui.image
{
    [AddComponentMenu("UI/PShape", 35), ExecuteInEditMode]
    public class ShapeImage : ImageCore
    {
        [SerializeField] private ImageShape _shape;
        [SerializeField] private Vector2 _ellipse = Vector2.zero;
        [SerializeField] private Vector2 _skew = Vector2.zero;
        [SerializeField] private bool _fillCenter = true;

        [SerializeField] [FormerlySerializedAs("_keepAspect")]
        private bool _preserveAspect = true;

        [SerializeField] private bool _clockWise = true;
        [SerializeField] [Range(0.1f, 100f)] private float _thickness = 10f;
        [SerializeField] [Range(0.0f, 1f)] private float _fillAmount = 1f;
        [SerializeField] [Range(3f, 60f)] private int _numEdge = 20;
        [SerializeField] [Range(0.0f, 360f)] private float _startAngle;
        [SerializeField] private RectTransform _surround;
        [SerializeField] private Vector4 _surroundOffset;

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            ShapeImage.shapedraw_rect shapedrawRect;
            if (!(bool) (UnityEngine.Object) this.activeSprite)
            {
                toFill.Clear();
            }
            else
            {
                switch (this.shape)
                {
                    case ImageShape.Rect:
                        shapedrawRect = new ShapeImage.shapedraw_rect();
                        shapedrawRect.Fill(toFill, this);
                        break;
                    case ImageShape.Round:
                        new ShapeImage.shapedraw_round().Fill(toFill, this);
                        break;
                    case ImageShape.RoundRect:
                        new ShapeImage.shapedraw_ellipse().Fill(toFill, this);
                        break;
                    case ImageShape.Surround:
                        if ((bool) (UnityEngine.Object) this._surround)
                        {
                            new ShapeImage.shapedraw_surround().Fill(toFill, this);
                            break;
                        }

                        shapedrawRect = new ShapeImage.shapedraw_rect();
                        shapedrawRect.Fill(toFill, this);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public ImageShape shape
        {
            get => this._shape;
            set
            {
                if (this._shape == value)
                    return;
                this._shape = value;
                this.SetVerticesDirty();
            }
        }

        public Vector2 ellipse
        {
            get => this._ellipse;
            set
            {
                if (this._ellipse == value)
                    return;
                this._ellipse = value;
                this.SetVerticesDirty();
            }
        }

        public Vector2 skew
        {
            get => this._skew;
            set
            {
                if (this._skew == value)
                    return;
                this._skew = value;
                this.SetVerticesDirty();
            }
        }

        public bool fillCenter
        {
            get => this._fillCenter;
            set
            {
                if (this._fillCenter == value)
                    return;
                this._fillCenter = value;
                this.SetVerticesDirty();
            }
        }

        public bool preserveAspect
        {
            get => this._preserveAspect;
            set
            {
                if (this._preserveAspect == value)
                    return;
                this._preserveAspect = value;
                this.SetVerticesDirty();
            }
        }

        public bool clockWise
        {
            get => this._clockWise;
            set
            {
                if (this._clockWise == value)
                    return;
                this._clockWise = value;
                this.SetVerticesDirty();
            }
        }

        public float thickness
        {
            get => this._thickness;
            set
            {
                if (value.Equals(this._thickness))
                    return;
                this._thickness = value;
                this.SetVerticesDirty();
            }
        }

        public float fillAmount
        {
            get => this._fillAmount;
            set
            {
                value = Mathf.Clamp01(value);
                if ((double) Mathf.Abs(value - this._fillAmount) <= 9.999999747378752E-05)
                    return;
                this._fillAmount = value;
                this.SetVerticesDirty();
            }
        }

        public int numEdge
        {
            get => this._numEdge;
            set
            {
                value = value < 3 ? 3 : value;
                if (this._numEdge == value)
                    return;
                this._numEdge = value;
                this.SetVerticesDirty();
            }
        }

        public float startAngle
        {
            get => this._startAngle;
            set
            {
                value = Mathf.Clamp(value, 0.0f, 360f);
                if ((double) Mathf.Abs(this._startAngle - value) < 9.999999747378752E-05)
                    return;
                this._startAngle = value;
                this.SetVerticesDirty();
            }
        }

        public RectTransform surround
        {
            get => this._surround;
            set
            {
                if (!((UnityEngine.Object) this._surround != (UnityEngine.Object) value))
                    return;
                this._surround = value;
                this.SetVerticesDirty();
            }
        }

        public Vector4 surroundOffset
        {
            get => this._surroundOffset;
            set
            {
                if (!(this._surroundOffset != value))
                    return;
                this._surroundOffset = value;
                this.SetVerticesDirty();
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct shapedraw_ellipse
        {
            internal void Fill(VertexHelper toFill, ShapeImage image)
            {
                toFill.Clear();
                Color color = image.color;
                int currentVertCount1 = toFill.currentVertCount;
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                Vector4 outerUv = image.GetOuterUV();
                Matrix4x4 matrix = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                Vector2 ellipse = image._ellipse;
                ShapeImage.shapedraw_ellipse.move_to(toFill,
                    new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.line_to(toFill,
                    new Vector3(drawingDimensions.z - ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.curve_to(toFill, new Vector3(drawingDimensions.z, drawingDimensions.y),
                    new Vector3(drawingDimensions.z, drawingDimensions.y + ellipse.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.line_to(toFill,
                    new Vector3(drawingDimensions.z, drawingDimensions.w - ellipse.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.curve_to(toFill, new Vector3(drawingDimensions.z, drawingDimensions.w),
                    new Vector3(drawingDimensions.z - ellipse.x, drawingDimensions.w), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.line_to(toFill,
                    new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.w), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.curve_to(toFill, new Vector3(drawingDimensions.x, drawingDimensions.w),
                    new Vector3(drawingDimensions.x, drawingDimensions.w - ellipse.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.line_to(toFill,
                    new Vector3(drawingDimensions.x, drawingDimensions.y + ellipse.y), drawingDimensions, outerUv,
                    color, matrix);
                ShapeImage.shapedraw_ellipse.curve_to(toFill, new Vector3(drawingDimensions.x, drawingDimensions.y),
                    new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
                    color, matrix);
                int currentVertCount2 = toFill.currentVertCount;
                int num = currentVertCount2 - currentVertCount1;
                for (int index = currentVertCount1; index <= currentVertCount2; ++index)
                    toFill.AddTriangle(currentVertCount1, (index + 2 - currentVertCount1) % num,
                        (index + 1 - currentVertCount1) % num);
            }

            private static void move_to(
                VertexHelper vh,
                Vector3 pos,
                Vector4 dimentions,
                Vector4 uvs,
                Color color,
                Matrix4x4 matrix)
            {
                if (matrix != Matrix4x4.identity)
                    pos = matrix.MultiplyPoint(pos);
                Vector2 uv = GraphicTools.TransferUV((Vector2) pos, dimentions, uvs);
                GraphicTools.MoveTo(vh, pos, uv, color);
            }

            private static void line_to(
                VertexHelper vh,
                Vector3 pos,
                Vector4 dimentions,
                Vector4 uvs,
                Color color,
                Matrix4x4 matrix)
            {
                if (matrix != Matrix4x4.identity)
                    pos = matrix.MultiplyPoint(pos);
                Vector2 uv = GraphicTools.TransferUV((Vector2) pos, dimentions, uvs);
                GraphicTools.LineTo(vh, pos, uv, color);
            }

            private static void curve_to(
                VertexHelper vh,
                Vector3 control,
                Vector3 to,
                Vector4 dimentions,
                Vector4 uvs,
                Color color,
                Matrix4x4 matrix)
            {
                if (matrix != Matrix4x4.identity)
                {
                    control = matrix.MultiplyPoint(control);
                    to = matrix.MultiplyPoint(to);
                }

                Vector2 uvNext = GraphicTools.TransferUV((Vector2) to, dimentions, uvs);
                Vector2 uvControl = GraphicTools.TransferUV((Vector2) control, dimentions, uvs);
                GraphicTools.CurveTo(vh, (Vector2) control, to, uvControl, uvNext, color);
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct shapedraw_rect
        {
            internal void Fill(VertexHelper toFill, ShapeImage image)
            {
                toFill.Clear();
                if (image._fillCenter)
                    ShapeImage.shapedraw_rect.draw_rect(toFill, image);
                else
                    ShapeImage.shapedraw_rect.draw_rect(toFill, image, image._thickness);
            }

            private static void draw_rect(VertexHelper toFill, ShapeImage image)
            {
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                Vector4 outerUv = image.GetOuterUV();
                Matrix4x4 matrix = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                ShapeImage.shapedraw_rect.add_quad(toFill, drawingDimensions, outerUv, drawingDimensions,
                    image.color, matrix);
            }

            private static void draw_rect(VertexHelper toFill, ShapeImage image, float t)
            {
                if ((double) t <= 9.999999747378752E-05)
                    return;
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                if ((double) t >= ((double) drawingDimensions.z - (double) drawingDimensions.x) * 0.5 &&
                    (double) t >= ((double) drawingDimensions.w - (double) drawingDimensions.y) * 0.5)
                {
                    ShapeImage.shapedraw_rect.draw_rect(toFill, image);
                }
                else
                {
                    Vector4 outerUv = image.GetOuterUV();
                    Matrix4x4 matrix = image.skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                    Color color = image.color;
                    ShapeImage.shapedraw_rect.add_quad(toFill,
                        new Vector4(drawingDimensions.x, drawingDimensions.y, drawingDimensions.x + t,
                            drawingDimensions.w), outerUv, drawingDimensions, color, matrix);
                    ShapeImage.shapedraw_rect.add_quad(toFill,
                        new Vector4(drawingDimensions.x + t, drawingDimensions.y, drawingDimensions.z - t,
                            drawingDimensions.y + t), outerUv, drawingDimensions, color, matrix);
                    ShapeImage.shapedraw_rect.add_quad(toFill,
                        new Vector4(drawingDimensions.z - t, drawingDimensions.y, drawingDimensions.z,
                            drawingDimensions.w), outerUv, drawingDimensions, color, matrix);
                    ShapeImage.shapedraw_rect.add_quad(toFill,
                        new Vector4(drawingDimensions.x + t, drawingDimensions.w - t, drawingDimensions.z - t,
                            drawingDimensions.w), outerUv, drawingDimensions, color, matrix);
                }
            }

            private static void add_quad(
                VertexHelper toFill,
                Vector4 area,
                Vector4 uv4,
                Vector4 dimensions,
                Color color,
                Matrix4x4 matrix)
            {
                Color32 color32 = (Color32) color;
                Vector3[] verts = VectorMath.verts;
                UIVertex[] vertex = VectorMath.vertex;
                verts[0] = new Vector3(area.x, area.y);
                verts[1] = new Vector3(area.x, area.w);
                verts[2] = new Vector3(area.z, area.w);
                verts[3] = new Vector3(area.z, area.y);
                bool flag = matrix == Matrix4x4.identity;
                for (int index = 0; index < 4; ++index)
                {
                    Vector3 position = verts[index];
                    vertex[index].uv0 = GraphicTools.TransferUV((Vector2) position, dimensions, uv4);
                    vertex[index].position = flag ? verts[index] : matrix.MultiplyPoint(verts[index]);
                    vertex[index].color = color32;
                }

                toFill.AddUIVertexQuad(vertex);
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct shapedraw_round
        {
            internal void Fill(VertexHelper toFill, ShapeImage image)
            {
                toFill.Clear();
                if ((double) image._fillAmount < 1.0 / (double) image.numEdge)
                    return;
                if (image._fillCenter)
                    ShapeImage.shapedraw_round.draw(toFill, image);
                else
                    ShapeImage.shapedraw_round.draw(toFill, image, image._thickness);
            }

            private static void draw(VertexHelper toFill, ShapeImage image)
            {
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                Color32 color = (Color32) image.color;
                float num1 = drawingDimensions.z - drawingDimensions.x;
                float num2 = drawingDimensions.w - drawingDimensions.y;
                float num3 = (double) num1 > (double) num2 ? num2 * 0.5f : num1 * 0.5f;
                Vector2 point = new Vector2(drawingDimensions.x + num1 * 0.5f, drawingDimensions.y + num2 * 0.5f);
                Vector4 outerUv = image.GetOuterUV();
                Matrix4x4 matrix4x4 = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                Vector2 position1 = (Vector2) matrix4x4.MultiplyPoint((Vector3) point);
                int numEdge = image._numEdge;
                float num4 = 6.2831855f / (float) numEdge;
                if ((double) image._fillAmount < (double) num4)
                    return;
                UIVertex v = new UIVertex()
                {
                    position = (Vector3) position1,
                    uv0 = GraphicTools.TransferUV(position1, drawingDimensions, outerUv),
                    color = color
                };
                toFill.AddVert(v);
                float num5 = image._startAngle * ((float) Math.PI / 180f);
                float num6 = (float) ((double) image._fillAmount * 3.1415927410125732 * 2.0);
                for (int idx2 = 0; idx2 <= numEdge; ++idx2)
                {
                    float num7 = num4 * (float) idx2;
                    float f = image._clockWise ? num5 - num7 : num5 + num7;
                    float num8 = Mathf.Cos(f);
                    float num9 = Mathf.Sin(f);
                    Vector3 position2 =
                        matrix4x4.MultiplyPoint(new Vector3(position1.x + num8 * num3, position1.y + num9 * num3,
                            0.0f));
                    Vector2 uv0 = GraphicTools.TransferUV((Vector2) position2, drawingDimensions, outerUv);
                    toFill.AddVert(position2, color, uv0);
                    if (idx2 >= 1)
                        toFill.AddTriangle(0, idx2 - 1, idx2);
                    if ((double) num7 > (double) num6)
                        break;
                }

                if ((double) image._fillAmount < 1.0)
                    return;
                toFill.AddTriangle(0, numEdge, 1);
            }

            private static void draw(VertexHelper toFill, ShapeImage image, float thick)
            {
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                Color32 color = (Color32) image.color;
                Matrix4x4 matrix4x4 = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                float num1 = drawingDimensions.z - drawingDimensions.x;
                float num2 = drawingDimensions.w - drawingDimensions.y;
                float num3 = (double) num1 > (double) num2 ? num2 * 0.5f : num1 * 0.5f;
                float num4 = num3 - thick;
                Vector2 point = new Vector2(drawingDimensions.x + num1 * 0.5f, drawingDimensions.y + num2 * 0.5f);
                Vector2 vector2_1 = (Vector2) matrix4x4.MultiplyPoint((Vector3) point);
                int numEdge = image._numEdge;
                float num5 = 6.2831855f / (float) numEdge;
                Vector4 outerUv = image.GetOuterUV();
                UIVertex[] vertex = VectorMath.vertex;
                float num6 = (float) ((double) image._fillAmount * 3.1415927410125732 * 2.0);
                float num7 = image._startAngle * ((float) Math.PI / 180f);
                for (int index = 0; index <= numEdge; ++index)
                {
                    float num8 = num5 * (float) index;
                    float f = image._clockWise ? num7 - num8 : num7 + num8;
                    float num9 = Mathf.Cos(f);
                    float num10 = Mathf.Sin(f);
                    Vector3 position1 = matrix4x4.MultiplyPoint(new Vector3(vector2_1.x + num9 * num3,
                        vector2_1.y + num10 * num3, 0.0f));
                    Vector2 vector2_2 = GraphicTools.TransferUV((Vector2) position1, drawingDimensions, outerUv);
                    Vector3 position2 = matrix4x4.MultiplyPoint(new Vector3(vector2_1.x + num9 * num4,
                        vector2_1.y + num10 * num4, 0.0f));
                    Vector2 vector2_3 = GraphicTools.TransferUV((Vector2) position2, drawingDimensions, outerUv);
                    UIVertex[] uiVertexArray1 = vertex;
                    UIVertex uiVertex1 = new UIVertex();
                    uiVertex1.position = position1;
                    uiVertex1.uv0 = vector2_2;
                    uiVertex1.color = color;
                    UIVertex uiVertex2 = uiVertex1;
                    uiVertexArray1[2] = uiVertex2;
                    UIVertex[] uiVertexArray2 = vertex;
                    uiVertex1 = new UIVertex();
                    uiVertex1.position = position2;
                    uiVertex1.uv0 = vector2_3;
                    uiVertex1.color = color;
                    UIVertex uiVertex3 = uiVertex1;
                    uiVertexArray2[3] = uiVertex3;
                    if (index >= 1)
                        toFill.AddUIVertexQuad(vertex);
                    vertex[0] = vertex[3];
                    vertex[1] = vertex[2];
                    if ((double) num8 > (double) num6)
                        break;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        internal struct shapedraw_surround
        {
            internal void Fill(VertexHelper toFill, ShapeImage image)
            {
                toFill.Clear();
                ShapeImage.shapedraw_surround.draw_rect(toFill, image);
            }

            private static void draw_rect(VertexHelper toFill, ShapeImage image)
            {
                Rect rect = image._surround.rect;
                Vector4 drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
                Vector4 vector4 = new Vector4(rect.x, rect.y, rect.xMax, rect.yMax);
                Vector4 surroundOffset = image._surroundOffset;
                vector4.x += surroundOffset.x;
                vector4.y += surroundOffset.y;
                vector4.z -= surroundOffset.w;
                vector4.w -= surroundOffset.z;
                Vector4 outerUv = image.GetOuterUV();
                Matrix4x4 matrix = image.skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
                Color color = image.color;
                ShapeImage.shapedraw_surround.add_quad(toFill,
                    new Vector4(drawingDimensions.x, drawingDimensions.y, vector4.x, drawingDimensions.w), outerUv,
                    drawingDimensions, color, matrix);
                ShapeImage.shapedraw_surround.add_quad(toFill,
                    new Vector4(vector4.x, drawingDimensions.y, vector4.z, vector4.y), outerUv, drawingDimensions,
                    color, matrix);
                ShapeImage.shapedraw_surround.add_quad(toFill,
                    new Vector4(vector4.z, drawingDimensions.y, drawingDimensions.z, drawingDimensions.w), outerUv,
                    drawingDimensions, color, matrix);
                ShapeImage.shapedraw_surround.add_quad(toFill,
                    new Vector4(vector4.x, vector4.w, vector4.z, drawingDimensions.w), outerUv, drawingDimensions,
                    color, matrix);
            }

            private static void add_quad(
                VertexHelper toFill,
                Vector4 area,
                Vector4 uv4,
                Vector4 dimensions,
                Color color,
                Matrix4x4 matrix)
            {
                if ((double) area.sqrMagnitude < 0.001)
                    return;
                Color32 color32 = (Color32) color;
                Vector3[] verts = VectorMath.verts;
                UIVertex[] vertex = VectorMath.vertex;
                verts[0] = new Vector3(area.x, area.y);
                verts[1] = new Vector3(area.x, area.w);
                verts[2] = new Vector3(area.z, area.w);
                verts[3] = new Vector3(area.z, area.y);
                bool flag = matrix == Matrix4x4.identity;
                for (int index = 0; index < 4; ++index)
                {
                    Vector3 position = verts[index];
                    vertex[index].uv0 = GraphicTools.TransferUV((Vector2) position, dimensions, uv4);
                    vertex[index].position = flag ? verts[index] : matrix.MultiplyPoint(verts[index]);
                    vertex[index].color = color32;
                }

                toFill.AddUIVertexQuad(vertex);
            }
        }
    }
}