using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace pure.ui.image {
	public abstract class ImageCore : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter {
		[SerializeField]
		private Sprite m_Sprite;

		[SerializeField]
		private Sprite m_OverrideSprite;

		[SerializeField]
		[FormerlySerializedAs("hole")]
		private bool m_Hole;

		private static Material s_default_etc1;

		public float pixelsPerUnit {
			get {
				float num1 = 100f;
				if ((bool)(UnityEngine.Object)this.activeSprite)
					num1 = this.activeSprite.pixelsPerUnit;
				float num2 = 100f;
				if ((bool)(UnityEngine.Object)this.canvas)
					num2 = this.canvas.referencePixelsPerUnit;
				return num1 / num2;
			}
		}

		protected static void AddQuad(
			VertexHelper toFill,
			Vector3[] quadPositions,
			Color32 color,
			Vector3[] quadUVs) {
			int currentVertCount = toFill.currentVertCount;
			for (int index = 0; index < 4; ++index)
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
			int currentVertCount = toFill.currentVertCount;
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
			for (int index = 0; index <= 1; ++index) {
				if ((double)Math.Abs(transformRect.size[index]) > 9.999999747378752E-06) {
					float num = adjustedRect.size[index] / transformRect.size[index];
					border[index] *= num;
					border[index + 2] *= num;
				}

				float num1 = border[index] + border[index + 2];
				if ((double)adjustedRect.size[index] < (double)num1 && (double)Math.Abs(num1) > 9.999999747378752E-06) {
					float num2 = adjustedRect.size[index] / num1;
					border[index] *= num2;
					border[index + 2] *= num2;
				}
			}

			return border;
		}

		public Sprite sprite {
			get => this.m_Sprite;
			set {
				if ((UnityEngine.Object)this.m_Sprite == (UnityEngine.Object)value)
					return;
				this.m_Sprite = value;
				this.SetAllDirty();
			}
		}

		public Sprite overrideSprite {
			get => this.activeSprite;
			set {
				if ((UnityEngine.Object)this.m_OverrideSprite == (UnityEngine.Object)value)
					return;
				this.m_OverrideSprite = value;
				this.SetAllDirty();
			}
		}

		public bool hole {
			get => this.m_Hole;
			set {
				if (this.m_Hole == value)
					return;
				this.m_Hole = value;
				this.SetMaterialDirty();
			}
		}

		public static Material defaultETC1GraphicMaterial {
			get {
				if (!(bool)(UnityEngine.Object)ImageCore.s_default_etc1)
					ImageCore.s_default_etc1 = Canvas.GetETC1SupportedCanvasMaterial();
				return ImageCore.s_default_etc1;
			}
		}

		public override Texture mainTexture {
			get {
				if ((bool)(UnityEngine.Object)this.activeSprite)
					return (Texture)this.activeSprite.texture;
				return (bool)(UnityEngine.Object)this.material && (bool)(UnityEngine.Object)this.material.mainTexture ? this.material.mainTexture : (Texture)Graphic.s_WhiteTexture;
			}
		}

		public override Material material {
			get {
				if ((bool)(UnityEngine.Object)this.m_Material)
					return this.m_Material;
				return (bool)(UnityEngine.Object)this.activeSprite && (bool)(UnityEngine.Object)this.activeSprite.associatedAlphaSplitTexture
					? ImageCore.defaultETC1GraphicMaterial
					: this.defaultMaterial;
			}
			set => base.material = value;
		}

		protected virtual Sprite activeSprite => !(bool)(UnityEngine.Object)this.m_OverrideSprite ? this.m_Sprite : this.m_OverrideSprite;

		public bool hasBorder => (double)this.GetSpriteBorder().sqrMagnitude > 0.0;

		protected virtual Vector4 GetSpritePadding() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector4.zero : DataUtility.GetPadding(this.activeSprite);

		protected virtual Vector4 GetSpriteBorder() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector4.zero : this.activeSprite.border;

		protected virtual Vector2 GetSpriteSize() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector2.zero : this.activeSprite.rect.size;

		protected virtual Vector4 GetOuterUV() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector4.zero : DataUtility.GetOuterUV(this.activeSprite);

		protected virtual Vector4 GetInnterUV() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector4.zero : DataUtility.GetInnerUV(this.activeSprite);

		protected virtual Vector2 GetSpriteMinSize() => !(bool)(UnityEngine.Object)this.activeSprite ? Vector2.zero : DataUtility.GetMinSize(this.activeSprite);

		protected virtual Texture2D GetAlphaSplitTexuture() => !(bool)(UnityEngine.Object)this.activeSprite ? (Texture2D)null : this.activeSprite.associatedAlphaSplitTexture;

		protected override void UpdateMaterial() {
			base.UpdateMaterial();
			if (!(bool)(UnityEngine.Object)this.activeSprite) {
				this.canvasRenderer.SetAlphaTexture((Texture)null);
			}
			else {
				Texture2D alphaSplitTexuture = this.GetAlphaSplitTexuture();
				if (!(bool)(UnityEngine.Object)alphaSplitTexuture)
					return;
				this.canvasRenderer.SetAlphaTexture((Texture)alphaSplitTexuture);
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial) => !this.hole ? base.GetModifiedMaterial(baseMaterial) : this.make_hole(baseMaterial);

		private Material make_hole(Material baseMaterial) {
			Material baseMat = baseMaterial;
			if (this.m_ShouldRecalculateStencil) {
				Transform sortOverrideCanvas = MaskUtilities.FindRootSortOverrideCanvas(this.transform);
				this.m_StencilValue = this.maskable ? MaskUtilities.GetStencilDepth(this.transform, sortOverrideCanvas) : 0;
				this.m_ShouldRecalculateStencil = false;
			}

			if (this.m_StencilValue == 0)
				return baseMat;
			Mask component = this.GetComponent<Mask>();
			if ((bool)(UnityEngine.Object)component && component.IsActive())
				return baseMat;
			Material material = StencilMaterial.Add(baseMat, (1 << this.m_StencilValue) - 1, StencilOp.Keep, CompareFunction.NotEqual, ColorWriteMask.All,
				(1 << this.m_StencilValue) - 1, 0);
			StencilMaterial.Remove(this.m_MaskMaterial);
			this.m_MaskMaterial = material;
			return this.m_MaskMaterial;
		}

		public override void SetNativeSize() {
			if (!((UnityEngine.Object)this.activeSprite != (UnityEngine.Object)null))
				return;
			Rect rect = this.activeSprite.rect;
			float x = rect.width / this.pixelsPerUnit;
			rect = this.activeSprite.rect;
			float y = rect.height / this.pixelsPerUnit;
			this.rectTransform.anchorMax = this.rectTransform.anchorMin;
			this.rectTransform.sizeDelta = new Vector2(x, y);
			this.SetAllDirty();
		}

		protected Vector4 GetDrawingDimensions(bool lPreservAspect) {
			Vector4 spritePadding = this.GetSpritePadding();
			Vector2 spriteSize = this.GetSpriteSize();
			Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
			int num1 = Mathf.RoundToInt(spriteSize.x);
			int num2 = Mathf.RoundToInt(spriteSize.y);
			Vector4 vector4 = new Vector4(spritePadding.x / (float)num1, spritePadding.y / (float)num2, ((float)num1 - spritePadding.z) / (float)num1,
				((float)num2 - spritePadding.w) / (float)num2);
			if (lPreservAspect && (double)spriteSize.sqrMagnitude > 0.0) {
				float num3 = spriteSize.x / spriteSize.y;
				float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
				if ((double)num3 > (double)num4) {
					float height = pixelAdjustedRect.height;
					pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
					pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * this.rectTransform.pivot.y;
				}
				else {
					float width = pixelAdjustedRect.width;
					pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
					pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * this.rectTransform.pivot.x;
				}
			}

			return new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector4.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4.y,
				pixelAdjustedRect.x + pixelAdjustedRect.width * vector4.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector4.w);
		}

		public virtual void CalculateLayoutInputHorizontal() { }

		public virtual void CalculateLayoutInputVertical() { }

		public virtual float minWidth => 0.0f;

		public virtual float preferredWidth => !(bool)(UnityEngine.Object)this.activeSprite ? 0.0f : this.GetSpriteSize().x / this.pixelsPerUnit;

		public virtual float flexibleWidth => -1f;

		public virtual float minHeight => 0.0f;

		public virtual float preferredHeight => !(bool)(UnityEngine.Object)this.activeSprite ? 0.0f : this.GetSpriteSize().y / this.pixelsPerUnit;

		public virtual float flexibleHeight => -1f;

		public virtual int layoutPriority => 0;

		public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera) => !(bool)(UnityEngine.Object)this.activeSprite ||
		                                                                                       RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform,
			                                                                                       screenPoint, eventCamera, out Vector2 _);
	}
}