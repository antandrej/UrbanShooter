using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    private RectTransform crosshairRect;

    void Start()
    {
        crosshairRect = GetComponent<RectTransform>();
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            crosshairRect.parent as RectTransform, 
            mousePosition, 
            null, 
            out localPoint);
        
        crosshairRect.localPosition = localPoint;
    }
}
