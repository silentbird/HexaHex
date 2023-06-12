using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace pure.ui.imageFiller {
	public static class GraphicTools {
		internal static Vector3 startPos;
		internal static Vector2 startUV;
		private static Vector3 prevPos;
		private static Vector3 prevUV;

		public static Vector2 CaculateBezier(float t, Vector2 v0, Vector2 v1, Vector2 control) {
			var num1 = (float)((1.0 - t) * (1.0 - t));
			var num2 = t * t;
			return new Vector2((float)(v0.x * (double)num1 + control.x * (double)t * (1.0 - t) * 2.0 + num2 * (double)v1.x),
				(float)(v0.y * (double)num1 + control.y * (double)t * (1.0 - t) * 2.0 + num2 * (double)v1.y));
		}

		public static Vector4 GetDrawingDimensions(
			Graphic graphic,
			Sprite sprite,
			bool shouldPreserveAspect) {
			var vector4 = (bool)(Object)sprite ? DataUtility.GetPadding(sprite) : Vector4.zero;
			Vector2 vector2_1;
			if (!(bool)(Object)sprite) {
				vector2_1 = Vector2.zero;
			}
			else {
				var rect = sprite.rect;
				double width = rect.width;
				rect = sprite.rect;
				double height = rect.height;
				vector2_1 = new Vector2((float)width, (float)height);
			}

			var vector2_2 = vector2_1;
			var pixelAdjustedRect = graphic.GetPixelAdjustedRect();
			var num1 = Mathf.RoundToInt(vector2_2.x);
			var num2 = Mathf.RoundToInt(vector2_2.y);
			var drawingDimensions = new Vector4(vector4.x / num1, vector4.y / num2, (num1 - vector4.z) / num1,
				(num2 - vector4.w) / num2);
			var rectTransform = graphic.rectTransform;
			if (shouldPreserveAspect && vector2_2.sqrMagnitude > 0.0) {
				var num3 = vector2_2.x / vector2_2.y;
				var num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > (double)num4) {
					var height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * rectTransform.pivot.y;
				}
				else {
					var width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * rectTransform.pivot.x;
				}
			}

			drawingDimensions = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.x,
				pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.y, pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.z,
				pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.w);
			return drawingDimensions;
		}

		public static Vector4 GetMaskDimensions(
			Graphic graphic,
			Sprite sprite,
			Rect mask,
			bool shouldPreserveAspect) {
			var vector4 = (bool)(Object)sprite ? DataUtility.GetPadding(sprite) : Vector4.zero;
			Vector2 vector2_1;
			if (!(bool)(Object)sprite) {
				vector2_1 = Vector2.zero;
			}
			else {
				var rect = sprite.rect;
				double width = rect.width;
				rect = sprite.rect;
				double height = rect.height;
				vector2_1 = new Vector2((float)width, (float)height);
			}

			var vector2_2 = vector2_1;
			vector2_2.x = vector2_2.x > (double)mask.width ? mask.width : vector2_2.x;
			vector2_2.y = vector2_2.y > (double)mask.height ? mask.height : vector2_2.y;
			var pixelAdjustedRect = graphic.GetPixelAdjustedRect();
			var num1 = Mathf.RoundToInt(vector2_2.x);
			var num2 = Mathf.RoundToInt(vector2_2.y);
			var maskDimensions = new Vector4((vector4.x + mask.x) / num1, (vector4.y + mask.y) / num2, (num1 - vector4.z) / num1,
				(num2 - vector4.w) / num2);
			var rectTransform = graphic.rectTransform;
			if (shouldPreserveAspect && vector2_2.sqrMagnitude > 0.0) {
				var num3 = vector2_2.x / vector2_2.y;
				var num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if (num3 > (double)num4) {
					var height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * rectTransform.pivot.y;
				}
				else {
					var width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * rectTransform.pivot.x;
				}
			}

			maskDimensions = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * maskDimensions.x, pixelAdjustedRect.y + pixelAdjustedRect.height * maskDimensions.y,
				pixelAdjustedRect.x + pixelAdjustedRect.width * maskDimensions.z, pixelAdjustedRect.y + pixelAdjustedRect.height * maskDimensions.w);
			return maskDimensions;
		}

		public static void MoveTo(VertexHelper vh, Vector3 pos, Vector2 uv, Color color) {
			prevPos = startPos = pos;
			prevUV = startUV = uv;
			vh.AddVert(pos, color, uv);
		}

		public static void LineTo(VertexHelper vh, Vector3 pos, Vector2 uv, Color color) {
			prevPos = pos;
			prevUV = uv;
			vh.AddVert(pos, color, uv);
		}

		public static void CurveTo(
			VertexHelper vh,
			Vector2 posControl,
			Vector3 posNext,
			Vector2 uvControl,
			Vector2 uvNext,
			Color color,
			int segment = 10) {
			var num = 1f / segment;
			var t = 0.0f;
			for (var index = 0; index <= segment; ++index) {
				Vector3 position = CaculateBezier(t, prevPos, posNext, posControl);
				var uv0 = CaculateBezier(t, prevUV, uvNext, uvControl);
				vh.AddVert(position, color, uv0);
				t += num;
			}

			prevPos = posNext;
			prevUV = uvNext;
		}

		public static Vector2 TransferUV(Vector3 position, Rect posR, Rect uvR) {
			double x = uvR.x;
			double y = uvR.y;
			var num1 = (position.x - (double)posR.x) / posR.width;
			var num2 = (position.y - (double)posR.y) / posR.height;
			return new Vector2((float)(x + num1 * uvR.width), (float)(y + num2 * uvR.height));
		}

		public static Vector2 TransferUV(Vector2 position, Vector4 dimentions, Vector4 uv) {
			var num1 = (float)((position.x - (double)dimentions.x) / (dimentions.z - (double)dimentions.x));
			var num2 = (float)((position.y - (double)dimentions.y) / (dimentions.w - (double)dimentions.y));
			return new Vector2(uv.x + (uv.z - uv.x) * num1, uv.y + (uv.w - uv.y) * num2);
		}
	}
}