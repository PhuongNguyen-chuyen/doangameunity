
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Gun Rotation")]
    [SerializeField] private float rotateOffset = 0f;

    [Header("Shooting Settings")]
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;

    [Header("Ammo Settings")]
    [SerializeField] private int maxAmmo = 24;
    public int currentAmmo;

    private float nextShot;

    private Camera mainCamera;
    private Vector3 originalScale;

    private void Start()
    {
        currentAmmo = maxAmmo;

        mainCamera = Camera.main;
        originalScale = transform.localScale;
    }

    private void Update()
    {
        RotateGun();
        Shoot();
        ReLoad();
    }

    // Xoay súng theo vị trí chuột
    private void RotateGun()
    {
        if (Mouse.current == null || mainCamera == null)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (mousePosition.x < 0 || mousePosition.x > Screen.width ||
            mousePosition.y < 0 || mousePosition.y > Screen.height)
        {
            return;
        }

        float distanceToCamera = Mathf.Abs(
            mainCamera.transform.position.z - transform.position.z
        );

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, distanceToCamera)
        );

        Vector2 direction = mouseWorldPosition - transform.position;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        float angle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            0f,
            0f,
            angle + rotateOffset
        );

        // Lật súng khi ngắm sang trái
        Vector3 scale = originalScale;

        if (angle > 90f || angle < -90f)
        {
            scale.y = -Mathf.Abs(originalScale.y);
        }
        else
        {
            scale.y = Mathf.Abs(originalScale.y);
        }

        transform.localScale = scale;
    }

    // Bắn đạn
    private void Shoot()
    {
        if (Mouse.current == null)
            return;

        if (firePos == null || bulletPrefabs == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame &&
            currentAmmo > 0 &&
            Time.time >= nextShot)
        {
            nextShot = Time.time + shotDelay;

            Instantiate(
                bulletPrefabs,
                firePos.position,
                firePos.rotation
            );

            currentAmmo--;
        }
    }

    // Nạp lại đạn bằng chuột phải
    private void ReLoad()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.wasPressedThisFrame &&
            currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}