using UnityEngine;

public class BlockGroupManager : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 참조
    private BlockGroup currentBlockGroup; // 현재 그랩된 블록 그룹

    public void HandleBlockGroupGrab(Rigidbody grabbedObject, Transform handTransform)
    {
        // BlockGroup을 찾기
        BlockGroup blockGroup = grabbedObject.GetComponentInParent<BlockGroup>();
        if (blockGroup == null) return;

        if (currentBlockGroup != null)
        {
            // 이미 그랩된 그룹이 있다면 기존 그룹을 놓음
            DropBlockGroup();
        }

        // 새로운 그룹을 그랩
        currentBlockGroup = blockGroup;
        blockGroup.SetKinematicState(true);

        // 블록 그룹을 손에 연결
        blockGroup.transform.SetParent(handTransform);
        blockGroup.transform.position = handTransform.position;
        blockGroup.transform.rotation = handTransform.rotation;
    }

    public void DropBlockGroup()
    {
        if (currentBlockGroup == null) return;

        // 블록 그룹을 놓음
        currentBlockGroup.transform.SetParent(null);
        currentBlockGroup.SetKinematicState(false);

        // GrabObject의 속도를 그룹에 적용
        currentBlockGroup.ApplyVelocity(player.rotation * GetComponent<GrabObject>().xrinput.VelocityInput(),
                                        player.rotation * GetComponent<GrabObject>().xrinput.AngularVelocityInput());

        currentBlockGroup = null;
    }
}
