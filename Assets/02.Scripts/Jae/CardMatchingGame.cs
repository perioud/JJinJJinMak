using System.Collections;
using UnityEngine;

public class CardMatchingGame : MonoBehaviour
{
    public GameObject[] targetCards;   // 매칭할 카드들 (a, b, c)
    public int matchedCount;          // 매칭된 카드 수
    public GameObject mismatchUI;     // 불일치 UI
    public GameObject matchUI;        // 일치 UI
    public GameObject nextUI;         // 게임 종료 UI
    public GameObject otherUI;        // 특정 UI (비활성화 대상)
    public GameObject[] allCards;     // 모든 카드 배열
    public AudioSource matchAudio;    // 매칭 성공 오디오
    public AudioSource flipAudio;     // 카드 뒤집기 오디오
    public GameObject nextPointer;         // 게임 종료 UI
    public GameObject otherPointer;         // 게임 종료 UI
    public MoveAndReturn[] cardMovers;
    public StartButtonManager startButtonManager;

    private void Start()
    {
        matchedCount = 0; // 초기화
    }

    // 카드 선택 처리
    public void OnCardSelected(GameObject selectedCard)
    {
        if (selectedCard == null)
        {
            Debug.LogError("선택된 카드가 null입니다!");
            return;
        }

        flipAudio.Play();

        Card selectedCardComponent = selectedCard.GetComponent<Card>();
        if (selectedCardComponent == null)
        {
            Debug.LogError("선택된 카드에 Card 컴포넌트가 없습니다!");
            return;
        }

        int selectedID = selectedCardComponent.CardID; // 선택한 카드 ID
        bool matchFound = false;

        // 선택한 카드 ID와 매칭되는 카드 확인
        foreach (GameObject targetCard in targetCards)
        {
            Card targetCardComponent = targetCard.GetComponent<Card>();
            if (!targetCardComponent.isMatched && targetCardComponent.CardID == selectedID)
            {
                targetCardComponent.MarkAsMatched(); // 매칭된 카드 마크
                matchFound = true;
                break;
            }
        }

        if (matchFound)
        {
            matchedCount++; // 매칭된 카드 수 증가
            StartCoroutine(HandleMatch());
            CheckAllCardsFound();
        }
        else
        {
            StartCoroutine(HandleMismatch());
        }
    }

    // 매칭 불일치 처리
    private IEnumerator HandleMismatch()
    {
        mismatchUI.SetActive(true);       // 불일치 UI 활성화
        yield return new WaitForSeconds(1f);
        mismatchUI.SetActive(false);      // 1초 뒤 비활성화
    }

    // 매칭 성공 처리
    private IEnumerator HandleMatch()
    {
        matchUI.SetActive(true);         // 일치 UI 활성화
        matchAudio.Play();               // 성공 오디오 재생
        yield return new WaitForSeconds(1f);
        matchUI.SetActive(false);        // 1초 뒤 비활성화
    }

    // 모든 카드가 매칭되었는지 체크
    private void CheckAllCardsFound()
    {
        // 총 매칭 카드 수의 절반에 해당하는 카드가 매칭되었으면 게임 종료
        if (matchedCount >= targetCards.Length)
        {
            Debug.Log("모든 카드가 매칭되었습니다!");
            StartCoroutine(ActivateNextUIWithDelay());
        }
    }

    // 다음 UI 활성화 전에 대기 시간 추가
    private IEnumerator ActivateNextUIWithDelay()
    {
        yield return new WaitForSeconds(2.5f); // 2초 대기
        foreach (GameObject card in allCards)
        {
            MoveAndReturn moveAndReturn = card.GetComponent<MoveAndReturn>();
            if (moveAndReturn != null)
            {
                moveAndReturn.StartMoveAndResetRotation();
            }
        }
        yield return new WaitForSeconds(2f); // 2초 대기

        nextUI.SetActive(true); // 다음 UI 활성화
        if (startButtonManager != null)
        {
            yield return StartCoroutine(startButtonManager.FlipAllCards());
        }
        if (otherUI != null) // 특정 UI가 있다면 비활성화
        {
            otherUI.SetActive(false);
        }
        otherPointer.SetActive(false);
        nextPointer.SetActive(true); 
    }

    // 모든 카드를 뒤집는 메서드
    public void FlipAllCards()
    {
        foreach (GameObject card in allCards)
        {
            Card cardComponent = card.GetComponent<Card>();
            if (cardComponent != null)
            {
                cardComponent.Flip();
            }
        }
    }
}
