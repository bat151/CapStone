using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement speeds
    public float walkSpeed = 5f;       
    public float sprintSpeed = 9f;     
    public float crouchSpeed = 2.5f;  

    // Stamina system
    public float maxStamina = 5f;      
    public float staminaRegen = 2f;    
    public float currentStamina;      
    private bool canSprint = true;     
    private bool isSprinting = false;  

    // Crouch
    private bool isCrouching = false;  

    // Movement
    private float currentSpeed;               
    private CharacterController controller;   

    // Footstep audio
    public AudioSource footstepAudioSource;   
    public AudioClip walkClip;                
    public AudioClip runClip;                 

    // range for sounds used for the AI tracking
    [Range(0f, 1.5f)] public float walkVolume = 0.6f; 
    [Range(0f, 10f)] public float runVolume = 1.0f;  

    // minimum input to count as movement
    public float movementThreshold = 0.1f;

    // Range settings (how far and loud the sound plays to be picked up 
    public float WalkSoundRange = 5f;
    public float SprintSoundRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // get charecter controller and initialize stamina and speed
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;

        // Setup footstep audio
        if (footstepAudioSource != null && walkClip != null)
        {
            footstepAudioSource.clip = walkClip; // set the walk sound
            footstepAudioSource.loop = true; // loop the sound
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
        float x = Input.GetAxis("Horizontal"); // A/D left right movement
        float z = Input.GetAxis("Vertical"); // W/S up down movement

        // convert input and move the charecter
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    // handle player input for crouch and sprint
    void HandleInput()
    {
        // hold ctrl to crouch
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Check if player should sprint, if they have stamina and are not crouching
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching && canSprint)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        // Set appropriate speed depending on what input
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

    // update stamina depending on what movement
    void Stamina()
    {
        // decrease stamian while sprinting
        if (isSprinting)
        {
            currentStamina -= Time.deltaTime;

            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                canSprint = false; // cant sprint
                isSprinting = false; // stop sprint
            }
        }
        // regen stamina when not sprinting
        else
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;

                if (currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                    canSprint = true; // let player sprint again
                }
            }
        }
    }

    // handle footstep audio and broadcast the sound to the enemy "AI sound Tracking"
    void HandleFootstepAudio()
    {
        // detect if movement keys are being pressed
        bool isMovingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                             Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        // only play sound if moving and not crouching
        if (isMovingInput && !isCrouching)
        {
            // select correct clip based on sprinting state
            AudioClip targetClip = isSprinting ? runClip : walkClip;
            float targetVolume = isSprinting ? runVolume : walkVolume;

            // switch clip or adjust volume if needed
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

            // broadcast the sound to the enemy AI
            float loudness = isSprinting ? SprintSoundRange : WalkSoundRange;
            SoundEventManager.BroadcastSound(transform.position, loudness);
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

    // return stamina, used for the UI
    public float CurrentStaminaNormalized()
    {
        return currentStamina / maxStamina;
    }


}
