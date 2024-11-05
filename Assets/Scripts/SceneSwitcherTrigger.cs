// chatgpt written obviously

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherTrigger : MonoBehaviour
{
    [SerializeField] private string targetScene;  // The name of the scene to load when this trigger is activated

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player (or XR setup) collided with the trigger
        if (other.CompareTag("Player"))  // Assuming you tagged your XR setup as "Player"
        {
            // Load the target scene
            SceneManager.LoadScene(targetScene);
        }
    }
}
