using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Sprites;
using UnityEngine.UI;

[AddComponentMenu("UI/PImage", 35), ExecuteInEditMode]
public class PImage : Image
{
    private static readonly Vector2[] vertexScratch = new Vector2[4];
    private static readonly Vector2[] uvScratch = new Vector2[4];
    public bool hole;
    public bool useSlice;
    public bool forceRebuild;

    private Sprite activeSprite => !(bool) (Object) this.overrideSprite ? this.sprite : this.overrideSprite;

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (Application.isPlaying && !(bool) (Object) this.sprite && !(bool) (Object) this.m_Material &&
            !this.forceRebuild)
            toFill.Clear();
        else if (!this.useSlice || !this.hasBorder || this.fillMethod != Image.FillMethod.Horizontal)
        {
            base.OnPopulateMesh(toFill);
        }
        else
        {
            toFill.Clear();
            Vector4 vector4_1;
            Vector4 vector4_2;
            Vector4 vector4_3;
            Vector4 vector4_4;
            if ((bool) (Object) this.activeSprite)
            {
                vector4_1 = DataUtility.GetOuterUV(this.activeSprite);
                vector4_2 = DataUtility.GetInnerUV(this.activeSprite);
                vector4_3 = DataUtility.GetPadding(this.activeSprite);
                vector4_4 = this.activeSprite.border;
            }
            else
            {
                vector4_1 = Vector4.zero;
                vector4_2 = Vector4.zero;
                vector4_3 = Vector4.zero;
                vector4_4 = Vector4.zero;
            }

            Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
            Vector4 adjustedBorders = this.get_adjusted_borders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
            Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;
            PImage.vertexScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
            PImage.vertexScratch[3] = new Vector2(pixelAdjustedRect.width - vector4_5.z,
                pixelAdjustedRect.height - vector4_5.w);
            PImage.vertexScratch[1].x = adjustedBorders.x;
            PImage.vertexScratch[1].y = adjustedBorders.y;
            PImage.vertexScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
            PImage.vertexScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
            for (int index = 0; index < 4; ++index)
            {
                PImage.vertexScratch[index].x += pixelAdjustedRect.x;
                PImage.vertexScratch[index].y += pixelAdjustedRect.y;
            }

            PImage.uvScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
            PImage.uvScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
            PImage.uvScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
            PImage.uvScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
            toFill.Clear();
            Rect other = pixelAdjustedRect;
            other.width *= this.fillAmount;
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    int index4 = index3 + 1;
                    Vector2 vector2 = new Vector2(PImage.vertexScratch[index1].x,
                        PImage.vertexScratch[index3].y);
                    Vector2 posMax = new Vector2(PImage.vertexScratch[index2].x,
                        PImage.vertexScratch[index4].y);
                    Rect rect = new Rect(vector2, posMax - vector2);
                    if (rect.Overlaps(other))
                    {
                        float num = rect.xMax - other.xMax;
                        Vector2 uvMin = new Vector2(PImage.uvScratch[index1].x, PImage.uvScratch[index3].y);
                        Vector2 uvMax = new Vector2(PImage.uvScratch[index2].x, PImage.uvScratch[index4].y);
                        if ((double) num > 0.0)
                        {
                            posMax.x -= num;
                            float t = (other.xMax - rect.x) / rect.width;
                            uvMax.x = Mathf.Lerp(uvMin.x, uvMax.x, t);
                        }

                        PImage.add_quad(toFill, vector2, posMax, (Color32) this.color, uvMin, uvMax);
                    }
                }
            }
        }
    }

    private Vector4 get_adjusted_borders(Vector4 border, Rect adjustedRect)
    {
        Rect rect = this.rectTransform.rect;
        for (int index = 0; index <= 1; ++index)
        {
            Vector2 size = rect.size;
            if (!size[index].Equals(0.0f))
            {
                size = adjustedRect.size;
                double num1 = (double) size[index];
                size = rect.size;
                double num2 = (double) size[index];
                float num3 = (float) (num1 / num2);
                border[index] *= num3;
                border[index + 2] *= num3;
            }

            float num4 = border[index] + border[index + 2];
            size = adjustedRect.size;
            if ((double) size[index] < (double) num4 && !num4.Equals(0.0f))
            {
                size = adjustedRect.size;
                float num5 = size[index] / num4;
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
        Vector2 uvMax)
    {
        int currentVertCount = vertexHelper.currentVertCount;
        vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
        vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
        vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    public override Material GetModifiedMaterial(Material baseMaterial)
    {
        Material baseMat = baseMaterial;
        if (this.m_ShouldRecalculateStencil)
        {
            this.m_StencilValue = !this.maskable
                ? 0
                : MaskUtilities.GetStencilDepth(this.transform,
                    MaskUtilities.FindRootSortOverrideCanvas(this.transform));
            this.m_ShouldRecalculateStencil = false;
        }

        Mask component = this.GetComponent<Mask>();
        if (this.m_StencilValue > 0 && (!(bool) (Object) component || !component.IsActive()))
        {
            Material material = StencilMaterial.Add(baseMat, (1 << this.m_StencilValue) - 1, StencilOp.Keep,
                this.hole ? CompareFunction.NotEqual : CompareFunction.Equal, ColorWriteMask.All,
                (1 << this.m_StencilValue) - 1, 0);
            StencilMaterial.Remove(this.m_MaskMaterial);
            this.m_MaskMaterial = material;
            baseMat = this.m_MaskMaterial;
        }

        return baseMat;
    }
}