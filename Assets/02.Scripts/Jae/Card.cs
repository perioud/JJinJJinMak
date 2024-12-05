using UnityEngine;

public class Card : MonoBehaviour
{
    public int CardID; // 카드 고유 ID
    public bool isFlipped = false; // 현재 뒤집힘 상태
    public bool isMatched = false; // 매칭 여부

    private FlipCard flipCard;

    private void Start()
    {
        flipCard = GetComponent<FlipCard>(); // FlipCard 스크립트 참조
    }

    // 카드를 뒤집는 메서드
    public void Flip()
    {
        if (!isMatched) // 매칭된 카드는 뒤집지 않음
        {
            isFlipped = !isFlipped;
            flipCard.Flip();
        }
    }
     
    // 카드 매칭 성공 시 호출
    public void MarkAsMatched()
    {
        isMatched = true;
    }
}
