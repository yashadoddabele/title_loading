using UnityEngine;

// Implements custom cursor behavior for the game
public class Mouse : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        // Hide standard OS cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
    }

    // Updates this custom cursor to follow exactly where the user moves their OS mouse
    void Update()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        
        transform.position = new Vector3(worldPosition.x, worldPosition.y, 0f);
    }
}
