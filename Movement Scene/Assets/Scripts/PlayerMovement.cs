using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Deze Getallen kan je zelf invoeren in de Inspector van Unity. (Basis Values.)

//Belangrijk! geef de vloer, map of environment een Layer.
//Selecteer die layer dan in ground check. Zo val je niet door de vloer.

//Move speed = 6 
//Air Multiplier = 0.4
//Walk Speed = 4 
//Sprint Speed = 6 (Speed met Shift.)
//Acceleration = 10
//Jump Force = 14 (Hoe hoog je springt.)
//Ground Drag = 6 
//Air Drag = 2 (Hoe lang je in de lucht blijft.)

//Keybinds

//Jump Key = Space 
//Sprint Key = Left Shift

public class PlayerMovement : MonoBehaviour
{
    float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;

    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    //[Header("Crouching")]
    //public float crouchSpeed;
    //public float CrouchYScale;
    //private float startYScale;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    //[SerializeField] KeyCode crouchKey = KeyCode.C;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;
    float groundDistance = 0.4f;

    Vector3 moveDirection;
    Vector3 SlopeMoveDirection;

    Rigidbody rb;

    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        print(isGrounded);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        //if (Input.GetKey(crouchKey))
        {
            //moveSpeed = crouchSpeed;
        }

        SlopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
    }

    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;

        //Start crouch
        //if (Input.GetKeyDown(crouchKey))
        {
            //transform.localScale = new Vector3(transform.localScale.x, CrouchYScale, transform.localScale.z);
            //rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        //Stop Crouch
        //if (Input.GetKeyUp(crouchKey))
        {
            //transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            //rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
           
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && isGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        //On Slope
        if (isGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
        }
        //In Air
        else if (isGrounded && OnSlope())
        {
            rb.AddForce(SlopeMoveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
        //On ground
        else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
        }
    }
}