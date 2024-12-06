using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InClass : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inClass;
    void Start()
    {
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        if (inClass) {
            player.transform.position = GameObject.Find("ClassroomExitPosition").transform.position;
        }
        inClass = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
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

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "JamesMadisonCity") {
            GameObject player = GameObject.Find("XR Origin (XR Rig)");
            if (inClass)
            {
                player.transform.position = GameObject.Find("ClassroomExitPosition").transform.position;
            }
            inClass = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
