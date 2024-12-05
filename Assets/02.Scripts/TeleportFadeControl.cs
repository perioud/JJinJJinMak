using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TeleportFadeControl : MonoBehaviour
{
    public InputActionReference inputActionRef;
    public ScreenFade screenFade;

    private bool hasFaded = false;

    private void OnEnable()
    {
        inputActionRef.action.performed += OnPerformed;
        inputActionRef.action.canceled += OnCanceled;
    }

    private void OnDisable()
    {
        inputActionRef.action.performed -= OnPerformed;
        inputActionRef.action.canceled -= OnCanceled;
    }

    public void OnPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Up");
        if (!hasFaded)
        {
            screenFade?.FadeIn();
        }
    }

    public void OnCanceled(InputAction.CallbackContext obj)
    {
        if (!hasFaded)
        {
            screenFade.OnFadeComplete.RemoveAllListeners();
            screenFade.OnFadeComplete.AddListener(() =>
            {
                screenFade?.FadeIn();
                hasFaded = false; 
            });
            screenFade?.FadeOut();
            hasFaded = true; 
        }
    }
}
