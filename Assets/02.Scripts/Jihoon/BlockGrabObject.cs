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
        if (attachedGroup != null) return; // 이미 그랩 중이면 아무 동작 안함

        // 손의 위치 근처의 Collider를 찾기
        Collider[] nearbyColliders = Physics.OverlapSphere(handTransform.position, 0.2f);
        foreach (var collider in nearbyColliders)
        {
            BlockGroup blockGroup = collider.GetComponentInParent<BlockGroup>();
            if (blockGroup != null)
            {
                // 블록 그룹을 찾았으면 그랩
                attachedGroup = blockGroup.GetComponent<Rigidbody>();
                currentBlockGroup = blockGroup;

                // 블록 그룹의 모든 블록을 kinematic으로 전환하고 중력을 끔
                blockGroup.SetKinematicState(true);

                // 잡기 전에 블록 그룹의 스케일을 1로 초기화
                blockGroup.transform.localScale = Vector3.one;

                // 블록 그룹을 손의 자식으로 설정 (스케일 변경 방지)
                blockGroup.transform.SetParent(handTransform, false);

                // 블록 그룹의 로컬 위치 및 회전을 손의 로컬 기준으로 설정
                blockGroup.transform.localPosition = Vector3.zero;
                blockGroup.transform.localRotation = Quaternion.identity;

                break;
            }
        }
    }

    private void DropBlockGroup()
    {
        if (attachedGroup == null || currentBlockGroup == null) return;

        // 블록 그룹을 놓을 때
        currentBlockGroup.transform.SetParent(null, true); // 부모로부터 분리

        currentBlockGroup.SetKinematicState(true); // 중력은 꺼진 상태를 유지

        // 속도와 회전 적용 (필요 시 사용)
        //currentBlockGroup.ApplyVelocity(Vector3.zero, Vector3.zero); // 놓을 때 속도는 0으로 설정

        attachedGroup = null;
        currentBlockGroup = null;
    }
}
