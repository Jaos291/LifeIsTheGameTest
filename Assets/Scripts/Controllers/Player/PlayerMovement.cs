using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    [HideInInspector] public TextMeshProUGUI text_speed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight *0.5f + 0.2f, whatIsGround); 

        MyInput();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }


        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (GameController.Instance.worldObjects[0])
            {
                for (int i = 0; i < GameController.Instance.worldObjects.Length; i++)
                {
                    if (GameController.Instance.worldObjects[i].activeSelf)
                    {
                        GameController.Instance.worldObjects[i].SetActive(false);
                    }
                }

                GameController.Instance.worldObjects[0].SetActive(true);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            if (GameController.Instance.worldObjects[1])
            {
                for (int i = 0; i < GameController.Instance.worldObjects.Length; i++)
                {
                    if (GameController.Instance.worldObjects[i].activeSelf)
                    {
                        GameController.Instance.worldObjects[i].SetActive(false);
                    }
                }

                GameController.Instance.worldObjects[1].SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (GameController.Instance.worldObjects[2])
            {
                for (int i = 0; i < GameController.Instance.worldObjects.Length; i++)
                {
                    if (GameController.Instance.worldObjects[i].activeSelf)
                    {
                        GameController.Instance.worldObjects[i].SetActive(false);
                    }
                }

                GameController.Instance.worldObjects[2].SetActive(true);
            }
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}