using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    public Transform targetObject; // ����ٴ� 3D ������Ʈ
    public RectTransform uiElement; // ����ٴϴ� UI (RectTransform)
    public Vector3 positionOffset; // ��ġ ������
    public Vector3 rotationOffset; // ȸ�� ������

    void Update()
    {
        if (targetObject == null || uiElement == null)
            return;

        // 1. ��ġ ����ȭ (������ ����)
        uiElement.position = targetObject.position + positionOffset;

        // 2. ȸ�� ����ȭ (������ ����)
        uiElement.rotation = targetObject.rotation * Quaternion.Euler(rotationOffset);
    }
}

