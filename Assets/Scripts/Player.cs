
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Health Settings")]
    [SerializeField] private float maxHp = 100f;

    private float currentHp;
    private bool isDead = false;

    [Header("Health Bar")]
    [SerializeField] private UnityEngine.UI.Image hpBar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();

        currentHp = maxHp;
        UpdateHpBar();
    }

    private void Update()
    {
        if (isDead)
            return;

        moveInput = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
            {
                moveInput.x = -1;
            }

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
            {
                moveInput.x = 1;
            }

            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
            {
                moveInput.y = 1;
            }

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
            {
                moveInput.y = -1;
            }
        }

        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        if (spriteRenderer != null)
        {
            if (moveInput.x < 0)
                spriteRenderer.flipX = true;
            else if (moveInput.x > 0)
                spriteRenderer.flipX = false;
        }

        if (animator != null)
        {
            bool isMoving = moveInput != Vector2.zero;
            animator.SetBool("isRun", isMoving);
        }
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        rb.linearVelocity = moveInput * moveSpeed;
    }

    // Nhận sát thương từ Enemy
    public void TakeDamage(float damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        UpdateHpBar();

        UnityEngine.Debug.Log(
            "Player bị tấn công! Máu còn: " + currentHp + "/" + maxHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // Cập nhật thanh máu
    private void UpdateHpBar()
    {
        if (hpBar != null && maxHp > 0)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    // Player chết
    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        UnityEngine.Debug.Log("Player đã chết!");

        rb.linearVelocity = Vector2.zero;

        Destroy(gameObject);
    }

    // Lấy máu hiện tại
    public float GetCurrentHealth()
    {
        return currentHp;
    }

    // Lấy máu tối đa
    public float GetMaxHealth()
    {
        return maxHp;
    }
}
