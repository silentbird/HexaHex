using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Sprites;
using UnityEngine.UI;

[AddComponentMenu("UI/PImage", 35), ExecuteInEditMode]
public class PImage : Image {
	private static readonly Vector2[] vertexScratch = new Vector2[4];
	private static readonly Vector2[] uvScratch = new Vector2[4];
	public bool hole;
	public bool useSlice;
	public bool forceRebuild;

	private Sprite activeSprite => !(bool)(Object)overrideSprite ? sprite : overrideSprite;

	protected override void OnPopulateMesh(VertexHelper toFill) {
		if (Application.isPlaying && !(bool)(Object)sprite && !(bool)(Object)m_Material &&
		    !forceRebuild) {
			toFill.Clear();
		}
		else if (!useSlice || !hasBorder || fillMethod != FillMethod.Horizontal) {
			base.OnPopulateMesh(toFill);
		}
		else {
			toFill.Clear();
			Vector4 vector4_1;
			Vector4 vector4_2;
			Vector4 vector4_3;
			Vector4 vector4_4;
			if ((bool)(Object)activeSprite) {
				vector4_1 = DataUtility.GetOuterUV(activeSprite);
				vector4_2 = DataUtility.GetInnerUV(activeSprite);
				vector4_3 = DataUtility.GetPadding(activeSprite);
				vector4_4 = activeSprite.border;
			}
			else {
				vector4_1 = Vector4.zero;
				vector4_2 = Vector4.zero;
				vector4_3 = Vector4.zero;
				vector4_4 = Vector4.zero;
			}

			var pixelAdjustedRect = GetPixelAdjustedRect();
			var adjustedBorders = get_adjusted_borders(vector4_4 / pixelsPerUnit, pixelAdjustedRect);
			var vector4_5 = vector4_3 / pixelsPerUnit;
			vertexScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
			vertexScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z,
				pixelAdjustedRect.height - vector4_5.w);
			vertexScratch[1].x = adjustedBorders.x;
			vertexScratch[1].y = adjustedBorders.y;
			vertexScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
			vertexScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
			for (var index = 0; index < 4; ++index) {
				vertexScratch[index].x += pixelAdjustedRect.x;
				vertexScratch[index].y += pixelAdjustedRect.y;
			}

			uvScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
			uvScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
			uvScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
			uvScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
			toFill.Clear();
			var other = pixelAdjustedRect;
			other.width *= fillAmount;
			for (var index1 = 0; index1 < 3; ++index1) {
				var index2 = index1 + 1;
				for (var index3 = 0; index3 < 3; ++index3) {
					var index4 = index3 + 1;
					var vector2 = new Vector2(vertexScratch[index1].x,
						vertexScratch[index3].y);
					var posMax = new Vector2(vertexScratch[index2].x,
						vertexScratch[index4].y);
					var rect = new Rect(vector2, posMax - vector2);
					if (rect.Overlaps(other)) {
						var num = rect.xMax - other.xMax;
						var uvMin = new Vector2(uvScratch[index1].x, uvScratch[index3].y);
						var uvMax = new Vector2(uvScratch[index2].x, uvScratch[index4].y);
						if (num > 0.0) {
							posMax.x -= num;
							var t = (other.xMax - rect.x) / rect.width;
							uvMax.x = Mathf.Lerp(uvMin.x, uvMax.x, t);
						}

						add_quad(toFill, vector2, posMax, color, uvMin, uvMax);
					}
				}
			}
		}
	}

	private Vector4 get_adjusted_borders(Vector4 border, Rect adjustedRect) {
		var rect = rectTransform.rect;
		for (var index = 0; index <= 1; ++index) {
			var size = rect.size;
			if (!size[index].Equals(0.0f)) {
				size = adjustedRect.size;
				double num1 = size[index];
				size = rect.size;
				double num2 = size[index];
				var num3 = (float)(num1 / num2);
				border[index] *= num3;
				border[index + 2] *= num3;
			}

			var num4 = border[index] + border[index + 2];
			size = adjustedRect.size;
			if (size[index] < (double)num4 && !num4.Equals(0.0f)) {
				size = adjustedRect.size;
				var num5 = size[index] / num4;
				border[index] *= num5;
				border[index + 2] *= num5;
			}
		}

		return border;
	}

	private static void add_quad(
		VertexHelper vertexHelper,
		Vector2 posMin,
		Vector2 posMax,
		Color32 color,
		Vector2 uvMin,
		Vector2 uvMax) {
		var currentVertCount = vertexHelper.currentVertCount;
		vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
		vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
		vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
		vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
		vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
		vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
	}

	public override Material GetModifiedMaterial(Material baseMaterial) {
		var baseMat = baseMaterial;
		if (m_ShouldRecalculateStencil) {
			m_StencilValue = !maskable
				? 0
				: MaskUtilities.GetStencilDepth(transform,
					MaskUtilities.FindRootSortOverrideCanvas(transform));
			m_ShouldRecalculateStencil = false;
		}

		var component = GetComponent<Mask>();
		if (m_StencilValue > 0 && (!(bool)(Object)component || !component.IsActive())) {
			var material = StencilMaterial.Add(baseMat, (1 << m_StencilValue) - 1, StencilOp.Keep,
				hole ? CompareFunction.NotEqual : CompareFunction.Equal, ColorWriteMask.All,
				(1 << m_StencilValue) - 1, 0);
			StencilMaterial.Remove(m_MaskMaterial);
			m_MaskMaterial = material;
			baseMat = m_MaskMaterial;
		}

		return baseMat;
	}
}