
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;

    [Header("Damage Settings")]
    [SerializeField] private float damage = 10f;

    private void Start()
    {
        // Tự hủy viên đạn sau khoảng thời gian quy định
        Destroy(gameObject, timeDestroy);
    }

    private void Update()
    {
        MoveBullet();
    }

    private void MoveBullet()
    {
        // Bay thẳng theo hướng bên phải của viên đạn
        transform.Translate(
            Vector2.right * moveSpeed * Time.deltaTime,
            Space.Self
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Tìm Enemy kể cả khi Collider nằm ở object con
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            // Gây sát thương cho Enemy
            enemy.TakeDamage(damage);

            // Hủy viên đạn sau khi trúng Enemy
            Destroy(gameObject);
        }
    }
}