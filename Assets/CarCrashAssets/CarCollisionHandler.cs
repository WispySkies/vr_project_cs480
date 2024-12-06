using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class CarCollision : MonoBehaviour
{
    public RawImage imageToDisplay; // Reference to the RawImage component to display the image
    public Canvas imageCanvas; // Reference to the Canvas displaying the image
    public AudioSource crashAudio; // Reference to the AudioSource for the crash sound
    public float restartDelay = 6f; // Delay before restarting the scene
    private bool isTriggered = false; // Ensures the image is shown only once per collision
    public ScooterController controller; // Reference to the scooter controller to check speed

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player and the speed exceeds the threshold
        if (other.CompareTag("Player") && controller.currentSpeed > 5 && !isTriggered)
        {
            Debug.Log("Car Collision Detected");

            // Trigger the collision actions
            isTriggered = true;
            controller.currentSpeed = 0; // Stop the player's movement
            controller.currentVelocity = Vector3.zero;

            // Show the image canvas and the image
            imageCanvas.gameObject.SetActive(true);
            imageToDisplay.gameObject.SetActive(true);

            // Play the crash sound
            if (crashAudio != null)
            {
                crashAudio.Play();
            }

            // Restart the scene after the delay
            Invoke("RestartScene", restartDelay);
        }
    }

    // Restart the scene
    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
