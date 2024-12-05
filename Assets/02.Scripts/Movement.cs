using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public InputManager LeftxrInput;
    public InputManager RightInput;   // InputManager 참조
    public float movementSpeed = 3f; // 이동 속도
    public float rotationAngle = 45f; // 회전 각도

    private CharacterController characterController;
    private bool hasRotated = false;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 이동 처리
        Vector2 movementInput = LeftxrInput.JoystickInput();
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime;
        characterController.Move(move);

        // 회전 처리
        Vector2 rotationInput = RightInput.JoystickInput();

        if (rotationInput.x > 0.5f && !hasRotated) // 오른쪽으로 회전
        {
            RotateCharacter(rotationAngle);
            hasRotated = true;
        }
        else if (rotationInput.x < -0.5f && !hasRotated) // 왼쪽으로 회전
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
