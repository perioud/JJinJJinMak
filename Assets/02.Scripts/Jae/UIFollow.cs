using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Transform targetObject; // 따라다닐 3D 오브젝트
    public RectTransform uiElement; // 따라다니는 UI (RectTransform)
    public Vector3 positionOffset; // 위치 오프셋
    public Vector3 rotationOffset; // 회전 오프셋

    void Update()
    {
        if (targetObject == null || uiElement == null)
            return;

        // 1. 위치 동기화 (오프셋 포함)
        uiElement.position = targetObject.position + positionOffset;

        // 2. 회전 동기화 (오프셋 포함)
        uiElement.rotation = targetObject.rotation * Quaternion.Euler(rotationOffset);
    }
}

