using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speeds
    public float walkSpeed = 5f;       // Speed when walking
    public float sprintSpeed = 9f;     // Speed when sprinting
    public float crouchSpeed = 2.5f;   // Speed when crouching

    // Stamina system
    public float maxStamina = 5f;      // Maximum stamina player can have
    public float staminaRegen = 2f;    // Stamina regeneration rate (per second)
    public float currentStamina;      // Current stamina value
    private bool canSprint = true;     // Whether the player is allowed to sprint
    private bool isSprinting = false;  // Whether the player is currently sprinting

    // Crouch
    private bool isCrouching = false;  // Whether the player is currently crouching

    // Movement
    private float currentSpeed;               // Current movement speed based on state
    private CharacterController controller;   // Reference to Unity’s CharacterController

    // Footstep audio
    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;   // Audio source for footsteps
    public AudioClip walkClip;                // Audio clip for walking
    public AudioClip runClip;                 // Audio clip for sprinting

    [Header("Footstep Volume")]
    [Range(0f, 1.5f)] public float walkVolume = 0.6f; // Volume for walking clip
    [Range(0f, 10f)] public float runVolume = 1.0f;  // Volume for running clip

    [Header("Footstep Settings")]
    public float movementThreshold = 0.1f;    // Threshold to consider the player as "moving" (currently unused)

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;

        // Setup footstep audio
        if (footstepAudioSource != null && walkClip != null)
        {
            footstepAudioSource.clip = walkClip;
            footstepAudioSource.loop = true;
            footstepAudioSource.spatialBlend = 0f; // Play in 2D
            footstepAudioSource.volume = walkVolume;
            footstepAudioSource.Stop(); // Wait for input before playing
            Debug.Log("Footstep audio initialized.");
        }
        else
        {
            Debug.LogWarning("Footstep AudioSource or walkClip is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();          // Check player inputs
        Movement();             // Move the player
        Stamina();              // Handle stamina changes
        HandleFootstepAudio();  // Play footsteps based on movement input
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleInput()
    {
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Check if player should sprint
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching && canSprint)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // Set appropriate speed
        if (isCrouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }

    void Stamina()
    {
        if (isSprinting)
        {
            currentStamina -= Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false;
                isSprinting = false;
            }
        }
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;

                if (currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                    canSprint = true;
                }
            }
        }
    }

    void HandleFootstepAudio()
    {
        // Detect if movement keys are being pressed
        bool isMovingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                             Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Only play sound if moving and not crouching
        if (isMovingInput && !isCrouching)
        {
            // Select correct clip based on sprinting state
            AudioClip targetClip = isSprinting ? runClip : walkClip;
            float targetVolume = isSprinting ? runVolume : walkVolume;

            // Switch clip or adjust volume if needed
            if (footstepAudioSource.clip != targetClip)
            {
                footstepAudioSource.clip = targetClip;
                footstepAudioSource.volume = targetVolume;
                footstepAudioSource.loop = true;
                footstepAudioSource.Play();
            }
            else if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.volume = targetVolume;
                footstepAudioSource.Play();
            }
        }
        else
        {
            // Stop audio if no input or crouching
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

    // Collision detection for escape object
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.Instance.escapeObject)
        {
            Debug.Log("Player reached the escape object!");
            GameManager.Instance.WinGame();
        }
    }

    public float CurrentStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }


}
