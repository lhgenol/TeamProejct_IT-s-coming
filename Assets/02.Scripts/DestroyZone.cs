using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Chunk") || other.CompareTag("Obstacle") || other.CompareTag("Item"))
        {
            Destroy(other.gameObject);

        }
    }
}
