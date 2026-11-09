using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    // ==========================================
    // 1. HEALTH & LIVES (ระบบหัวใจและความเสียหาย)
    // ==========================================
    [Header("Health Settings")]
    [SerializeField] private int maxHp = 100;
    private int hp;

    public int MaxHp => maxHp;
    public int Hp 
    { 
        get => hp; 
        set => hp = Mathf.Clamp(value, 0, maxHp); 
    }

    [Header("Damage Cooldown / Invulnerability")]
    [SerializeField] private float invulnerabilityDuration = 0.5f; // เวลาอมตะหลังโดนตี (วินาที)เพื่อป้องกันโดนตีซ้ำรัวๆ
    private float invulnerabilityTimer = 0f;
    private bool isInvulnerable = false;

    // ==========================================
    // 2. MOVEMENT & GRAVITY (ระบบการเคลื่อนที่และแรงโน้มถ่วง)
    // ==========================================
    [Header("Movement Settings")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Glide Settings (Automatic Chicken Fall)")]
    [SerializeField] private float glideGravity = -3.0f; 

    // ==========================================
    // 3. DASH & STAMINA (ระบบแดชและสตามิน่า)
    // ==========================================
    [Header("Dash & Stamina Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaCostPerDash = 25f; 
    [SerializeField] private float staminaRegenRate = 25f;   
    [SerializeField] private float staminaRegenDelay = 0.5f; 
    
    private float currentStamina;
    private float staminaRegenTimer = 0f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private Vector3 dashDirection = Vector3.forward;

    public float CurrentStamina => currentStamina;
    public float MaxStamina => maxStamina;

    // ==========================================
    // 4. CARRY & INTERACTION (ระบบหยิบจับและปฏิสัมพันธ์)
    // ==========================================
    [Header("Carry System & Interaction")]
    [SerializeField] private Transform carrySocket;    
    [SerializeField] private float carryRange = 2f;    
    [SerializeField] private LayerMask collectibleLayer;
    private ICollectible carriedItem;                

    // ==========================================
    // 5. COMPONENTS & REFERENCES (ตัวแปรอ้างอิงภายใน)
    // ==========================================
    private CharacterController controller;
    private Transform camTransform; 
    private Vector3 velocity;
    private bool isGrounded;

    // ==========================================
    // UNITY LIFECYCLE METHODS
    // ==========================================
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Hp = maxHp; 
        currentStamina = maxStamina; 
        
        if (Camera.main != null)
        {
            camTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // จัดการคูลดาวน์ Dash
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        // จัดการเวลาอมตะหลังโดนตี
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
            }
        }

        HandleStaminaRegeneration();
        UpdateUI();
        
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(moveX, 0f, moveZ).normalized;
        
        // เช็คการกดปุ่ม Dash (LeftShift)
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            if (currentStamina >= staminaCostPerDash)
            {
                StartDash(inputDir);
            }
            else
            {
                Debug.Log("Stamina ไม่พอสำหรับการ Dash!");
            }
        }

        // ควบคุมการเคลื่อนที่ระหว่าง Dash หรือเดินปกติ
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
            HandleNormalMovement(inputDir);
        }
        
        HandleGravity();
        controller.Move(velocity * Time.deltaTime);
        
        HandleInteractions();
    }

    // ==========================================
    // MOVEMENT & ACTION METHODS
    // ==========================================
    private void HandleNormalMovement(Vector3 inputDir)
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

    private void HandleGravity()
    {
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

        currentStamina -= staminaCostPerDash;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaRegenTimer = 0f; 

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

    // ==========================================
    // STAMINA & UI METHODS
    // ==========================================
    private void HandleStaminaRegeneration()
    {
        if (!isDashing && currentStamina < maxStamina)
        {
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= staminaRegenDelay)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
            }
        }
    }

    private void UpdateUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateStaminaUI(currentStamina, maxStamina);
        }
    }

    // ==========================================
    // INTERACTION & DAMAGE METHODS
    // ==========================================
    private void HandleInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (carriedItem == null)
            {
                if (!TryInteractWithObject())
                {
                    TryPickUpItem();
                }
            }
            else
            {
                carriedItem.OnUse(this);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Q) && carriedItem != null)
        {
            carriedItem.OnDrop();
            carriedItem = null;
        }
    }

    private void TryPickUpItem()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, carryRange, collectibleLayer);
        
        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                ICollectible collectible = hitCollider.GetComponent<ICollectible>();
                if (collectible != null)
                {
                    carriedItem = collectible;
                    carriedItem.OnPickUp(carrySocket); 
                    Debug.Log("Picked up: " + hitCollider.name);
                    break;
                }
            }
        }
    }

    private bool TryInteractWithObject()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position + transform.up * 0.8f, transform.forward, out hit, carryRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Use(this);
                return true; 
            }
        }
        return false;
    }

    public void ClearCarriedItem()
    {
        carriedItem = null;
    }

    // ระบบรับดาเมจ: ป้องกันการโดนตีซ้ำซ้อนด้วยระบบอมตะชั่วขณะ
    public void TakeDamage(int damage, Character attacker)
    {
        if (isInvulnerable) return; // ถ้ากำลังอมตะอยู่จะไม่โดนหักหัวใจซ้ำ

        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityDuration;

        Debug.Log($"Player took damage from {attacker.name}! Losing 1 Heart.");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPlayerTakeDamage();
        }
    }

    public void RespawnAt(Vector3 checkpointPos)
    {
        controller.enabled = false;
        transform.position = checkpointPos;
        controller.enabled = true;

        Hp = maxHp;
        currentStamina = maxStamina; 
        velocity = Vector3.zero;
        
        // รีเซ็ตสถานะอมตะตอนเกิดใหม่เพื่อให้พร้อมรับการเล่นต่อ
        isInvulnerable = false;
        invulnerabilityTimer = 0f;

        Debug.Log("Player respawned at checkpoint.");
    }
}