using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.Sprites;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace pure.ui.image {
	public abstract class ImageCore : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter {
		private static Material s_default_etc1;

		[SerializeField]
		private Sprite m_Sprite;

		[SerializeField]
		private Sprite m_OverrideSprite;

		[SerializeField, FormerlySerializedAs("hole")]
		private bool m_Hole;

		public float pixelsPerUnit {
			get {
				var num1 = 100f;
				if ((bool)(Object)activeSprite)
					num1 = activeSprite.pixelsPerUnit;
				var num2 = 100f;
				if ((bool)(Object)canvas)
					num2 = canvas.referencePixelsPerUnit;
				return num1 / num2;
			}
		}

		public Sprite sprite {
			get => m_Sprite;
			set {
				if (m_Sprite == value)
					return;
				m_Sprite = value;
				SetAllDirty();
			}
		}

		public Sprite overrideSprite {
			get => activeSprite;
			set {
				if (m_OverrideSprite == value)
					return;
				m_OverrideSprite = value;
				SetAllDirty();
			}
		}

		public bool hole {
			get => m_Hole;
			set {
				if (m_Hole == value)
					return;
				m_Hole = value;
				SetMaterialDirty();
			}
		}

		public static Material defaultETC1GraphicMaterial {
			get {
				if (!(bool)(Object)s_default_etc1)
					s_default_etc1 = Canvas.GetETC1SupportedCanvasMaterial();
				return s_default_etc1;
			}
		}

		public override Texture mainTexture {
			get {
				if ((bool)(Object)activeSprite)
					return activeSprite.texture;
				return (bool)(Object)material && (bool)(Object)material.mainTexture ? material.mainTexture : s_WhiteTexture;
			}
		}

		public override Material material {
			get {
				if ((bool)(Object)m_Material)
					return m_Material;
				return (bool)(Object)activeSprite && (bool)(Object)activeSprite.associatedAlphaSplitTexture
					? defaultETC1GraphicMaterial
					: defaultMaterial;
			}
			set => base.material = value;
		}

		protected virtual Sprite activeSprite => !(bool)(Object)m_OverrideSprite ? m_Sprite : m_OverrideSprite;

		public bool hasBorder => GetSpriteBorder().sqrMagnitude > 0.0;

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) {
			return !(bool)(Object)activeSprite ||
			       RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
				       screenPoint, eventCamera, out var _);
		}

		public virtual void CalculateLayoutInputHorizontal() {
		}

		public virtual void CalculateLayoutInputVertical() {
		}

		public virtual float minWidth => 0.0f;

		public virtual float preferredWidth => !(bool)(Object)activeSprite ? 0.0f : GetSpriteSize().x / pixelsPerUnit;

		public virtual float flexibleWidth => -1f;

		public virtual float minHeight => 0.0f;

		public virtual float preferredHeight => !(bool)(Object)activeSprite ? 0.0f : GetSpriteSize().y / pixelsPerUnit;

		public virtual float flexibleHeight => -1f;

		public virtual int layoutPriority => 0;

		protected static void AddQuad(
			VertexHelper toFill,
			Vector3[] quadPositions,
			Color32 color,
			Vector3[] quadUVs) {
			var currentVertCount = toFill.currentVertCount;
			for (var index = 0; index < 4; ++index)
				toFill.AddVert(quadPositions[index], color, (Vector2)quadUVs[index]);
			toFill.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			toFill.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		protected static void AddQuad(
			VertexHelper toFill,
			Vector2 posMin,
			Vector2 posMax,
			Color32 color,
			Vector2 uvMin,
			Vector2 uvMax) {
			var currentVertCount = toFill.currentVertCount;
			toFill.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
			toFill.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
			toFill.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
			toFill.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
			toFill.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
			toFill.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
		}

		protected static Vector4 GetAdjustedBorders(
			Vector4 border,
			Rect adjustedRect,
			Rect transformRect) {
			for (var index = 0; index <= 1; ++index) {
				if (Math.Abs(transformRect.size[index]) > 9.999999747378752E-06) {
					var num = adjustedRect.size[index] / transformRect.size[index];
					border[index] *= num;
					border[index + 2] *= num;
				}

				var num1 = border[index] + border[index + 2];
				if (adjustedRect.size[index] < (double)num1 && Math.Abs(num1) > 9.999999747378752E-06) {
					var num2 = adjustedRect.size[index] / num1;
					border[index] *= num2;
					border[index + 2] *= num2;
				}
			}

			return border;
		}

		protected virtual Vector4 GetSpritePadding() {
			return !(bool)(Object)activeSprite ? Vector4.zero : DataUtility.GetPadding(activeSprite);
		}

		protected virtual Vector4 GetSpriteBorder() {
			return !(bool)(Object)activeSprite ? Vector4.zero : activeSprite.border;
		}

		protected virtual Vector2 GetSpriteSize() {
			return !(bool)(Object)activeSprite ? Vector2.zero : activeSprite.rect.size;
		}

		protected virtual Vector4 GetOuterUV() {
			return !(bool)(Object)activeSprite ? Vector4.zero : DataUtility.GetOuterUV(activeSprite);
		}

		protected virtual Vector4 GetInnterUV() {
			return !(bool)(Object)activeSprite ? Vector4.zero : DataUtility.GetInnerUV(activeSprite);
		}

		protected virtual Vector2 GetSpriteMinSize() {
			return !(bool)(Object)activeSprite ? Vector2.zero : DataUtility.GetMinSize(activeSprite);
		}

		protected virtual Texture2D GetAlphaSplitTexuture() {
			return !(bool)(Object)activeSprite ? null : activeSprite.associatedAlphaSplitTexture;
		}

		protected override void UpdateMaterial() {
			base.UpdateMaterial();
			if (!(bool)(Object)activeSprite) {
				canvasRenderer.SetAlphaTexture(null);
			}
			else {
				var alphaSplitTexuture = GetAlphaSplitTexuture();
				if (!(bool)(Object)alphaSplitTexuture)
					return;
				canvasRenderer.SetAlphaTexture(alphaSplitTexuture);
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial) {
			return !hole ? base.GetModifiedMaterial(baseMaterial) : make_hole(baseMaterial);
		}

		private Material make_hole(Material baseMaterial) {
			var baseMat = baseMaterial;
			if (m_ShouldRecalculateStencil) {
				var sortOverrideCanvas = MaskUtilities.FindRootSortOverrideCanvas(transform);
				m_StencilValue = maskable ? MaskUtilities.GetStencilDepth(transform, sortOverrideCanvas) : 0;
				m_ShouldRecalculateStencil = false;
			}

			if (m_StencilValue == 0)
				return baseMat;
			var component = GetComponent<Mask>();
			if ((bool)(Object)component && component.IsActive())
				return baseMat;
			var material = StencilMaterial.Add(baseMat, (1 << m_StencilValue) - 1, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All,
				(1 << m_StencilValue) - 1, 0);
			StencilMaterial.Remove(m_MaskMaterial);
			m_MaskMaterial = material;
			return m_MaskMaterial;
		}

		public override void SetNativeSize() {
			if (!(activeSprite != null))
				return;
			var rect = activeSprite.rect;
			var x = rect.width / pixelsPerUnit;
			rect = activeSprite.rect;
			var y = rect.height / pixelsPerUnit;
			rectTransform.anchorMax = rectTransform.anchorMin;
			rectTransform.sizeDelta = new Vector2(x, y);
			SetAllDirty();
		}

		protected Vector4 GetDrawingDimensions(bool lPreservAspect) {
			var spritePadding = GetSpritePadding();
			var spriteSize = GetSpriteSize();
			var pixelAdjustedRect = GetPixelAdjustedRect();
			var num1 = Mathf.RoundToInt(spriteSize.x);
			var num2 = Mathf.RoundToInt(spriteSize.y);
			var vector4 = new Vector4(spritePadding.x / num1, spritePadding.y / num2, (num1 - spritePadding.z) / num1,
				(num2 - spritePadding.w) / num2);
			if (lPreservAspect && spriteSize.sqrMagnitude > 0.0) {
				var num3 = spriteSize.x / spriteSize.y;
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

			return new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector4.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4.y,
				pixelAdjustedRect.x + pixelAdjustedRect.width * vector4.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4.w);
		}
	}
}