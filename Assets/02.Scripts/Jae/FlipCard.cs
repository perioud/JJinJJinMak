using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public float flipSpeed = 5.0f; // 회전 속도
    private float targetAngle;
    private bool isAnimating = false;

    // 카드 뒤집기 호출
    public void Flip()
    {
        if (isAnimating) return;

        isAnimating = true;

        // 현재 회전 상태에 따라 목표 각도 변경
        targetAngle = (Mathf.Abs(transform.eulerAngles.z) < 1f) ? 180f : 0f;
    }

    private void Update()
    {
        if (isAnimating)
        {
            // 부드럽게 회전
            float currentZRotation = transform.eulerAngles.z;
            float newZRotation = Mathf.LerpAngle(currentZRotation, targetAngle, Time.deltaTime * flipSpeed);

            // Z축 회전만 변경
            transform.eulerAngles = new Vector3(40f, 180f, newZRotation);

            // 목표 각도에 도달하면 애니메이션 종료
            if (Mathf.Abs(newZRotation - targetAngle) < 0.1f)
            {
                transform.eulerAngles = new Vector3(40f, 180f, targetAngle);
                isAnimating = false;
            }
        }
    }
}

