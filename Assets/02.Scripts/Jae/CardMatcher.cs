using System.Collections;
using UnityEngine;

public class CardMatcher : MonoBehaviour
{
    private Card firstCard;   // 첫 번째로 선택된 카드
    private Card secondCard;  // 두 번째로 선택된 카드
    public float resetDelay = 1.0f; // 매칭 실패 시 카드 뒤집기 대기 시간

    public void OnCardSelected(Card selectedCard)
    {
        if (firstCard == null) // 첫 번째 카드 선택
        {
            firstCard = selectedCard;
        }
        else if (secondCard == null) // 두 번째 카드 선택
        {
            secondCard = selectedCard;
            StartCoroutine(CheckMatch()); // 두 장이 선택되면 매칭 검사
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(resetDelay);

        // 카드 ID 비교 (각 카드의 ID를 가진 ItemID 변수)
        int firstCardID = firstCard.CardID;
        int secondCardID = secondCard.CardID;

        if (firstCardID == secondCardID)
        {
            Debug.Log("Match!");
            // 매칭 성공 시 두 카드는 그대로 유지
        }
        else
        {
            Debug.Log("No Match!");
            // Flip() 호출하여 카드 뒤집기
            firstCard.GetComponent<FlipCard>().Flip();
            secondCard.GetComponent<FlipCard>().Flip();
        }

        // 상태 초기화
        firstCard = null;
        secondCard = null;
    }
}
