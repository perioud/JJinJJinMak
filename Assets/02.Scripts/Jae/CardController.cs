using System.Collections;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public Card firstCard; // ù ��° ���õ� ī��
    public Card secondCard; // �� ��° ���õ� ī��
    public AudioSource matchSound; // ��Ī ���� �� ����
    public GameObject mismatchUI; // ��Ī ���� �� UI
    public float uiFadeDelay = 1f; // UI ��Ȱ��ȭ ������

    public void OnCardSelected(GameObject selectedCardObject)
    {
        Card selectedCard = selectedCardObject.GetComponent<Card>();
        if (selectedCard == null || selectedCard.isMatched || selectedCard.isFlipped)
        {
            return; // �̹� ��Ī�� ī�峪 ������ ī��� �������� ����
        }

        selectedCard.Flip();

        if (firstCard == null)
        {
            firstCard = selectedCard; // ù ��° ī��� ����
        }
        else
        {
            secondCard = selectedCard; // �� ��° ī��� ����
            StartCoroutine(CheckMatch()); // ��Ī �˻�
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f); // ī�� Ȯ�� �ð� ����

        if (firstCard.CardID == secondCard.CardID)
        {
            // ��Ī ����
            firstCard.MarkAsMatched();
            secondCard.MarkAsMatched();
            matchSound.Play(); // ���� ���
        }
        else
        {
            // ��Ī ����
            mismatchUI.SetActive(true);
            yield return new WaitForSeconds(uiFadeDelay);
            mismatchUI.SetActive(false);

            // �� ī�� ���� ���·� ����
            firstCard.Flip();
            secondCard.Flip();
        }

        // ���õ� ī�� �ʱ�ȭ
        firstCard = null;
        secondCard = null;
    }
}
