using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XylophoneTest : MonoBehaviour
{
    public AudioSource audioSource; // ����Ƽ �����Ϳ��� ���� AudioSource ����
    public string noteColor; // ���� ����
    public XylophoneGame game; // XylophoneGame ��ũ��Ʈ ����

    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �̸��� "Stick"�� ���
        if (other.tag == "Stick")
        {
            game.PlayXylophone(noteColor);  // XylophoneGame ��ũ��Ʈ�� PlayXylophone �޼ҵ� ȣ��
            audioSource.Play(); // ���� ���� ���
            Debug.Log("Color played: " + noteColor); // ���� ����� ���� Ȯ��
        }
    }
}
