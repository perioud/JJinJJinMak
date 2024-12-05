using UnityEngine;

public class ReplayButton : MonoBehaviour
{
    public XylophoneGame game;

    public void ReplaySelectedSong()
    {
        if (!game.isSequenceRunning && game.correctOrder != null)
        {
            Debug.Log("선택된 곡 다시 듣기 실행");
            game.StartCoroutine(game.StartCircleSequence());
        }
        else
        {
            Debug.Log("현재 재생 중이거나 선택된 곡이 없습니다.");
        }
    }
    void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체의 이름이 "Stick"인 경우
        if (other.gameObject.tag == "Stick")
        {
            ReplaySelectedSong();
        }
    }
}