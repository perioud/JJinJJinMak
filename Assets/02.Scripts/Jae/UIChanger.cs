using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public GameObject uiToDisable; // 비활성화할 UI
    public GameObject uiToEnable;  // 활성화할 UI

    // 버튼 클릭 시 호출되는 함수
    public void ChangeUI()
    {
        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false); // UI 비활성화
        }

        if (uiToEnable != null)
        {
            uiToEnable.SetActive(true);  // UI 활성화
        }

        Debug.Log("UI 변경 완료: " +
                  (uiToDisable != null ? $"{uiToDisable.name} 비활성화" : "비활성화할 UI 없음") + ", " +
                  (uiToEnable != null ? $"{uiToEnable.name} 활성화" : "활성화할 UI 없음"));
    }
}
