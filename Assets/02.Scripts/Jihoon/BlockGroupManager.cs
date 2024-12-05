using UnityEngine;

public class BlockGroupManager : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform ����
    private BlockGroup currentBlockGroup; // ���� �׷��� ��� �׷�

    public void HandleBlockGroupGrab(Rigidbody grabbedObject, Transform handTransform)
    {
        // BlockGroup�� ã��
        BlockGroup blockGroup = grabbedObject.GetComponentInParent<BlockGroup>();
        if (blockGroup == null) return;

        if (currentBlockGroup != null)
        {
            // �̹� �׷��� �׷��� �ִٸ� ���� �׷��� ����
            DropBlockGroup();
        }

        // ���ο� �׷��� �׷�
        currentBlockGroup = blockGroup;
        blockGroup.SetKinematicState(true);

        // ��� �׷��� �տ� ����
        blockGroup.transform.SetParent(handTransform);
        blockGroup.transform.position = handTransform.position;
        blockGroup.transform.rotation = handTransform.rotation;
    }

    public void DropBlockGroup()
    {
        if (currentBlockGroup == null) return;

        // ��� �׷��� ����
        currentBlockGroup.transform.SetParent(null);
        currentBlockGroup.SetKinematicState(false);

        // GrabObject�� �ӵ��� �׷쿡 ����
        currentBlockGroup.ApplyVelocity(player.rotation * GetComponent<GrabObject>().xrinput.VelocityInput(),
                                        player.rotation * GetComponent<GrabObject>().xrinput.AngularVelocityInput());

        currentBlockGroup = null;
    }
}
