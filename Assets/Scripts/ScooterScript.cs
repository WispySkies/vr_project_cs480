using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class ScooterController : MonoBehaviour
{
    public Transform handle;
    public Transform scooter;
    public Transform scooterNeck;

    public DynamicMoveProvider moveProvider;

    public Transform player; // Reference to the player (XR Rig or camera)
    public Transform playerCamera;

    private bool isGrabbing = false;


    void Start()
    {
        // Get the XR Grab Interactables on the handles
        var grabInteractable = handle.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbing = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;

    }

    void Update()
    {
        if (isGrabbing)
        {
            moveProvider.moveSpeed = 10f;
            MoveScooter();
        }
        else
        {
            moveProvider.moveSpeed = 1f;
        }
    }

    private void MoveScooter()
    {
        Quaternion q = Quaternion.Euler(0, playerCamera.eulerAngles.y - 90, 0);
        Vector3 relativeTransform = player.position - scooter.position + new Vector3(0.0f, -0.1f, 0.0f);

        scooter.position = Vector3.Lerp(scooter.position, scooter.position + relativeTransform, 2f + Time.deltaTime);
        scooter.rotation = q;

        handle.position = scooterNeck.position + new Vector3(0.0f, 0.45f, 0.0f);
        handle.rotation = q;
    }
}
