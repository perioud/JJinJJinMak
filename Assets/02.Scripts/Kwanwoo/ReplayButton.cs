using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    public XylophoneGame game;

    public void ReplaySelectedSong()
    {
        if (!game.isSequenceRunning && game.correctOrder != null)
        {
            Debug.Log("���õ� �� �ٽ� ��� ����");
            game.StartCoroutine(game.StartCircleSequence());
        }
        else
        {
            Debug.Log("���� ��� ���̰ų� ���õ� ���� �����ϴ�.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // �浹�� ��ü�� �̸��� "Stick"�� ���
        if (other.gameObject.tag == "Stick")
        {
            ReplaySelectedSong();
        }
    }
}