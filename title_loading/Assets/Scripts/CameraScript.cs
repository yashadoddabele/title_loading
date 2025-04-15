using UnityEngine;

// Script to handle camera movement and interaction
public class CameraScript : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float minX, maxX;

    // For smooth scrolling
    private void LateUpdate() {
        Vector3 desiredPosition = new Vector3(playerTransform.position.x + offset.x, transform.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    public float getMaxX() {
        return maxX;
    }
    
    public float getMinX() {
        return minX;
    }

}
