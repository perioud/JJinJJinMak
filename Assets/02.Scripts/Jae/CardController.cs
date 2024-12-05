using System.Collections;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card firstCard; // 첫 번째 선택된 카드
    public Card secondCard; // 두 번째 선택된 카드
    public AudioSource matchSound; // 매칭 성공 시 사운드
    public GameObject mismatchUI; // 매칭 실패 시 UI
    public float uiFadeDelay = 1f; // UI 비활성화 딜레이

    public void OnCardSelected(GameObject selectedCardObject)
    {
        Card selectedCard = selectedCardObject.GetComponent<Card>();
        if (selectedCard == null || selectedCard.isMatched || selectedCard.isFlipped)
        {
            return; // 이미 매칭된 카드나 뒤집힌 카드는 선택하지 않음
        }

        selectedCard.Flip();

        if (firstCard == null)
        {
            firstCard = selectedCard; // 첫 번째 카드로 설정
        }
        else
        {
            secondCard = selectedCard; // 두 번째 카드로 설정
            StartCoroutine(CheckMatch()); // 매칭 검사
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f); // 카드 확인 시간 지연

        if (firstCard.CardID == secondCard.CardID)
        {
            // 매칭 성공
            firstCard.MarkAsMatched();
            secondCard.MarkAsMatched();
            matchSound.Play(); // 사운드 재생
        }
        else
        {
            // 매칭 실패
            mismatchUI.SetActive(true);
            yield return new WaitForSeconds(uiFadeDelay);
            mismatchUI.SetActive(false);

            // 두 카드 원래 상태로 복원
            firstCard.Flip();
            secondCard.Flip();
        }

        // 선택된 카드 초기화
        firstCard = null;
        secondCard = null;
    }
}
