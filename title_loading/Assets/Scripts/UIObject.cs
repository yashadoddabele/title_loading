using UnityEngine;

public class UIObject : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    public float minX, maxX;
    public float minY, maxY;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // Method implementing clickability
    void OnMouseDown()
    {
        offset = transform.position - getMouseOffset();
    }

    // Method implementing dragging
    void OnMouseDrag()
    {
        transform.position = getMouseOffset() + offset;
        ClampPosition();
    }

    private Vector3 getMouseOffset() {
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPos.z);
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mousePos);
        return mouseWorldPos;
    }

    // Dont let user move UI object out of screen bounds
    private void ClampPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}

