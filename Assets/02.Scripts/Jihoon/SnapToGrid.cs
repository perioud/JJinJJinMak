using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGrid : MonoBehaviour
{
    private bool isSnapped = false;
    public ActiveState activeState;
    private void OnTriggerEnter(Collider other)
    {
        if (!activeState.isGrabbed && !isSnapped && other.CompareTag(gameObject.tag))
        {
            // ��ġ�� ȸ���� ���԰� �����ϰ� ����
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

            // Rigidbody ��Ȱ��ȭ
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // ���� ��Ȱ��ȭ
                rb.useGravity = false; // �߷� ��Ȱ��ȭ
            }

            isSnapped = true; // ���� �Ϸ�
            Debug.Log($"�� {gameObject.name}�� ��ġ�� ȸ���� ���Կ� ������ϴ�.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(gameObject.tag))
        {
            isSnapped = false; // ���Կ��� ���
        }
    }
}
