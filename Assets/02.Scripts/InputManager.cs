using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InputManager : MonoBehaviour
{
    // �Է� �׼�
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
        // ������� ����̽��� �̸�
        string DeviceName = XRSettings.loadedDeviceName;

        if (DeviceName.Contains("OpenVR"))
        {
            Debug.Log("Vive Pro �Ǵ� SteamVR");
        }
        else if (DeviceName.Contains("Oculus"))
        {
            Debug.Log("Meta Quest 2 �Ǵ� Oculus ����̽�");
        }
        else
        {
            Debug.Log("�� �� ���� ����̽� ��� ��: " + DeviceName);
        }
    }

    private void Update()
    {
        // �׸� ��ư �Է� ���� ����
        HandleGripInput();

        // Ʈ���� ��ư �Է� ���� ����
        HandleTriggerInput();

        // ���̽�ƽ �Է� ���� ����
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
        // �׸� ��ư�� ���� ���¸� Ȯ��
        bool isGripPressed = gripAction.action.ReadValue<float>() > 0.5f;


        if (isGripPressed && !wasGripPressed)
        {
            Debug.Log("Grip ��ư�� ����");
        }


        if (!isGripPressed && wasGripPressed)
        {
            Debug.Log("Grip ��ư�� ����");
        }

        // ���� ���¸� ���� ���·� ������Ʈ
        wasGripPressed = isGripPressed;
    }

    private void HandleTriggerInput()
    {
        // Ʈ���� ��ư�� ���� ���¸� Ȯ��
        bool isTriggerPressed = triggerAction.action.ReadValue<float>() > 0.5f;


        if (isTriggerPressed && !wasTriggerPressed)
        {
            Debug.Log("Trigger ��ư�� ����");
        }


        if (!isTriggerPressed && wasTriggerPressed)
        {
            Debug.Log("Trigger ��ư�� ����");
        }

        // ���� ���¸� ���� ���·� ������Ʈ
        wasTriggerPressed = isTriggerPressed;
    }

    //private void HandleJoystickInput()
    //{
    //    // ���̽�ƽ�� �Է� ���� ����
    //    Vector2 joystickValue = joystickAction.action.ReadValue<Vector2>();

    //    // ���̽�ƽ�� �����̰� ���� ���� �α� ���
    //    if (joystickValue != Vector2.zero)
    //    {
    //        Debug.Log($"���̽�ƽ ��: {joystickValue}");
    //    }
    //}
}