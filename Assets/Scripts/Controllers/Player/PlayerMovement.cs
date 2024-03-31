using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;
    public Transform orientation;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;

    private Rigidbody rb;
    private bool grounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        HandleInput();
        ControlSpeed();
    }

    private void FixedUpdate()
    {
        GroundCheck();
        MovePlayer();
    }

    private void GroundCheck()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        rb.drag = grounded ? groundDrag : 0f;
    }

    private void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
    }

    private void MovePlayer()
    {
        if (grounded)
        {
            Vector3 moveDirection = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }

    private void ControlSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void SwitchWeapon(int index)
    {
        if (GameController.Instance.worldObjects != null && index >= 0 && index < GameController.Instance.worldObjects.Length)
        {
            foreach (GameObject obj in GameController.Instance.worldObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }

            if (GameController.Instance.worldObjects[index] != null)
            {
                GameController.Instance.worldObjects[index].SetActive(true);
            }
        }
    }
}
