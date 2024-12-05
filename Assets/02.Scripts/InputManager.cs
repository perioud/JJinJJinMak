using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    // 입력 액션
    public InputActionProperty gripAction;
    public InputActionProperty triggerAction;
    public InputActionProperty joystickAction;
    public InputActionProperty velocity;
    public InputActionProperty angularVelocity;
    public InputActionProperty haptic;

    private bool wasGripPressed = false;
    private bool wasTriggerPressed = false;

    void Start()
    {
        // 사용중인 디바이스의 이름
        string DeviceName = XRSettings.loadedDeviceName;

        if (DeviceName.Contains("OpenVR"))
        {
            Debug.Log("Vive Pro 또는 SteamVR");
        }
        else if (DeviceName.Contains("Oculus"))
        {
            Debug.Log("Meta Quest 2 또는 Oculus 디바이스");
        }
        else
        {
            Debug.Log("알 수 없는 디바이스 사용 중: " + DeviceName);
        }
    }

    private void Update()
    {
        // 그립 버튼 입력 상태 감지
        HandleGripInput();

        // 트리거 버튼 입력 상태 감지
        HandleTriggerInput();

        // 조이스틱 입력 상태 감지
        //HandleJoystickInput();
    }
    public float GripInput()
    {
        return gripAction.action.ReadValue<float>();
    }

    public float TriggerInput()
    {
        return triggerAction.action.ReadValue<float>();
    }

    public Vector2 JoystickInput()
    {
        return joystickAction.action.ReadValue<Vector2>();
    }

    public Vector3 VelocityInput()
    {
        return velocity.action.ReadValue<Vector3>();
    }
    public Vector3 AngularVelocityInput()
    {
        return angularVelocity.action.ReadValue<Vector3>();
    }

    public Vector3 HapticInput()
    {
        return haptic.action.ReadValue<Vector3>();
    }

    public bool IsGripPressed()
    {
        return gripAction.action.ReadValue<float>() > 0.5f;
    }

    public bool IsTriggerPressed()
    {
        return triggerAction.action.ReadValue<float>() > 0.5f;
    }

    private void HandleGripInput()
    {
        // 그립 버튼이 눌린 상태를 확인
        bool isGripPressed = gripAction.action.ReadValue<float>() > 0.5f;


        if (isGripPressed && !wasGripPressed)
        {
            Debug.Log("Grip 버튼이 눌림");
        }


        if (!isGripPressed && wasGripPressed)
        {
            Debug.Log("Grip 버튼이 떼짐");
        }

        // 현재 상태를 이전 상태로 업데이트
        wasGripPressed = isGripPressed;
    }

    private void HandleTriggerInput()
    {
        // 트리거 버튼이 눌린 상태를 확인
        bool isTriggerPressed = triggerAction.action.ReadValue<float>() > 0.5f;


        if (isTriggerPressed && !wasTriggerPressed)
        {
            Debug.Log("Trigger 버튼이 눌림");
        }


        if (!isTriggerPressed && wasTriggerPressed)
        {
            Debug.Log("Trigger 버튼이 떼짐");
        }

        // 현재 상태를 이전 상태로 업데이트
        wasTriggerPressed = isTriggerPressed;
    }

    //private void HandleJoystickInput()
    //{
    //    // 조이스틱의 입력 값을 감지
    //    Vector2 joystickValue = joystickAction.action.ReadValue<Vector2>();

    //    // 조이스틱이 움직이고 있을 때만 로그 출력
    //    if (joystickValue != Vector2.zero)
    //    {
    //        Debug.Log($"조이스틱 값: {joystickValue}");
    //    }
    //}
}