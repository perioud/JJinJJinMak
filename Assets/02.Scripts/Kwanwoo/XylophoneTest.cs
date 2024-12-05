using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneTest : MonoBehaviour
{
    public AudioSource audioSource; // 유니티 에디터에서 직접 AudioSource 지정
    public string noteColor; // 음판 색깔
    public XylophoneGame game; // XylophoneGame 스크립트 참조

    void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체의 이름이 "Stick"인 경우
        if (other.tag == "Stick")
        {
            game.PlayXylophone(noteColor);  // XylophoneGame 스크립트의 PlayXylophone 메소드 호출
            audioSource.Play(); // 음성 파일 재생
            Debug.Log("Color played: " + noteColor); // 현재 재생된 색깔 확인
        }
    }
}
