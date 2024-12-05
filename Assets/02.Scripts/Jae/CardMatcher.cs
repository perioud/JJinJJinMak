using System.Collections;
using UnityEngine;

public class CardMatcher : MonoBehaviour
{
    private Card firstCard;   // ù ��°�� ���õ� ī��
    private Card secondCard;  // �� ��°�� ���õ� ī��
    public float resetDelay = 1.0f; // ��Ī ���� �� ī�� ������ ��� �ð�

    public void OnCardSelected(Card selectedCard)
    {
        if (firstCard == null) // ù ��° ī�� ����
        {
            firstCard = selectedCard;
        }
        else if (secondCard == null) // �� ��° ī�� ����
        {
            secondCard = selectedCard;
            StartCoroutine(CheckMatch()); // �� ���� ���õǸ� ��Ī �˻�
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(resetDelay);

        // ī�� ID �� (�� ī���� ID�� ���� ItemID ����)
        int firstCardID = firstCard.CardID;
        int secondCardID = secondCard.CardID;

        if (firstCardID == secondCardID)
        {
            Debug.Log("Match!");
            // ��Ī ���� �� �� ī��� �״�� ����
        }
        else
        {
            Debug.Log("No Match!");
            // Flip() ȣ���Ͽ� ī�� ������
            firstCard.GetComponent<FlipCard>().Flip();
            secondCard.GetComponent<FlipCard>().Flip();
        }

        // ���� �ʱ�ȭ
        firstCard = null;
        secondCard = null;
    }
}
