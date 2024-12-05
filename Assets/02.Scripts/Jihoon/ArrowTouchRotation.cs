using UnityEngine;

public class ArrowTouchRotation : MonoBehaviour
{
    [Header("References")]
    public Transform cityTransform;   // ȸ���� ������ Transform

    [Header("Rotation Settings")]
    public float rotationSpeed = 100.0f; // ȸ�� �ӵ�
    public bool isLeftArrow = true;      // ���� ȭ��ǥ���� ����

    [Header("Press Effect Settings")]
    public float pressDepth = 0.1f;      // ���� ����
    public float pressSpeed = 10f;       // ���� ȿ�� �ӵ�

    private bool isTouching = false;     // ��Ʈ�ѷ��� ��Ҵ��� ����
    private bool isPressed = false;     // ���� ����
    private Vector3 initialPosition;    // ȭ��ǥ�� �ʱ� ��ġ
    private Vector3 pressedPosition;    // ���� ������ ��ġ

    private void Start()
    {
        // �ʱ� ��ġ ����
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
            isTouching = true; // �浹�� ���������� ������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Controller"))
        {
            isTouching = false;
            isPressed = false; // �浹 ����
        }
    }

    private void Update()
    {
        HandlePressEffect(); // ���� �ִϸ��̼� ó��
        HandleRotation();    // ȸ�� ó��
    }

    private void HandlePressEffect()
    {
        // ���� �ִϸ��̼� ó��
        if (isPressed)
        {
            // ���� ���·� �̵�
            transform.localPosition = Vector3.Lerp(transform.localPosition, pressedPosition, pressSpeed * Time.deltaTime);
        }
        else
        {
            // ���� ��ġ�� ����
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, pressSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        // �ε巴�� ȸ�� ó��
        if (isTouching)
        {
            float rotationDirection = isLeftArrow ? -1 : 1;
            cityTransform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }
}
