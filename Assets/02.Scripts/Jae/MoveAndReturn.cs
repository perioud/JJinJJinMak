using UnityEngine;

public class MoveAndReturn : MonoBehaviour
{
    public Vector3 targetPosition; // �̵��� ��ġ (���� ��ǥ)
    public float moveSpeed = 5.0f; // �̵� �ӵ�
    public float pauseTime = 1.0f; // ��ǥ ��ġ���� ��� �ð�
    private Vector3 originalPosition; // ���� ��ġ ����
    private Quaternion originalRotation; // ���� ȸ���� ����
    public Quaternion initialRotation = Quaternion.Euler(40f, 180f, 180f); // �ʱ� ȸ���� ����

    private bool isAnimating = false; // �ִϸ��̼� ���� ����

    private void Start()
    {
        originalPosition = transform.position; // ���� �� ���� ��ġ ����
        originalRotation = transform.rotation; // ���� �� ���� ȸ���� ����
    }

    // �̵� �ִϸ��̼� ����
    public void StartMoveAndResetRotation()
    {
        if (isAnimating) return;

        isAnimating = true;
        StartCoroutine(AnimateMovementWithRotationReset());
    }

    private System.Collections.IEnumerator AnimateMovementWithRotationReset()
    {
        // �̵�: ���� ��ġ -> ��ǥ ��ġ
        yield return StartCoroutine(MoveToPosition(targetPosition));

        // ��ǥ ��ġ���� ȸ���� �ʱ�ȭ
        transform.rotation = originalRotation;

        // ��ǥ ��ġ���� ��� ���
        yield return new WaitForSeconds(pauseTime);

        //// ��ǥ ��ġ���� ȸ���� ����
        //transform.rotation = originalRotation;

        // �̵�: ��ǥ ��ġ -> ���� ��ġ
        yield return StartCoroutine(MoveToPosition(originalPosition));


        isAnimating = false; // �ִϸ��̼� ����
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            // �ε巴�� �̵�
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // ��Ȯ�� ��ǥ ������ ��ġ��Ŵ
        transform.position = target;
    }
}
