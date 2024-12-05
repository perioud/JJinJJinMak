using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputManager LeftxrInput;
    public InputManager RightInput;   // InputManager ����
    public float movementSpeed = 3f; // �̵� �ӵ�
    public float rotationAngle = 45f; // ȸ�� ����

    private CharacterController characterController;
    private bool hasRotated = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // �̵� ó��
        Vector2 movementInput = LeftxrInput.JoystickInput();
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime;
        characterController.Move(move);

        // ȸ�� ó��
        Vector2 rotationInput = RightInput.JoystickInput();

        if (rotationInput.x > 0.5f && !hasRotated) // ���������� ȸ��
        {
            RotateCharacter(rotationAngle);
            hasRotated = true;
        }
        else if (rotationInput.x < -0.5f && !hasRotated) // �������� ȸ��
        {
            RotateCharacter(-rotationAngle);
            hasRotated = true;
        }
        else if (rotationInput.x == 0)
        {
            hasRotated = false;
        }
    }

    private void RotateCharacter(float angle)
    {
        transform.Rotate(0, angle, 0);
    }
}
