
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Images")]
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShoot;
    [SerializeField] private Texture2D cursorReLoad;

    [Header("Cursor Settings")]
    [SerializeField] private Vector2 hotspot = new Vector2(16, 48);

    private void Start()
    {
        // Hiển thị con trỏ bình thường khi bắt đầu game
        SetCursor(cursorNormal);
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        // Khi nhấn giữ chuột trái
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            SetCursor(cursorShoot);
        }
        // Khi thả chuột trái
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            SetCursor(cursorNormal);
        }

        // Khi nhấn giữ chuột phải
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            SetCursor(cursorReLoad);
        }
        // Khi thả chuột phải
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            SetCursor(cursorNormal);
        }
    }

    // Hàm thay đổi hình con trỏ
    private void SetCursor(Texture2D cursorTexture)
    {
        if (cursorTexture != null)
        {
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }

    private void OnDisable()
    {
        // Trả con trỏ về trạng thái bình thường
        SetCursor(cursorNormal);
    }
}
