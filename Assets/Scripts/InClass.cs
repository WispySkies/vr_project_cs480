using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InClass : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inClass;
    public GameObject player;
    void Start()
    {
        if (inClass) {
            player.transform.position = GameObject.Find("ClassroomExitPosition").transform.position;
        }
        inClass = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player (or XR setup) collided with the trigger
        if (other.CompareTag("Player"))  // Assuming you tagged your XR setup as "Player"
        {
            // Load the target scene
            inClass = true;
            SceneManager.LoadScene("Classroom");
        }
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
