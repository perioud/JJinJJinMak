using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    public GameObject UIOn;
    public GameObject UIOff;

    // 버튼 클릭 시 호출되는 함수
    public void ToggleObjects()
    {
        if (UIOn != null && UIOff != null)
        {
            // 게임 오브젝트1 활성화
            UIOn.SetActive(true);

            // 게임 오브젝트2 비활성화
            UIOff.SetActive(false);
        }
    }
}
