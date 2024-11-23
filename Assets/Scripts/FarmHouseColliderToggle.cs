using UnityEngine;

public class ColliderNegation : MonoBehaviour
{
    public Collider meshCollider; // Reference to the mesh collider you want to negate

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player entered the trigger
        {
            meshCollider.enabled = false; // Disable the mesh collider
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player exited the trigger
        {
            meshCollider.enabled = true; // Re-enable the mesh collider
        }
    }
}
