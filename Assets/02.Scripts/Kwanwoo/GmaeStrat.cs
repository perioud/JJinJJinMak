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
            Debug.Log("���� ����");
            startUI.SetActive(false);
            game.SelectRandomSong(); // ���� ���� �� �������� �� ����
            StartCoroutine(game.StartCircleSequence()); // ���õ� �� ������ ����

        }
    }
}
