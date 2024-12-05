using UnityEngine;

public class MoveAndReturn : MonoBehaviour
{
    public Vector3 targetPosition; // 이동할 위치 (월드 좌표)
    public float moveSpeed = 5.0f; // 이동 속도
    public float pauseTime = 1.0f; // 목표 위치에서 대기 시간
    private Vector3 originalPosition; // 원래 위치 저장
    private Quaternion originalRotation; // 원래 회전값 저장
    public Quaternion initialRotation = Quaternion.Euler(40f, 180f, 180f); // 초기 회전값 설정

    private bool isAnimating = false; // 애니메이션 진행 여부

    private void Start()
    {
        originalPosition = transform.position; // 시작 시 원래 위치 저장
        originalRotation = transform.rotation; // 시작 시 원래 회전값 저장
    }

    // 이동 애니메이션 시작
    public void StartMoveAndResetRotation()
    {
        if (isAnimating) return;

        isAnimating = true;
        StartCoroutine(AnimateMovementWithRotationReset());
    }

    private System.Collections.IEnumerator AnimateMovementWithRotationReset()
    {
        // 이동: 현재 위치 -> 목표 위치
        yield return StartCoroutine(MoveToPosition(targetPosition));

        // 목표 위치에서 회전을 초기화
        transform.rotation = originalRotation;

        // 목표 위치에서 잠시 대기
        yield return new WaitForSeconds(pauseTime);

        //// 목표 위치에서 회전을 복원
        //transform.rotation = originalRotation;

        // 이동: 목표 위치 -> 원래 위치
        yield return StartCoroutine(MoveToPosition(originalPosition));


        isAnimating = false; // 애니메이션 종료
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            // 부드럽게 이동
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // 정확히 목표 지점에 위치시킴
        transform.position = target;
    }
}
