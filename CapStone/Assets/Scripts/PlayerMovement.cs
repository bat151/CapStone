using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;

    public float sprintSpeed = 9f;
    public float maxStamina = 5f;           
    public float staminaRegen = 2f;     
    private float currentStamina;
    private bool isSprinting = false;

    public float crouchSpeed = 2.5f;
    private bool isCrouching = false;

    private float currentSpeed;
    private CharacterController controller;

    private bool canSprint = true;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;
        currentSpeed = walkSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Movement();
        Stamina();
        
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
        // crtl being held to crouvh
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // shift being held to sprint and make sure crouch is not being used and make sure the player has stamina
        if(Input.GetKey(KeyCode.LeftShift) && currentStamina > 0 && !isCrouching && canSprint)
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

            // make sure stamina cant go below 0 and if it hits zero stop sprint
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
            if(currentStamina < maxStamina)
            {
                currentStamina += staminaRegen * Time.deltaTime;
                
                // once stamkna is regened allow sprint again
                if(currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                    canSprint = true;
                }
            }
        }
    }
}
