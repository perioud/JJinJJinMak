using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public XylophoneGame game;
    public GameObject startUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Stick" && !game.isSequenceRunning)
        {
            Debug.Log("게임 시작");
            startUI.SetActive(false);
            game.SelectRandomSong(); // 게임 시작 시 랜덤으로 곡 선택
            StartCoroutine(game.StartCircleSequence()); // 선택된 곡 시퀀스 시작

        }
    }
}
