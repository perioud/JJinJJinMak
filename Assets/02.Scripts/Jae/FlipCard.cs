using UnityEngine;

public class FlipCard : MonoBehaviour
{
    public float flipSpeed = 5.0f; // ȸ�� �ӵ�
    private float targetAngle;
    private bool isAnimating = false;

    // ī�� ������ ȣ��
    public void Flip()
    {
        if (isAnimating) return;

        isAnimating = true;

        // ���� ȸ�� ���¿� ���� ��ǥ ���� ����
        targetAngle = (Mathf.Abs(transform.eulerAngles.z) < 1f) ? 180f : 0f;
    }

    private void Update()
    {
        if (isAnimating)
        {
            // �ε巴�� ȸ��
            float currentZRotation = transform.eulerAngles.z;
            float newZRotation = Mathf.LerpAngle(currentZRotation, targetAngle, Time.deltaTime * flipSpeed);

            // Z�� ȸ���� ����
            transform.eulerAngles = new Vector3(40f, 180f, newZRotation);

            // ��ǥ ������ �����ϸ� �ִϸ��̼� ����
            if (Mathf.Abs(newZRotation - targetAngle) < 0.1f)
            {
                transform.eulerAngles = new Vector3(40f, 180f, targetAngle);
                isAnimating = false;
            }
        }
    }
}

