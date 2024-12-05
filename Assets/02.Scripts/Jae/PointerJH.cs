using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerJH : MonoBehaviour
{
    public Transform pointer; // ���� ���� ����
    public Color noHitColor = Color.red; // ���̰� ���� �ʾ��� �� ����
    public Color hitColor = Color.green; // ���̰� ����� �� ����
    public Color highlightColor = Color.cyan; // ���̿� ����� �� ���̶���Ʈ ����
    public Color clickColor = Color.gray; // Ŭ�� �� ������ ȿ�� ����
    private RaycastHit hit;
    public LineRenderer lineRenderer;
    public InputManager xrinput;
    public LayerMask interactable;
    public float rayDistance = 5f;
    private Outline currentOutline;
    private GameObject highlightedObject = null; // ���� ���̶���Ʈ�� ������Ʈ
    public StartButtonManager startButtonManager; // StartButtonManager ����
    public GameObject O_UI;  // O_UI ������Ʈ
    public GameObject X_UI;  // X_UI ������Ʈ
    public CardMatchingGame cardMatchingGame;

    private bool isFlipping = false; // ī�� ������ ������ üũ�ϴ� �÷���
    private void Start()
    {
        lineRenderer.startColor = noHitColor;
        lineRenderer.endColor = noHitColor;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // ����ĳ��Ʈ ����
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, rayDistance, interactable))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            lineRenderer.enabled = true;
            lineRenderer.startColor = hitColor;
            lineRenderer.endColor = hitColor;

            // ������ �������� ���� ����
            lineRenderer.SetPosition(0, pointer.position); // ������
            lineRenderer.SetPosition(1, hit.point);        // ���� 

            // ���̶���Ʈ ó��
            HandleHighlight(hit.collider.gameObject);

            // �±׷� ��ȣ�ۿ� ���� ���� �˻�
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject") || hit.collider.CompareTag("UI")
                || hit.collider.CompareTag("StartButton") || hit.collider.CompareTag("Card") || hit.collider.CompareTag("Button")))
            {
                TriggerSelect(hit.collider.gameObject);
            }

            // �浹�� ������Ʈ�� �ƿ����� ó��
            ShowOutline(hit.collider.gameObject);
        }
        else
        {
            lineRenderer.enabled = false;
            ClearHighlight();
            ClearOutline();
        }
    }

    // ���̶���Ʈ ó��
    private void HandleHighlight(GameObject obj)
    {
        if (highlightedObject == obj)
            return;

        // ���� ���̶���Ʈ ����
        ClearHighlight();

        // ���ο� ������Ʈ ���̶���Ʈ ����
        highlightedObject = obj;
        Image image = highlightedObject.GetComponent<Image>();
        if (image != null)
        {
            image.color = highlightColor; // ���̶���Ʈ �������� ����
        }

        // ���̶���Ʈ ȿ���� ���� �̺�Ʈ Ʈ����
        IPointerEnterHandler enterHandler = obj.GetComponent<IPointerEnterHandler>();
        if (enterHandler != null)
        {
            PointerEventData pointerEnterEventData = new PointerEventData(EventSystem.current);
            enterHandler.OnPointerEnter(pointerEnterEventData);
        }
    }

    // ���̶���Ʈ ����
    private void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            // ���� ���̶���Ʈ�� ������Ʈ�� ���� ����
            Image image = highlightedObject.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.white; // �⺻ �������� ����
            }

            // ���̶���Ʈ ������ ���� �̺�Ʈ Ʈ����
            IPointerExitHandler exitHandler = highlightedObject.GetComponent<IPointerExitHandler>();
            if (exitHandler != null)
            {
                PointerEventData pointerExitEventData = new PointerEventData(EventSystem.current);
                exitHandler.OnPointerExit(pointerExitEventData);
            }

            highlightedObject = null;
        }
    }

    // Ŭ�� ȿ�� ó��
    private void TriggerSelect(GameObject obj)
    {
        Debug.Log($"��ȣ�ۿ�: {obj.name}");

        // StartButton�� Ŭ���Ǹ� ���� ���� ó��
        if (obj.CompareTag("StartButton"))
        {
            if (startButtonManager != null)
            {
                startButtonManager.StartGame();
            }
            else
            {
                Debug.LogError("startButtonManager is not assigned!");
            }
        }
        // Card�� Ŭ���Ǹ� CardID�� Ȯ���Ͽ� �ش� UI�� Ȱ��ȭ
        if (obj.CompareTag("Card"))
        {
            if (cardMatchingGame != null)
            {
                cardMatchingGame.OnCardSelected(obj); // ī�� ���� ó��
            }
            // ī�� ������Ʈ���� Card ������Ʈ�� �����ɴϴ�.
            Card card = obj.GetComponent<Card>();

            if (card != null)
            {
                card.Flip();
                // �ش� ī���� CardID�� UI ����
                CheckForMatchingCards(card.CardID);
            }
            else
            {
                Debug.LogError("Card component is not attached to the card!");
            }
        }
        // UI ��ư ��ȣ�ۿ� ó��
        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            // Ŭ�� �� ���� ����
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                StartCoroutine(FlashClickEffect(image));
            }
            // ��ư�� Ŭ�� �̺�Ʈ ȣ��
            button.onClick.Invoke();
        }

    }

    // Ŭ�� �� ������ ȿ��
    private System.Collections.IEnumerator FlashClickEffect(Image image)
    {
        Color originalColor = image.color; // Ŭ�� ���� �̹����� ������ ����
        image.color = clickColor; // Ŭ�� �������� ����

        // Ŭ�� ���¸� ��� ����
        yield return new WaitForSeconds(0.1f);

        // ���� �������� ����, ���� ���̶���Ʈ�� ������Ʈ���� Ȯ��
        if (highlightedObject == image.gameObject)
        {
            image.color = highlightColor; // ���̶���Ʈ �������� ����
        }
        else
        {
            image.color = Color.white; // �⺻ �������� ����
        }
    }

    // �ƿ����� ó��
    private void ShowOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();

        if (outline != null)
        {
            if (currentOutline != outline)
            {
                ClearOutline();
                currentOutline = outline;
                currentOutline.enabled = true; // ���� ������Ʈ�� �ƿ����� Ȱ��ȭ
            }
        }
        else
        {
            ClearOutline();
        }
    }

    // �ƿ����� ����
    private void ClearOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
    // ������ CardID�� ���� ī�尡 2�� �̻� �ִ��� üũ
    private void CheckForMatchingCards(int cardID)
    {
        Debug.Log($"Checking for matching CardID: {cardID}");

        // �� �� ��� Card ������Ʈ ã��
        Card[] allCards = FindObjectsOfType<Card>();
        int matchingCardsCount = 0;
        Card firstCard = null; // ù ��° ��ġ�ϴ� ī�带 ����

        foreach (Card card in allCards)
        {
            if (card.CardID == cardID)
            {
                matchingCardsCount++; // ������ CardID�� ���� ī�� �� ����
                if (firstCard == null)
                    firstCard = card; // ù ��° ��ġ�ϴ� ī�带 ���
            }
        }

        // ������ ī�尡 2�� �̻� �ִ� ���
        if (matchingCardsCount >= 2)
        {
            O_UI.SetActive(true);  // O_UI Ȱ��ȭ
            X_UI.SetActive(false); // X_UI ��Ȱ��ȭ
            StartCoroutine(DisableUIAfterDelay(O_UI)); // 2�� �� O_UI ��Ȱ��ȭ
        }
        else
        {
            X_UI.SetActive(true);  // X_UI Ȱ��ȭ
            O_UI.SetActive(false); // O_UI ��Ȱ��ȭ
            StartCoroutine(DisableUIAfterDelay(X_UI)); // 2�� �� X_UI ��Ȱ��ȭ

            // Ʋ�� ī�带 �ٽ� ������
            if (firstCard != null && firstCard.isFlipped)
            {
                StartCoroutine(FlipCardBack(firstCard)); // 2�� �� �ٽ� ������
            }
        }
    }


    private System.Collections.IEnumerator FlipCardBack(Card card)
    {
        yield return new WaitForSeconds(2f); // 2�� ���
        card.Flip(); // ī�� ������
        isFlipping = false; // ������ �Ϸ� �� �Է� �簳
    }

    // UI ��Ȱ��ȭ �� �Է� �����ϰ� ����� �޼���
    private System.Collections.IEnumerator DisableUIAfterDelay(GameObject ui)
    {
        yield return new WaitForSeconds(2f); // 2�� ���
        ui.SetActive(false); // UI ��Ȱ��ȭ
    }
}
