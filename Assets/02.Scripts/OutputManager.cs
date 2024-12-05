using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class OutputManager : MonoBehaviour
{
    public float hapticStrength = 0.5f;
    public float hapticDuration = 0.1f;

    private XRController controller;

    private void Start()
    {
        controller = GetComponent<XRController>();
    }

    public void TriggerHaptic(float strength, float duration)
    {
        if (controller != null && controller.inputDevice.TryGetHapticCapabilities(out HapticCapabilities haptic) && haptic.supportsImpulse)
        {
            controller.inputDevice.SendHapticImpulse(0, strength, duration);
        }
    }

    public void TriggerDefaultHapticFeedback()
    {
        TriggerHaptic(hapticStrength, hapticDuration);
    }

    public void StopHaptics()
    {
        if (controller != null)
        {
            controller.inputDevice.StopHaptics();
        }
    }
}
