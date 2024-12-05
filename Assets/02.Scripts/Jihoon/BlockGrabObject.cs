using System.Collections.Generic;
using UnityEngine;

public class BlockGrabObject : MonoBehaviour
{
    public InputManager xrinput;
    public Transform player;
    private Transform handTransform;
    private Rigidbody attachedGroup;
    private BlockGroup currentBlockGroup;

    void Start()
    {
        handTransform = GetComponent<Transform>();
    }

    void Update()
    {
        if (xrinput.IsGripPressed())
        {
            GrabBlockGroup();
        }
        else
        {
            DropBlockGroup();
        }
    }

    private void GrabBlockGroup()
    {
        if (attachedGroup != null) return; // �̹� �׷� ���̸� �ƹ� ���� ����

        // ���� ��ġ ��ó�� Collider�� ã��
        Collider[] nearbyColliders = Physics.OverlapSphere(handTransform.position, 0.2f);
        foreach (var collider in nearbyColliders)
        {
            BlockGroup blockGroup = collider.GetComponentInParent<BlockGroup>();
            if (blockGroup != null)
            {
                // ��� �׷��� ã������ �׷�
                attachedGroup = blockGroup.GetComponent<Rigidbody>();
                currentBlockGroup = blockGroup;

                // ��� �׷��� ��� ����� kinematic���� ��ȯ�ϰ� �߷��� ��
                blockGroup.SetKinematicState(true);

                // ��� ���� ��� �׷��� �������� 1�� �ʱ�ȭ
                blockGroup.transform.localScale = Vector3.one;

                // ��� �׷��� ���� �ڽ����� ���� (������ ���� ����)
                blockGroup.transform.SetParent(handTransform, false);

                // ��� �׷��� ���� ��ġ �� ȸ���� ���� ���� �������� ����
                blockGroup.transform.localPosition = Vector3.zero;
                blockGroup.transform.localRotation = Quaternion.identity;

                break;
            }
        }
    }

    private void DropBlockGroup()
    {
        if (attachedGroup == null || currentBlockGroup == null) return;

        // ��� �׷��� ���� ��
        currentBlockGroup.transform.SetParent(null, true); // �θ�κ��� �и�

        currentBlockGroup.SetKinematicState(true); // �߷��� ���� ���¸� ����

        // �ӵ��� ȸ�� ���� (�ʿ� �� ���)
        //currentBlockGroup.ApplyVelocity(Vector3.zero, Vector3.zero); // ���� �� �ӵ��� 0���� ����

        attachedGroup = null;
        currentBlockGroup = null;
    }
}
