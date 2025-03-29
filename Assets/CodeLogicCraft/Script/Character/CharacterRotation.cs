using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 3000f;
    public float autoRotateSpeed = 20f;
    public bool autoRotate = true;

    private float currentYRotation;
    private bool isDragging;
    private float touchStartX;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseRotation();
#elif UNITY_ANDROID || UNITY_IOS
            HandleTouchRotation();
#endif

        if (autoRotate)
        {
            AutoRotate();
        }
    }

    void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchStartX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - touchStartX;
            currentYRotation -= deltaX * rotationSpeed * Time.deltaTime * 0.1f;
            transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
            touchStartX = Input.mousePosition.x;
        }
    }

    void HandleTouchRotation()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartX = touch.position.x;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                float deltaX = touch.position.x - touchStartX;
                currentYRotation -= deltaX * rotationSpeed * Time.deltaTime * 0.1f;
                transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
                touchStartX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    void AutoRotate()
    {
        currentYRotation -= autoRotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
    }
}
