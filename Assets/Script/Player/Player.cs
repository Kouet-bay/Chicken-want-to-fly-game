using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Glide Settings (Automatic Chicken Fall)")]
    [SerializeField] private float glideGravity = -3.0f; 

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private Vector3 dashDirection = Vector3.forward;

    private CharacterController controller;
    private Transform camTransform; 
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        
        isGrounded = controller.isGrounded;

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(moveX, 0f, moveZ).normalized;

        
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash(inputDir);
        }

        if (isDashing)
        {
            velocity.x = dashDirection.x * dashSpeed;
            velocity.z = dashDirection.z * dashSpeed;

            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
            }
        }
        else
        {
            Vector3 moveDir = Vector3.zero;

            if (inputDir != Vector3.zero)
            {
                if (camTransform != null)
                {
                    Vector3 forward = camTransform.forward;
                    Vector3 right = camTransform.right;

                    forward.y = 0f;
                    right.y = 0f;
                    forward.Normalize();
                    right.Normalize();

                    moveDir = (forward * inputDir.z + right * inputDir.x).normalized;
                }
                else
                {
                    moveDir = inputDir;
                }

                Turn(moveDir);
            }

            velocity.x = moveDir.x * movementSpeed;
            velocity.z = moveDir.z * movementSpeed;

            
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                Jump();
            }
        }

        
        if (isGrounded)
        {
            if (velocity.y < 0)
            {
                velocity.y = -2f; 
            }
        }
        else
        {
            
            if (velocity.y < 0)
            {
                velocity.y += glideGravity * Time.deltaTime;
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }
        }

        
        controller.Move(velocity * Time.deltaTime);
    }

    public void Turn(Vector3 moveDirection)
    {
        if (moveDirection != Vector3.zero && !float.IsNaN(moveDirection.x) && moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            targetRotation.eulerAngles = new Vector3(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = targetRotation; 
        }
    }

    public void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void StartDash(Vector3 inputDir)
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        dashCooldownTimer = dashCooldown;

        if (inputDir != Vector3.zero && camTransform != null)
        {
            Vector3 forward = camTransform.forward;
            Vector3 right = camTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            dashDirection = (forward * inputDir.z + right * inputDir.x).normalized;
        }
        else if (inputDir != Vector3.zero)
      {
            dashDirection = inputDir;
        }
        else
        {
            dashDirection = transform.forward;
        }

        dashDirection.y = 0f;
        dashDirection.Normalize();

        if (dashDirection == Vector3.zero || float.IsNaN(dashDirection.x))
        {
            dashDirection = transform.forward;
        }

        Turn(dashDirection);
    }
}