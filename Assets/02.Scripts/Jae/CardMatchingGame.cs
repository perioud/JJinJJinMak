using System.Collections;
using UnityEngine;

public class CardMatchingGame : MonoBehaviour
{
    public GameObject[] targetCards;   // ��Ī�� ī��� (a, b, c)
    public int matchedCount;          // ��Ī�� ī�� ��
    public GameObject mismatchUI;     // ����ġ UI
    public GameObject matchUI;        // ��ġ UI
    public GameObject nextUI;         // ���� ���� UI
    public GameObject otherUI;        // Ư�� UI (��Ȱ��ȭ ���)
    public GameObject[] allCards;     // ��� ī�� �迭
    public AudioSource matchAudio;    // ��Ī ���� �����
    public AudioSource flipAudio;     // ī�� ������ �����
    public GameObject nextPointer;         // ���� ���� UI
    public GameObject otherPointer;         // ���� ���� UI
    public MoveAndReturn[] cardMovers;
    public StartButtonManager startButtonManager;

    private void Start()
    {
        matchedCount = 0; // �ʱ�ȭ
    }

    // ī�� ���� ó��
    public void OnCardSelected(GameObject selectedCard)
    {
        if (selectedCard == null)
        {
            Debug.LogError("���õ� ī�尡 null�Դϴ�!");
            return;
        }

        flipAudio.Play();

        Card selectedCardComponent = selectedCard.GetComponent<Card>();
        if (selectedCardComponent == null)
        {
            Debug.LogError("���õ� ī�忡 Card ������Ʈ�� �����ϴ�!");
            return;
        }

        int selectedID = selectedCardComponent.CardID; // ������ ī�� ID
        bool matchFound = false;

        // ������ ī�� ID�� ��Ī�Ǵ� ī�� Ȯ��
        foreach (GameObject targetCard in targetCards)
        {
            Card targetCardComponent = targetCard.GetComponent<Card>();
            if (!targetCardComponent.isMatched && targetCardComponent.CardID == selectedID)
            {
                targetCardComponent.MarkAsMatched(); // ��Ī�� ī�� ��ũ
                matchFound = true;
                break;
            }
        }

        if (matchFound)
        {
            matchedCount++; // ��Ī�� ī�� �� ����
            StartCoroutine(HandleMatch());
            CheckAllCardsFound();
        }
        else
        {
            StartCoroutine(HandleMismatch());
        }
    }

    // ��Ī ����ġ ó��
    private IEnumerator HandleMismatch()
    {
        mismatchUI.SetActive(true);       // ����ġ UI Ȱ��ȭ
        yield return new WaitForSeconds(1f);
        mismatchUI.SetActive(false);      // 1�� �� ��Ȱ��ȭ
    }

    // ��Ī ���� ó��
    private IEnumerator HandleMatch()
    {
        matchUI.SetActive(true);         // ��ġ UI Ȱ��ȭ
        matchAudio.Play();               // ���� ����� ���
        yield return new WaitForSeconds(1f);
        matchUI.SetActive(false);        // 1�� �� ��Ȱ��ȭ
    }

    // ��� ī�尡 ��Ī�Ǿ����� üũ
    private void CheckAllCardsFound()
    {
        // �� ��Ī ī�� ���� ���ݿ� �ش��ϴ� ī�尡 ��Ī�Ǿ����� ���� ����
        if (matchedCount >= targetCards.Length)
        {
            Debug.Log("��� ī�尡 ��Ī�Ǿ����ϴ�!");
            StartCoroutine(ActivateNextUIWithDelay());
        }
    }

    // ���� UI Ȱ��ȭ ���� ��� �ð� �߰�
    private IEnumerator ActivateNextUIWithDelay()
    {
        yield return new WaitForSeconds(2.5f); // 2�� ���
        foreach (GameObject card in allCards)
        {
            MoveAndReturn moveAndReturn = card.GetComponent<MoveAndReturn>();
            if (moveAndReturn != null)
            {
                moveAndReturn.StartMoveAndResetRotation();
            }
        }
        yield return new WaitForSeconds(2f); // 2�� ���

        nextUI.SetActive(true); // ���� UI Ȱ��ȭ
        if (startButtonManager != null)
        {
            yield return StartCoroutine(startButtonManager.FlipAllCards());
        }
        if (otherUI != null) // Ư�� UI�� �ִٸ� ��Ȱ��ȭ
        {
            otherUI.SetActive(false);
        }
        otherPointer.SetActive(false);
        nextPointer.SetActive(true); 
    }

    // ��� ī�带 ������ �޼���
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
