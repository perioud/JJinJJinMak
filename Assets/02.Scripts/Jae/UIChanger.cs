using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public GameObject uiToDisable; // ��Ȱ��ȭ�� UI
    public GameObject uiToEnable;  // Ȱ��ȭ�� UI

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ChangeUI()
    {
        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false); // UI ��Ȱ��ȭ
        }

        if (uiToEnable != null)
        {
            uiToEnable.SetActive(true);  // UI Ȱ��ȭ
        }

        Debug.Log("UI ���� �Ϸ�: " +
                  (uiToDisable != null ? $"{uiToDisable.name} ��Ȱ��ȭ" : "��Ȱ��ȭ�� UI ����") + ", " +
                  (uiToEnable != null ? $"{uiToEnable.name} Ȱ��ȭ" : "Ȱ��ȭ�� UI ����"));
    }
}
