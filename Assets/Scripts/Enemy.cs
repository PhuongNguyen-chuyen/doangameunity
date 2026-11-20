
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] protected float enemyMoveSpeed = 1f;

    protected Player player;

    [Header("Health Settings")]
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;

    [Header("Health Bar")]
    [SerializeField] private UnityEngine.UI.Image hpBar;

    [Header("Contact Damage")]
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;
    [SerializeField] protected float damageInterval = 1f;

    private float nextDamageTime;
    private bool isDead;

    protected virtual void Start()
    {
        player = FindAnyObjectByType<Player>();

        currentHp = maxHp;

        UpdateHpBar();
    }

    protected virtual void Update()
    {
        if (isDead)
            return;

        MoveToPlayer();
    }

    // Di chuyển Enemy đến vị trí Player
    protected virtual void MoveToPlayer()
    {
        if (player != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.transform.position,
                enemyMoveSpeed * Time.deltaTime
            );

            FlipEnemy();
        }
    }

    // Quay mặt Enemy theo hướng Player
    protected virtual void FlipEnemy()
    {
        if (player != null)
        {
            Vector3 scale = transform.localScale;

            if (player.transform.position.x < transform.position.x)
            {
                scale.x = -Mathf.Abs(scale.x);
            }
            else
            {
                scale.x = Mathf.Abs(scale.x);
            }

            transform.localScale = scale;
        }
    }

    // Enemy vừa chạm Player
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        Player target = collision.GetComponentInParent<Player>();

        if (target != null)
        {
            target.TakeDamage(enterDamage);

            nextDamageTime = Time.time + damageInterval;
        }
    }

    // Enemy tiếp tục tiếp xúc với Player
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (isDead || Time.time < nextDamageTime)
            return;

        Player target = collision.GetComponentInParent<Player>();

        if (target != null)
        {
            target.TakeDamage(stayDamage);

            nextDamageTime = Time.time + damageInterval;
        }
    }

    // Enemy nhận sát thương từ đạn
    public virtual void TakeDamage(float damage)
    {
        if (isDead || currentHp <= 0 || damage <= 0)
            return;

        currentHp -= damage;

        currentHp = Mathf.Max(currentHp, 0);

        UpdateHpBar();

        UnityEngine.Debug.Log(
            "Enemy bị tấn công! Máu còn: " + currentHp + "/" + maxHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    // Enemy bị tiêu diệt
    protected virtual void Die()
    {
        if (isDead)
            return;

        isDead = true;

        UnityEngine.Debug.Log("Enemy đã bị tiêu diệt!");

        Destroy(gameObject);
    }

    // Cập nhật thanh máu Enemy
    protected void UpdateHpBar()
    {
        if (hpBar != null && maxHp > 0)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
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
