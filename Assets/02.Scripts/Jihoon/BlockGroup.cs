using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public List<Rigidbody> blocksInGroup = new List<Rigidbody>(); // �׷쿡 ���� ����� ������ٵ� ���
    private Rigidbody groupRigidbody; // �׷��� �߽� ������ٵ�

    void Start()
    {
        // �׷��� �߽ɿ� ������ ������ٵ� �߰�
        groupRigidbody = gameObject.AddComponent<Rigidbody>();
        groupRigidbody.isKinematic = true; // �̵����� ����

        // �׷쿡 ���� ��� ����� �׷��� �ڽ����� ����
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.transform.SetParent(transform); // ��� ����� �� �׷��� �ڽ����� ����
        }

        // �׷��� �߽�(Pivot) ���
        RecalculateCenterPivot();
    }

    public void RecalculateCenterPivot()
    {
        if (blocksInGroup.Count == 0) return;

        // ��� ����� ��� ��ġ�� ����Ͽ� �߽������� ����
        Vector3 center = Vector3.zero;
        foreach (Rigidbody rb in blocksInGroup)
        {
            center += rb.transform.localPosition;
        }
        center /= blocksInGroup.Count;

        // �� ����� ��ġ�� �߽����� �°� ����
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.transform.localPosition -= center;
        }

        // �׷��� ��ġ�� ���� ����� �߽������� �̵�
        transform.localPosition += center;
    }

    public void SetKinematicState(bool isKinematic)
    {
        groupRigidbody.isKinematic = isKinematic;
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.isKinematic = isKinematic;
            rb.useGravity = !isKinematic; // ����� �׷��� �� �߷��� ��
        }
    }

    public void ApplyVelocity(Vector3 velocity, Vector3 angularVelocity)
    {
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }
}
