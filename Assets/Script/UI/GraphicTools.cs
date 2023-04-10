// Decompiled with JetBrains decompiler
// Type: pure.ui.imageFiller.GraphicTools
// Assembly: pure, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3647E74B-3271-4E18-8589-B6FF96DC8026
// Assembly location: D:\W\UnityProj\mhgd\proj_mhgd\Assets\Plugins\core\pure\Win\pure.dll

using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace pure.ui.imageFiller
{
  public static class GraphicTools
  {
    internal static Vector3 startPos;
    internal static Vector2 startUV;
    private static Vector3 prevPos;
    private static Vector3 prevUV;

    public static Vector2 CaculateBezier(float t, Vector2 v0, Vector2 v1, Vector2 control)
    {
      float num1 = (float) ((1.0 - (double) t) * (1.0 - (double) t));
      float num2 = t * t;
      return new Vector2((float) ((double) v0.x * (double) num1 + (double) control.x * (double) t * (1.0 - (double) t) * 2.0 + (double) num2 * (double) v1.x), (float) ((double) v0.y * (double) num1 + (double) control.y * (double) t * (1.0 - (double) t) * 2.0 + (double) num2 * (double) v1.y));
    }

    public static Vector4 GetDrawingDimensions(
      Graphic graphic,
      Sprite sprite,
      bool shouldPreserveAspect)
    {
      Vector4 vector4 = (bool) (Object) sprite ? DataUtility.GetPadding(sprite) : Vector4.zero;
      Vector2 vector2_1;
      if (!(bool) (Object) sprite)
      {
        vector2_1 = Vector2.zero;
      }
      else
      {
        Rect rect = sprite.rect;
        double width = (double) rect.width;
        rect = sprite.rect;
        double height = (double) rect.height;
        vector2_1 = new Vector2((float) width, (float) height);
      }
      Vector2 vector2_2 = vector2_1;
      Rect pixelAdjustedRect = graphic.GetPixelAdjustedRect();
      int num1 = Mathf.RoundToInt(vector2_2.x);
      int num2 = Mathf.RoundToInt(vector2_2.y);
      Vector4 drawingDimensions = new Vector4(vector4.x / (float) num1, vector4.y / (float) num2, ((float) num1 - vector4.z) / (float) num1, ((float) num2 - vector4.w) / (float) num2);
      RectTransform rectTransform = graphic.rectTransform;
      if (shouldPreserveAspect && (double) vector2_2.sqrMagnitude > 0.0)
      {
        float num3 = vector2_2.x / vector2_2.y;
        float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
        if ((double) num3 > (double) num4)
        {
          float height = pixelAdjustedRect.height;
          pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
          pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * rectTransform.pivot.y;
        }
        else
        {
          float width = pixelAdjustedRect.width;
          pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
          pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * rectTransform.pivot.x;
        }
      }
      drawingDimensions = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.x, pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.y, pixelAdjustedRect.x + pixelAdjustedRect.width * drawingDimensions.z, pixelAdjustedRect.y + pixelAdjustedRect.height * drawingDimensions.w);
      return drawingDimensions;
    }

    public static Vector4 GetMaskDimensions(
      Graphic graphic,
      Sprite sprite,
      Rect mask,
      bool shouldPreserveAspect)
    {
      Vector4 vector4 = (bool) (Object) sprite ? DataUtility.GetPadding(sprite) : Vector4.zero;
      Vector2 vector2_1;
      if (!(bool) (Object) sprite)
      {
        vector2_1 = Vector2.zero;
      }
      else
      {
        Rect rect = sprite.rect;
        double width = (double) rect.width;
        rect = sprite.rect;
        double height = (double) rect.height;
        vector2_1 = new Vector2((float) width, (float) height);
      }
      Vector2 vector2_2 = vector2_1;
      vector2_2.x = (double) vector2_2.x > (double) mask.width ? mask.width : vector2_2.x;
      vector2_2.y = (double) vector2_2.y > (double) mask.height ? mask.height : vector2_2.y;
      Rect pixelAdjustedRect = graphic.GetPixelAdjustedRect();
      int num1 = Mathf.RoundToInt(vector2_2.x);
      int num2 = Mathf.RoundToInt(vector2_2.y);
      Vector4 maskDimensions = new Vector4((vector4.x + mask.x) / (float) num1, (vector4.y + mask.y) / (float) num2, ((float) num1 - vector4.z) / (float) num1, ((float) num2 - vector4.w) / (float) num2);
      RectTransform rectTransform = graphic.rectTransform;
      if (shouldPreserveAspect && (double) vector2_2.sqrMagnitude > 0.0)
      {
        float num3 = vector2_2.x / vector2_2.y;
        float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
        if ((double) num3 > (double) num4)
        {
          float height = pixelAdjustedRect.height;
          pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num3);
          pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * rectTransform.pivot.y;
        }
        else
        {
          float width = pixelAdjustedRect.width;
          pixelAdjustedRect.width = pixelAdjustedRect.height * num3;
          pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * rectTransform.pivot.x;
        }
      }
      maskDimensions = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * maskDimensions.x, pixelAdjustedRect.y + pixelAdjustedRect.height * maskDimensions.y, pixelAdjustedRect.x + pixelAdjustedRect.width * maskDimensions.z, pixelAdjustedRect.y + pixelAdjustedRect.height * maskDimensions.w);
      return maskDimensions;
    }

    public static void MoveTo(VertexHelper vh, Vector3 pos, Vector2 uv, Color color)
    {
      GraphicTools.prevPos = GraphicTools.startPos = pos;
      GraphicTools.prevUV = (Vector3) (GraphicTools.startUV = uv);
      vh.AddVert(pos, (Color32) color, uv);
    }

    public static void LineTo(VertexHelper vh, Vector3 pos, Vector2 uv, Color color)
    {
      GraphicTools.prevPos = pos;
      GraphicTools.prevUV = (Vector3) uv;
      vh.AddVert(pos, (Color32) color, uv);
    }

    public static void CurveTo(
      VertexHelper vh,
      Vector2 posControl,
      Vector3 posNext,
      Vector2 uvControl,
      Vector2 uvNext,
      Color color,
      int segment = 10)
    {
      float num = 1f / (float) segment;
      float t = 0.0f;
      for (int index = 0; index <= segment; ++index)
      {
        Vector3 position = (Vector3) GraphicTools.CaculateBezier(t, (Vector2) GraphicTools.prevPos, (Vector2) posNext, posControl);
        Vector2 uv0 = GraphicTools.CaculateBezier(t, (Vector2) GraphicTools.prevUV, uvNext, uvControl);
        vh.AddVert(position, (Color32) color, uv0);
        t += num;
      }
      GraphicTools.prevPos = posNext;
      GraphicTools.prevUV = (Vector3) uvNext;
    }

    public static Vector2 TransferUV(Vector3 position, Rect posR, Rect uvR)
    {
      double x = (double) uvR.x;
      double y = (double) uvR.y;
      double num1 = ((double) position.x - (double) posR.x) / (double) posR.width;
      double num2 = ((double) position.y - (double) posR.y) / (double) posR.height;
      return new Vector2((float) (x + num1 * (double) uvR.width), (float) (y + num2 * (double) uvR.height));
    }

    public static Vector2 TransferUV(Vector2 position, Vector4 dimentions, Vector4 uv)
    {
      float num1 = (float) (((double) position.x - (double) dimentions.x) / ((double) dimentions.z - (double) dimentions.x));
      float num2 = (float) (((double) position.y - (double) dimentions.y) / ((double) dimentions.w - (double) dimentions.y));
      return new Vector2(uv.x + (uv.z - uv.x) * num1, uv.y + (uv.w - uv.y) * num2);
    }
  }
}
