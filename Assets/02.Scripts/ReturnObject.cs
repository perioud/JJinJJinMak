using UnityEngine;

public class ReturnObject : MonoBehaviour
{
    public float returnDelay = 2.0f; // ���� ��ġ�� ���ư��� �ð�
    public Collider triggerArea; // ���ư��� �ϴ� ����
    public ActiveState activeState; // �׷� ���� Ȯ�� ��

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isReturning = false; // ���� ������ Ȯ��
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
        if (!activeState.isGrabbed) // ��ü�� �׷����� �ʾ��� ���� �̵�
        {
            if (!isReturning && IsObjectOutOfBounds())
            {
                Invoke("ReturnToOriginalPosition", returnDelay);
                isReturning = true;
            }
        }
        else
        {
            // ��ü�� �׷��� ���¿����� ������ ���
            CancelInvoke("ReturnToOriginalPosition");
            isReturning = false; // ����
        }
    }

    private bool IsObjectOutOfBounds()
    {
        return !triggerArea.bounds.Contains(transform.position);
    }

    private void ReturnToOriginalPosition()
    {
        if (!activeState.isGrabbed) // �׷����� ���� ���¿����� ����
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;

            // Rigidbody �ӵ��� ȸ�� �ʱ�ȭ
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            isReturning = false;
        }
    }
}
