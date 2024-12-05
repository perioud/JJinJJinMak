using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineColorHandler : MonoBehaviour
{
    private Outline outline; // Outline 스크립트 참조

    private void Start()
    {
        // Outline 스크립트 가져오기
        outline = GetComponent<Outline>();
        if (outline == null)
        {
            Debug.LogError("Outline 스크립트가 이 오브젝트에 없습니다!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (outline == null) return;

        // 태그가 일치하는 경우 초록색으로 설정
        if (other.CompareTag(gameObject.tag))
        {
            outline.OutlineColor = Color.green;
            outline.enabled = true; // 아웃라인 활성화
        }
        else
        {
            // 태그가 일치하지 않는 경우 빨간색으로 설정
            outline.OutlineColor = Color.red;
            outline.enabled = true; // 아웃라인 활성화
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (outline == null) return;

        // 충돌이 끝나면 아웃라인 비활성화
        outline.enabled = false;
    }
}
