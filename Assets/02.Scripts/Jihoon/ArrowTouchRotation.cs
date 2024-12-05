using UnityEngine;

public class ArrowTouchRotation : MonoBehaviour
{
    [Header("References")]
    public Transform cityTransform;   // 회전할 도시의 Transform

    [Header("Rotation Settings")]
    public float rotationSpeed = 100.0f; // 회전 속도
    public bool isLeftArrow = true;      // 왼쪽 화살표인지 여부

    [Header("Press Effect Settings")]
    public float pressDepth = 0.1f;      // 눌림 깊이
    public float pressSpeed = 10f;       // 눌림 효과 속도

    private bool isTouching = false;     // 컨트롤러와 닿았는지 여부
    private bool isPressed = false;     // 눌림 상태
    private Vector3 initialPosition;    // 화살표의 초기 위치
    private Vector3 pressedPosition;    // 눌린 상태의 위치

    private void Start()
    {
        // 초기 위치 설정
        initialPosition = transform.localPosition;
        pressedPosition = initialPosition + Vector3.down * pressDepth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            isTouching = true;
            isPressed = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            isTouching = true; // 충돌이 지속적으로 유지됨
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            isTouching = false;
            isPressed = false; // 충돌 종료
        }
    }

    private void Update()
    {
        HandlePressEffect(); // 눌림 애니메이션 처리
        HandleRotation();    // 회전 처리
    }

    private void HandlePressEffect()
    {
        // 눌림 애니메이션 처리
        if (isPressed)
        {
            // 눌린 상태로 이동
            transform.localPosition = Vector3.Lerp(transform.localPosition, pressedPosition, pressSpeed * Time.deltaTime);
        }
        else
        {
            // 원래 위치로 복귀
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, pressSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        // 부드럽게 회전 처리
        if (isTouching)
        {
            float rotationDirection = isLeftArrow ? -1 : 1;
            cityTransform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }
}
