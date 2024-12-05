using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineColorHandler : MonoBehaviour
{
    private Outline outline; // Outline ��ũ��Ʈ ����

    private void Start()
    {
        // Outline ��ũ��Ʈ ��������
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline ��ũ��Ʈ�� �� ������Ʈ�� �����ϴ�!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (outline == null) return;

        // �±װ� ��ġ�ϴ� ��� �ʷϻ����� ����
        if (other.CompareTag(gameObject.tag))
        {
            outline.OutlineColor = Color.green;
            outline.enabled = true; // �ƿ����� Ȱ��ȭ
        }
        else
        {
            // �±װ� ��ġ���� �ʴ� ��� ���������� ����
            outline.OutlineColor = Color.red;
            outline.enabled = true; // �ƿ����� Ȱ��ȭ
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (outline == null) return;

        // �浹�� ������ �ƿ����� ��Ȱ��ȭ
        outline.enabled = false;
    }
}
