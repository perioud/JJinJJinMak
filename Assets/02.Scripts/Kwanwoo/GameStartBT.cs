using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartBT : MonoBehaviour
{
    // Ȱ��ȭ/��Ȱ��ȭ�� ���� ������Ʈ��
    public GameObject UIon;
    public GameObject UIoff;

    // ��ư Ŭ�� �� ������ �޼���
    public void ToggleObjects()
    {
        if (UIon != null && UIoff != null)
        {
            // ���� ������Ʈ1 Ȱ��ȭ
            UIon.SetActive(true);

            // ���� ������Ʈ2 ��Ȱ��ȭ
            UIoff.SetActive(false);
        }
    }
}
