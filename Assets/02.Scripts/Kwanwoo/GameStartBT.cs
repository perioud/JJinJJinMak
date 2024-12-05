using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartBT : MonoBehaviour
{
    // 활성화/비활성화할 게임 오브젝트들
    public GameObject UIon;
    public GameObject UIoff;

    // 버튼 클릭 시 실행할 메서드
    public void ToggleObjects()
    {
        if (UIon != null && UIoff != null)
        {
            // 게임 오브젝트1 활성화
            UIon.SetActive(true);

            // 게임 오브젝트2 비활성화
            UIoff.SetActive(false);
        }
    }
}
