using System.Collections;
using UnityEngine;

public class StartButtonManager : MonoBehaviour
{
    public GameObject startUI;         // 시작 UI
    public GameObject nextUI;          // 게임 시작 UI
    public CardMatchingGame cardGame;  // 카드 매칭 게임 스크립트
    public LayerMask uiLayer;          // UI 레이어 마스크
    public InputManager xrinput;       // VR 입력 시스템
    private RaycastHit hit;
    public FlipCard Flip;
    private bool isGameStarted = false;

    private void Update()
    {
        if (isGameStarted) return;

        // 레이캐스트를 사용하여 UI 오브젝트와 상호작용
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, uiLayer))  // UI 레이어 마스크를 추가
        {
            if (xrinput.IsTriggerPressed() && hit.collider.CompareTag("StartButton"))
            {
                Debug.Log("Start Game");
                StartGame();
            }
            if (xrinput.IsTriggerPressed() && hit.collider.CompareTag("Card"))
            {
                Flip.Flip();
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        isGameStarted = true;

        // 카드 전부 뒤집기 시작
        StartCoroutine(FlipAllCards());

    }

    public IEnumerator FlipAllCards()
    {
        cardGame.FlipAllCards(); // 카드 뒤집기 호출
        //// 시작 UI 비활성화
        startUI.SetActive(false);
        nextUI.SetActive(true);  // 게임 UI 활성화
        yield return new WaitForSeconds(5f); // 애니메이션 대기 시간
        cardGame.FlipAllCards(); // 카드 뒤집기 호출
    }
}
