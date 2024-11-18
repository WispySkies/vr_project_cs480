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
    public Transform fastEffect;

    private ParticleSystem.MainModule fastEffectMain;
    private ParticleSystem.EmissionModule fastEffectEmission;

    public DynamicMoveProvider moveProvider;

    public Transform player; // Reference to the player (XR Rig or camera)
    public Transform playerCamera;

    public InputActionProperty moveInput; // XR input for forward movement

    public float acceleration = 2f; // Speed increase rate
    public float deceleration = 3f; // Speed decrease rate
    public float maxSpeed = 10f; // Maximum movement speed
    public float minSpeed = 0f; // Minimum movement speed (when stopped)

    private float currentSpeed = 1f; // Current movement speed
    private Vector3 currentVelocity = Vector3.zero; // Current velocity for forward motion

    private bool isGrabbing = false;
    private bool isColliding = false;

    private float rotationSpeed = 150f;
    void Start()
    {
        // Get the XR Grab Interactables on the handle
        var grabInteractable = handle.GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        // Sets the effects for particles for more efficient changing of values
        fastEffectMain = fastEffect.GetComponent<ParticleSystem>().main;
        fastEffectEmission = fastEffect.GetComponent<ParticleSystem>().emission;
        fastEffect.gameObject.SetActive(false);

    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbing = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbing = false;

        // Stop immediately when letting go of the scooter
        currentSpeed = 3f;
        currentVelocity = Vector3.zero;
        moveProvider.moveSpeed = 3f;
    }

    void Update()
    {
        if (isGrabbing)
        {
            // Self explanitory
            HandleTurningInput();
            HandleMovementInput();
            MoveScooter();
            PlayFastEffect();
        }
        else
        {
            // Reset movement if the player releases the scooter
            currentSpeed = 0f;
            currentVelocity = Vector3.zero;
        }

        ApplyMovement();
    }

    private void HandleMovementInput()
    {
        // Read the Vector2 value from the input action
        Vector2 inputVector = moveInput.action.ReadValue<Vector2>();

        // Extract the forward input (y-axis) and turning input (x-axis)
        float forwardInput = inputVector.y;
        float turnInput = inputVector.x;

        // Calculate the forward direction based on the player's camera
        Vector3 forwardDirection = playerCamera.forward;

        forwardDirection.y = 0; // Ignore vertical component
        forwardDirection.Normalize();

        if (forwardInput > 0.1f) // Forward input is active
        {
            // Accelerate up to max speed
            currentSpeed = Mathf.Min(currentSpeed + acceleration * Time.deltaTime, maxSpeed);

            // Update the current velocity
            currentVelocity = forwardDirection * currentSpeed;
        }
        else if (currentSpeed > 0f) // If no forward input, apply deceleration
        {
            // Smooth deceleration
            if (forwardInput < 0f)
            {
                // If player is holding back, slow down faster
                currentSpeed = Mathf.Max(currentSpeed - deceleration * 2f * Time.deltaTime, 0f);
            }
            else
            {
                currentSpeed = Mathf.Max(currentSpeed - deceleration * Time.deltaTime, 0f);
            }

            // Update the velocity after deceleration
            currentVelocity = forwardDirection * currentSpeed;
        }

    }

    private void HandleTurningInput()
    {
        // Read the left/right turn input from the controller (turnInput is a Vector2)
        Vector2 turnInputValue = moveInput.action.ReadValue<Vector2>();

        // Get the horizontal input (left/right axis)
        float turnInputX = turnInputValue.x;

        if (Mathf.Abs(turnInputX) > 0.1f) // If there's a noticeable turn input
        {
            // Rotate the player camera left or right based on the input
            float turnAmount = turnInputX * rotationSpeed * Time.deltaTime;
            player.Rotate(Vector3.up * turnAmount); // Rotate around the Y axis (up)
        }
    }

    private void ApplyMovement()
    {
        // Move the player based on the current velocity
        if (currentVelocity != Vector3.zero && !isColliding)
        {
            player.position += currentVelocity * Time.deltaTime;
        }
    }

    private void MoveScooter()
    {
        // Get the angle of the player camera
        Quaternion q = Quaternion.Euler(0, playerCamera.eulerAngles.y - 90, 0);
        Vector3 relativeTransform = player.position - scooter.position + new Vector3(0.0f, -0.1f, 0.0f);

        // Update scooter position
        scooter.position = Vector3.Lerp(scooter.position, scooter.position + relativeTransform, 2f + Time.deltaTime);
        scooter.rotation = q;

        // Update invisible grabbable handle
        handle.position = scooterNeck.position + new Vector3(0.0f, 0.45f, 0.0f);
        handle.rotation = q;

        fastEffect.position = scooterNeck.position + new Vector3(0.0f, 0.6f, 0.0f) + playerCamera.forward;
        fastEffect.rotation = Quaternion.Euler(0, playerCamera.eulerAngles.y + 180, 0);
    }

    public void PlayFastEffect()
    {
        if (currentVelocity != Vector3.zero)
        {
            fastEffect.gameObject.SetActive(true);
            fastEffectMain.startSpeed = currentVelocity.magnitude * 5;
            fastEffectEmission.rateOverTime = currentVelocity.magnitude * 3;
        }
        else
        {
            fastEffect.gameObject.SetActive(false);
            fastEffectMain.startSpeed = 5;
            fastEffectEmission.rateOverTime = 1;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        isColliding = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
}


