using UnityEngine;

public class XButton : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("mouse clicked");
        if (gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
        }
        else {
            Destroy(transform.parent.gameObject);
        }
    }
}
