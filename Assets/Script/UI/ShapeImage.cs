using System;
using System.Runtime.InteropServices;
using pure.ui.imageFiller;
using pure.utils.mathTools;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace pure.ui.image {
	[AddComponentMenu("UI/PShape", 35), ExecuteInEditMode]
	public class ShapeImage : ImageCore {
		[SerializeField]
		private ImageShape _shape;

		[SerializeField]
		private Vector2 _ellipse = Vector2.zero;

		[SerializeField]
		private Vector2 _skew = Vector2.zero;

		[SerializeField]
		private bool _fillCenter = true;

		[SerializeField, FormerlySerializedAs("_keepAspect")] 
		private bool _preserveAspect = true;

		[SerializeField]
		private bool _clockWise = true;

		[SerializeField, Range(0.1f, 100f)] 
		private float _thickness = 10f;

		[SerializeField, Range(0.0f, 1f)] 
		private float _fillAmount = 1f;

		[SerializeField, Range(3f, 60f)] 
		private int _numEdge = 20;

		[SerializeField, Range(0.0f, 360f)] 
		private float _startAngle;

		[SerializeField]
		private RectTransform _surround;

		[SerializeField]
		private Vector4 _surroundOffset;

		public ImageShape shape {
			get => _shape;
			set {
				if (_shape == value)
					return;
				_shape = value;
				SetVerticesDirty();
			}
		}

		public Vector2 ellipse {
			get => _ellipse;
			set {
				if (_ellipse == value)
					return;
				_ellipse = value;
				SetVerticesDirty();
			}
		}

		public Vector2 skew {
			get => _skew;
			set {
				if (_skew == value)
					return;
				_skew = value;
				SetVerticesDirty();
			}
		}

		public bool fillCenter {
			get => _fillCenter;
			set {
				if (_fillCenter == value)
					return;
				_fillCenter = value;
				SetVerticesDirty();
			}
		}

		public bool preserveAspect {
			get => _preserveAspect;
			set {
				if (_preserveAspect == value)
					return;
				_preserveAspect = value;
				SetVerticesDirty();
			}
		}

		public bool clockWise {
			get => _clockWise;
			set {
				if (_clockWise == value)
					return;
				_clockWise = value;
				SetVerticesDirty();
			}
		}

		public float thickness {
			get => _thickness;
			set {
				if (value.Equals(_thickness))
					return;
				_thickness = value;
				SetVerticesDirty();
			}
		}

		public float fillAmount {
			get => _fillAmount;
			set {
				value = Mathf.Clamp01(value);
				if (Mathf.Abs(value - _fillAmount) <= 9.999999747378752E-05)
					return;
				_fillAmount = value;
				SetVerticesDirty();
			}
		}

		public int numEdge {
			get => _numEdge;
			set {
				value = value < 3 ? 3 : value;
				if (_numEdge == value)
					return;
				_numEdge = value;
				SetVerticesDirty();
			}
		}

		public float startAngle {
			get => _startAngle;
			set {
				value = Mathf.Clamp(value, 0.0f, 360f);
				if (Mathf.Abs(_startAngle - value) < 9.999999747378752E-05)
					return;
				_startAngle = value;
				SetVerticesDirty();
			}
		}

		public RectTransform surround {
			get => _surround;
			set {
				if (!(_surround != value))
					return;
				_surround = value;
				SetVerticesDirty();
			}
		}

		public Vector4 surroundOffset {
			get => _surroundOffset;
			set {
				if (!(_surroundOffset != value))
					return;
				_surroundOffset = value;
				SetVerticesDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper toFill) {
			shapedraw_rect shapedrawRect;
			if (!(bool)(Object)activeSprite)
				toFill.Clear();
			else
				switch (shape) {
					case ImageShape.Rect:
						shapedrawRect = new shapedraw_rect();
						shapedrawRect.Fill(toFill, this);
						break;
					case ImageShape.Round:
						new shapedraw_round().Fill(toFill, this);
						break;
					case ImageShape.RoundRect:
						new shapedraw_ellipse().Fill(toFill, this);
						break;
					case ImageShape.Surround:
						if ((bool)(Object)_surround) {
							new shapedraw_surround().Fill(toFill, this);
							break;
						}

						shapedrawRect = new shapedraw_rect();
						shapedrawRect.Fill(toFill, this);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct shapedraw_ellipse {
			internal void Fill(VertexHelper toFill, ShapeImage image) {
				toFill.Clear();
				var color = image.color;
				var currentVertCount1 = toFill.currentVertCount;
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				var outerUv = image.GetOuterUV();
				var matrix = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
				var ellipse = image._ellipse;
				move_to(toFill,
					new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
					color, matrix);
				line_to(toFill,
					new Vector3(drawingDimensions.z - ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
					color, matrix);
				curve_to(toFill, new Vector3(drawingDimensions.z, drawingDimensions.y),
					new Vector3(drawingDimensions.z, drawingDimensions.y + ellipse.y), drawingDimensions, outerUv,
					color, matrix);
				line_to(toFill,
					new Vector3(drawingDimensions.z, drawingDimensions.w - ellipse.y), drawingDimensions, outerUv,
					color, matrix);
				curve_to(toFill, new Vector3(drawingDimensions.z, drawingDimensions.w),
					new Vector3(drawingDimensions.z - ellipse.x, drawingDimensions.w), drawingDimensions, outerUv,
					color, matrix);
				line_to(toFill,
					new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.w), drawingDimensions, outerUv,
					color, matrix);
				curve_to(toFill, new Vector3(drawingDimensions.x, drawingDimensions.w),
					new Vector3(drawingDimensions.x, drawingDimensions.w - ellipse.y), drawingDimensions, outerUv,
					color, matrix);
				line_to(toFill,
					new Vector3(drawingDimensions.x, drawingDimensions.y + ellipse.y), drawingDimensions, outerUv,
					color, matrix);
				curve_to(toFill, new Vector3(drawingDimensions.x, drawingDimensions.y),
					new Vector3(drawingDimensions.x + ellipse.x, drawingDimensions.y), drawingDimensions, outerUv,
					color, matrix);
				var currentVertCount2 = toFill.currentVertCount;
				var num = currentVertCount2 - currentVertCount1;
				for (var index = currentVertCount1; index <= currentVertCount2; ++index)
					toFill.AddTriangle(currentVertCount1, (index + 2 - currentVertCount1) % num,
						(index + 1 - currentVertCount1) % num);
			}

			private static void move_to(
				VertexHelper vh,
				Vector3 pos,
				Vector4 dimentions,
				Vector4 uvs,
				Color color,
				Matrix4x4 matrix) {
				if (matrix != Matrix4x4.identity)
					pos = matrix.MultiplyPoint(pos);
				var uv = GraphicTools.TransferUV(pos, dimentions, uvs);
				GraphicTools.MoveTo(vh, pos, uv, color);
			}

			private static void line_to(
				VertexHelper vh,
				Vector3 pos,
				Vector4 dimentions,
				Vector4 uvs,
				Color color,
				Matrix4x4 matrix) {
				if (matrix != Matrix4x4.identity)
					pos = matrix.MultiplyPoint(pos);
				var uv = GraphicTools.TransferUV(pos, dimentions, uvs);
				GraphicTools.LineTo(vh, pos, uv, color);
			}

			private static void curve_to(
				VertexHelper vh,
				Vector3 control,
				Vector3 to,
				Vector4 dimentions,
				Vector4 uvs,
				Color color,
				Matrix4x4 matrix) {
				if (matrix != Matrix4x4.identity) {
					control = matrix.MultiplyPoint(control);
					to = matrix.MultiplyPoint(to);
				}

				var uvNext = GraphicTools.TransferUV(to, dimentions, uvs);
				var uvControl = GraphicTools.TransferUV(control, dimentions, uvs);
				GraphicTools.CurveTo(vh, control, to, uvControl, uvNext, color);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct shapedraw_rect {
			internal void Fill(VertexHelper toFill, ShapeImage image) {
				toFill.Clear();
				if (image._fillCenter)
					draw_rect(toFill, image);
				else
					draw_rect(toFill, image, image._thickness);
			}

			private static void draw_rect(VertexHelper toFill, ShapeImage image) {
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				var outerUv = image.GetOuterUV();
				var matrix = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
				add_quad(toFill, drawingDimensions, outerUv, drawingDimensions,
					image.color, matrix);
			}

			private static void draw_rect(VertexHelper toFill, ShapeImage image, float t) {
				if (t <= 9.999999747378752E-05)
					return;
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				if (t >= (drawingDimensions.z - (double)drawingDimensions.x) * 0.5 &&
				    t >= (drawingDimensions.w - (double)drawingDimensions.y) * 0.5) {
					draw_rect(toFill, image);
				}
				else {
					var outerUv = image.GetOuterUV();
					var matrix = image.skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
					var color = image.color;
					add_quad(toFill,
						new Vector4(drawingDimensions.x, drawingDimensions.y, drawingDimensions.x + t,
							drawingDimensions.w), outerUv, drawingDimensions, color, matrix);
					add_quad(toFill,
						new Vector4(drawingDimensions.x + t, drawingDimensions.y, drawingDimensions.z - t,
							drawingDimensions.y + t), outerUv, drawingDimensions, color, matrix);
					add_quad(toFill,
						new Vector4(drawingDimensions.z - t, drawingDimensions.y, drawingDimensions.z,
							drawingDimensions.w), outerUv, drawingDimensions, color, matrix);
					add_quad(toFill,
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
				Matrix4x4 matrix) {
				Color32 color32 = color;
				var verts = VectorMath.verts;
				var vertex = VectorMath.vertex;
				verts[0] = new Vector3(area.x, area.y);
				verts[1] = new Vector3(area.x, area.w);
				verts[2] = new Vector3(area.z, area.w);
				verts[3] = new Vector3(area.z, area.y);
				var flag = matrix == Matrix4x4.identity;
				for (var index = 0; index < 4; ++index) {
					var position = verts[index];
					vertex[index].uv0 = GraphicTools.TransferUV(position, dimensions, uv4);
					vertex[index].position = flag ? verts[index] : matrix.MultiplyPoint(verts[index]);
					vertex[index].color = color32;
				}

				toFill.AddUIVertexQuad(vertex);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct shapedraw_round {
			internal void Fill(VertexHelper toFill, ShapeImage image) {
				toFill.Clear();
				if (image._fillAmount < 1.0 / image.numEdge)
					return;
				if (image._fillCenter)
					draw(toFill, image);
				else
					draw(toFill, image, image._thickness);
			}

			private static void draw(VertexHelper toFill, ShapeImage image) {
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				Color32 color = image.color;
				var num1 = drawingDimensions.z - drawingDimensions.x;
				var num2 = drawingDimensions.w - drawingDimensions.y;
				var num3 = num1 > (double)num2 ? num2 * 0.5f : num1 * 0.5f;
				var point = new Vector2(drawingDimensions.x + num1 * 0.5f, drawingDimensions.y + num2 * 0.5f);
				var outerUv = image.GetOuterUV();
				var matrix4x4 = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
				Vector2 position1 = matrix4x4.MultiplyPoint(point);
				var numEdge = image._numEdge;
				var num4 = 6.2831855f / numEdge;
				if (image._fillAmount < (double)num4)
					return;
				var v = new UIVertex {
					position = position1,
					uv0 = GraphicTools.TransferUV(position1, drawingDimensions, outerUv),
					color = color
				};
				toFill.AddVert(v);
				var num5 = image._startAngle * ((float)Math.PI / 180f);
				var num6 = (float)(image._fillAmount * 3.1415927410125732 * 2.0);
				for (var idx2 = 0; idx2 <= numEdge; ++idx2) {
					var num7 = num4 * idx2;
					var f = image._clockWise ? num5 - num7 : num5 + num7;
					var num8 = Mathf.Cos(f);
					var num9 = Mathf.Sin(f);
					var position2 =
						matrix4x4.MultiplyPoint(new Vector3(position1.x + num8 * num3, position1.y + num9 * num3,
							0.0f));
					var uv0 = GraphicTools.TransferUV(position2, drawingDimensions, outerUv);
					toFill.AddVert(position2, color, uv0);
					if (idx2 >= 1)
						toFill.AddTriangle(0, idx2 - 1, idx2);
					if (num7 > (double)num6)
						break;
				}

				if (image._fillAmount < 1.0)
					return;
				toFill.AddTriangle(0, numEdge, 1);
			}

			private static void draw(VertexHelper toFill, ShapeImage image, float thick) {
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				Color32 color = image.color;
				var matrix4x4 = image._skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
				var num1 = drawingDimensions.z - drawingDimensions.x;
				var num2 = drawingDimensions.w - drawingDimensions.y;
				var num3 = num1 > (double)num2 ? num2 * 0.5f : num1 * 0.5f;
				var num4 = num3 - thick;
				var point = new Vector2(drawingDimensions.x + num1 * 0.5f, drawingDimensions.y + num2 * 0.5f);
				Vector2 vector2_1 = matrix4x4.MultiplyPoint(point);
				var numEdge = image._numEdge;
				var num5 = 6.2831855f / numEdge;
				var outerUv = image.GetOuterUV();
				var vertex = VectorMath.vertex;
				var num6 = (float)(image._fillAmount * 3.1415927410125732 * 2.0);
				var num7 = image._startAngle * ((float)Math.PI / 180f);
				for (var index = 0; index <= numEdge; ++index) {
					var num8 = num5 * index;
					var f = image._clockWise ? num7 - num8 : num7 + num8;
					var num9 = Mathf.Cos(f);
					var num10 = Mathf.Sin(f);
					var position1 = matrix4x4.MultiplyPoint(new Vector3(vector2_1.x + num9 * num3,
						vector2_1.y + num10 * num3, 0.0f));
					var vector2_2 = GraphicTools.TransferUV(position1, drawingDimensions, outerUv);
					var position2 = matrix4x4.MultiplyPoint(new Vector3(vector2_1.x + num9 * num4,
						vector2_1.y + num10 * num4, 0.0f));
					var vector2_3 = GraphicTools.TransferUV(position2, drawingDimensions, outerUv);
					var uiVertexArray1 = vertex;
					var uiVertex1 = new UIVertex();
					uiVertex1.position = position1;
					uiVertex1.uv0 = vector2_2;
					uiVertex1.color = color;
					var uiVertex2 = uiVertex1;
					uiVertexArray1[2] = uiVertex2;
					var uiVertexArray2 = vertex;
					uiVertex1 = new UIVertex();
					uiVertex1.position = position2;
					uiVertex1.uv0 = vector2_3;
					uiVertex1.color = color;
					var uiVertex3 = uiVertex1;
					uiVertexArray2[3] = uiVertex3;
					if (index >= 1)
						toFill.AddUIVertexQuad(vertex);
					vertex[0] = vertex[3];
					vertex[1] = vertex[2];
					if (num8 > (double)num6)
						break;
				}
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		internal struct shapedraw_surround {
			internal void Fill(VertexHelper toFill, ShapeImage image) {
				toFill.Clear();
				draw_rect(toFill, image);
			}

			private static void draw_rect(VertexHelper toFill, ShapeImage image) {
				var rect = image._surround.rect;
				var drawingDimensions = image.GetDrawingDimensions(image._preserveAspect);
				var vector4 = new Vector4(rect.x, rect.y, rect.xMax, rect.yMax);
				var surroundOffset = image._surroundOffset;
				vector4.x += surroundOffset.x;
				vector4.y += surroundOffset.y;
				vector4.z -= surroundOffset.w;
				vector4.w -= surroundOffset.z;
				var outerUv = image.GetOuterUV();
				var matrix = image.skew == Vector2.zero ? Matrix4x4.identity : MatrixUtils.Skew(image._skew);
				var color = image.color;
				add_quad(toFill,
					new Vector4(drawingDimensions.x, drawingDimensions.y, vector4.x, drawingDimensions.w), outerUv,
					drawingDimensions, color, matrix);
				add_quad(toFill,
					new Vector4(vector4.x, drawingDimensions.y, vector4.z, vector4.y), outerUv, drawingDimensions,
					color, matrix);
				add_quad(toFill,
					new Vector4(vector4.z, drawingDimensions.y, drawingDimensions.z, drawingDimensions.w), outerUv,
					drawingDimensions, color, matrix);
				add_quad(toFill,
					new Vector4(vector4.x, vector4.w, vector4.z, drawingDimensions.w), outerUv, drawingDimensions,
					color, matrix);
			}

			private static void add_quad(
				VertexHelper toFill,
				Vector4 area,
				Vector4 uv4,
				Vector4 dimensions,
				Color color,
				Matrix4x4 matrix) {
				if (area.sqrMagnitude < 0.001)
					return;
				Color32 color32 = color;
				var verts = VectorMath.verts;
				var vertex = VectorMath.vertex;
				verts[0] = new Vector3(area.x, area.y);
				verts[1] = new Vector3(area.x, area.w);
				verts[2] = new Vector3(area.z, area.w);
				verts[3] = new Vector3(area.z, area.y);
				var flag = matrix == Matrix4x4.identity;
				for (var index = 0; index < 4; ++index) {
					var position = verts[index];
					vertex[index].uv0 = GraphicTools.TransferUV(position, dimensions, uv4);
					vertex[index].position = flag ? verts[index] : matrix.MultiplyPoint(verts[index]);
					vertex[index].color = color32;
				}

				toFill.AddUIVertexQuad(vertex);
			}
		}
	}
}