using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;    // speed while walking
    public float sprintSpeed = 9f;  // speed while sprinting
    public float maxStamina = 5f;   // Maximum amount of stamina the player has           
    public float staminaRegen = 2f; // time it takes for the stamina to regen     
    private float currentStamina;
    private bool isSprinting = false;

    public float crouchSpeed = 2.5f;
    private bool isCrouching = false;

    private float currentSpeed;
    private CharacterController controller;

    private bool canSprint = true;

    // Sprint and walk audio
    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;
    public AudioClip walkClip;
    public AudioClip runClip;

    [Header("Footstep Settings")]
    public float movementThreshold = 0.1f;  // How much movement counts as movement

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;

        if (footstepAudioSource != null && walkClip != null)
        {
            footstepAudioSource.clip = walkClip;
            footstepAudioSource.loop = true;
            footstepAudioSource.spatialBlend = 0f; // 2D
            footstepAudioSource.volume = 1f;
            footstepAudioSource.Play();
            Debug.Log("Footstep audio started playing on Player!");
        }
        else
        {
            Debug.LogWarning("AudioSource or walkClip not assigned on Player!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Movement();
        Stamina();
        HandleFootstepAudio();
    }

    void Movement()
    {
        // A and D movement
        float x = Input.GetAxis("Horizontal");
        // W and S movement
        float z = Input.GetAxis("Vertical");

        // Move in direction player is facing
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    void HandleInput()
    {
        // ctrl being held to crouch
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // shift being held to sprint and make sure crouch is not being used and make sure the player has stamina
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching && canSprint)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // set speed based on if walking, sprinting, or crouching
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
            // lower stamina while sprinting
            currentStamina -= Time.deltaTime;

            // make sure stamina can't go below 0 and if it hits zero stop sprint
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false;
                isSprinting = false;
            }
        }
        else
        {
            // stamina regen
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;

                // once stamina is regenerated, allow sprint again
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
        // Check if any movement input is being pressed
        bool isMovingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                             Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // Don't play footsteps if crouching
        if (isMovingInput && !isCrouching)
        {
            // Determine which clip to play
            AudioClip targetClip = isSprinting ? runClip : walkClip;

            // If not already playing or switched clip, update it
            if (footstepAudioSource.clip != targetClip)
            {
                footstepAudioSource.clip = targetClip;
                footstepAudioSource.loop = true;
                footstepAudioSource.Play();
            }
            else if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Play();
            }
        }
        else
        {
            // Stop footstep sounds when no input or crouching
            if (footstepAudioSource.isPlaying)
            {
                footstepAudioSource.Stop();
            }
        }
    }

}
