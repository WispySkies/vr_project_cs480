// chatgpt written obviously

using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    // Assign these in the inspector
    [SerializeField] private GameObject xrInteractionSetup;
    [SerializeField] private Transform classroomEntryPosition;
    [SerializeField] private Transform classroomExitPosition;

    private void OnTriggerEnter(Collider other)
    {
        // Only proceed if the collider belongs to the XR player setup
        // if (other.gameObject == xrInteractionSetup)
        // {
            // Determine which trigger this script is attached to and teleport accordingly
            if (gameObject.name == "ClassroomEntry_Trigger")
            {
                TeleportPlayer(classroomEntryPosition.position);
            }
            else if (gameObject.name == "ClassroomExit_Trigger")
            {
                TeleportPlayer(classroomExitPosition.position);
            }
        // }
    }

    private void TeleportPlayer(Vector3 targetPosition)
    {
        // Move the XR setup to the target position
        xrInteractionSetup.transform.position = targetPosition;

        // Reset all rigidbody velocities
        Rigidbody[] rigidbodies = xrInteractionSetup.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
