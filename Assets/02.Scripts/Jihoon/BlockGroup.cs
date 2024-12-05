using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    public List<Rigidbody> blocksInGroup = new List<Rigidbody>(); // 그룹에 속한 블록의 리지드바디 목록
    private Rigidbody groupRigidbody; // 그룹의 중심 리지드바디

    void Start()
    {
        // 그룹의 중심에 가상의 리지드바디 추가
        groupRigidbody = gameObject.AddComponent<Rigidbody>();
        groupRigidbody.isKinematic = true; // 이동하지 않음

        // 그룹에 속한 모든 블록을 그룹의 자식으로 설정
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.transform.SetParent(transform); // 모든 블록을 이 그룹의 자식으로 설정
        }

        // 그룹의 중심(Pivot) 계산
        RecalculateCenterPivot();
    }

    public void RecalculateCenterPivot()
    {
        if (blocksInGroup.Count == 0) return;

        // 모든 블록의 평균 위치를 계산하여 중심점으로 설정
        Vector3 center = Vector3.zero;
        foreach (Rigidbody rb in blocksInGroup)
        {
            center += rb.transform.localPosition;
        }
        center /= blocksInGroup.Count;

        // 각 블록의 위치를 중심점에 맞게 조정
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.transform.localPosition -= center;
        }

        // 그룹의 위치를 새로 계산한 중심점으로 이동
        transform.localPosition += center;
    }

    public void SetKinematicState(bool isKinematic)
    {
        groupRigidbody.isKinematic = isKinematic;
        foreach (Rigidbody rb in blocksInGroup)
        {
            rb.isKinematic = isKinematic;
            rb.useGravity = !isKinematic; // 블록이 그랩될 때 중력을 끔
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
