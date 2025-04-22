using UnityEngine;

public class XButton : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("mouse clicked");
        if (gameObject.CompareTag("Enemy")) {
            AudioManager.Instance.EnemySound();
            Destroy(gameObject);
        }
        else {
            Destroy(transform.parent.gameObject);
        }
    }
}
