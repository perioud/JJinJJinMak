using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public GameObject UIOn;
    public GameObject UIOff;

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ToggleObjects()
    {
        if (UIOn != null && UIOff != null)
        {
            // ���� ������Ʈ1 Ȱ��ȭ
            UIOn.SetActive(true);

            // ���� ������Ʈ2 ��Ȱ��ȭ
            UIOff.SetActive(false);
        }
    }
}
