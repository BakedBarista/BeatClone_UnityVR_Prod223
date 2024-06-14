using UnityEngine;

public class DestroyObjectsOnContact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null && !other.CompareTag("GameController"))
        {
            Destroy(other.attachedRigidbody.gameObject);
        }
        else if (!other.CompareTag("GameController"))
        {
            Destroy(other.gameObject);
        }
    }
}
