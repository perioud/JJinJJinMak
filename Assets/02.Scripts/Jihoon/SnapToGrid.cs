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
            // 위치와 회전을 슬롯과 동일하게 설정
            transform.position = other.transform.position;
            transform.rotation = other.transform.rotation;

            // Rigidbody 비활성화
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // 물리 비활성화
                rb.useGravity = false; // 중력 비활성화
            }

            isSnapped = true; // 고정 완료
            Debug.Log($"블럭 {gameObject.name}이 위치와 회전을 슬롯에 맞췄습니다.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(gameObject.tag))
        {
            isSnapped = false; // 슬롯에서 벗어남
        }
    }
}
