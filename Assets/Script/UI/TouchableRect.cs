using UnityEngine;
using UnityEngine.UI;

namespace pure.ui.element
{
    [AddComponentMenu("UI/TouchableRect", 34), RequireComponent(typeof(RectTransform))]
    public class TouchableRect : Graphic, ICanvasRaycastFilter
    {
        protected virtual void OnPopulateMesh(VertexHelper vh)
        {
        }

        protected virtual void UpdateGeometry()
        {
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera) => true;
    }
}