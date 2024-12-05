using UnityEngine;

public class ReturnObject : MonoBehaviour
{
    public float returnDelay = 2.0f; // 원래 위치로 돌아가는 시간
    public Collider triggerArea; // 돌아가야 하는 영역
    public ActiveState activeState; // 그랩 상태 확인 용

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false; // 리턴 중인지 확인
    private Rigidbody rb;

    private void Start()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();

        if (activeState == null)
        {
            activeState = GetComponent<ActiveState>();
        }
    }

    private void Update()
    {
        if (!activeState.isGrabbed) // 물체가 그랩되지 않았을 때만 이동
        {
            if (!isReturning && IsObjectOutOfBounds())
            {
                Invoke("ReturnToOriginalPosition", returnDelay);
                isReturning = true;
            }
        }
        else
        {
            // 물체가 그랩된 상태에서는 리턴을 취소
            CancelInvoke("ReturnToOriginalPosition");
            isReturning = false; // 리셋
        }
    }

    private bool IsObjectOutOfBounds()
    {
        return !triggerArea.bounds.Contains(transform.position);
    }

    private void ReturnToOriginalPosition()
    {
        if (!activeState.isGrabbed) // 그랩되지 않은 상태에서만 리턴
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // Rigidbody 속도와 회전 초기화
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            isReturning = false;
        }
    }
}
