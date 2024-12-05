using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PointerJH : MonoBehaviour
{
    public Transform pointer; // 레이 시작 지점
    public Color noHitColor = Color.red; // 레이가 닿지 않았을 때 색상
    public Color hitColor = Color.green; // 레이가 닿았을 때 색상
    public Color highlightColor = Color.cyan; // 레이에 닿았을 때 하이라이트 색상
    public Color clickColor = Color.gray; // 클릭 시 깜빡임 효과 색상
    private RaycastHit hit;
    public LineRenderer lineRenderer;
    public InputManager xrinput;
    public LayerMask interactable;
    public float rayDistance = 5f;
    private Outline currentOutline;
    private GameObject highlightedObject = null; // 현재 하이라이트된 오브젝트
    public StartButtonManager startButtonManager; // StartButtonManager 참조
    public GameObject O_UI;  // O_UI 오브젝트
    public GameObject X_UI;  // X_UI 오브젝트
    public CardMatchingGame cardMatchingGame;

    private bool isFlipping = false; // 카드 뒤집는 중인지 체크하는 플래그
    private void Start()
    {
        lineRenderer.startColor = noHitColor;
        lineRenderer.endColor = noHitColor;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        // 레이캐스트 수행
        if (Physics.Raycast(pointer.position, pointer.forward, out hit, rayDistance, interactable))
        {
            Debug.Log($"Hit: {hit.collider.name}");

            lineRenderer.enabled = true;
            lineRenderer.startColor = hitColor;
            lineRenderer.endColor = hitColor;

            // 라인의 시작점과 끝점 설정
            lineRenderer.SetPosition(0, pointer.position); // 시작점
            lineRenderer.SetPosition(1, hit.point);        // 끝점 

            // 하이라이트 처리
            HandleHighlight(hit.collider.gameObject);

            // 태그로 상호작용 가능 여부 검사
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject") || hit.collider.CompareTag("UI")
                || hit.collider.CompareTag("StartButton") || hit.collider.CompareTag("Card") || hit.collider.CompareTag("Button")))
            {
                TriggerSelect(hit.collider.gameObject);
            }

            // 충돌한 오브젝트의 아웃라인 처리
            ShowOutline(hit.collider.gameObject);
        }
        else
        {
            lineRenderer.enabled = false;
            ClearHighlight();
            ClearOutline();
        }
    }

    // 하이라이트 처리
    private void HandleHighlight(GameObject obj)
    {
        if (highlightedObject == obj)
            return;

        // 이전 하이라이트 해제
        ClearHighlight();

        // 새로운 오브젝트 하이라이트 적용
        highlightedObject = obj;
        Image image = highlightedObject.GetComponent<Image>();
        if (image != null)
        {
            image.color = highlightColor; // 하이라이트 색상으로 변경
        }

        // 하이라이트 효과를 위한 이벤트 트리거
        IPointerEnterHandler enterHandler = obj.GetComponent<IPointerEnterHandler>();
        if (enterHandler != null)
        {
            PointerEventData pointerEnterEventData = new PointerEventData(EventSystem.current);
            enterHandler.OnPointerEnter(pointerEnterEventData);
        }
    }

    // 하이라이트 해제
    private void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            // 기존 하이라이트된 오브젝트의 색상 복원
            Image image = highlightedObject.GetComponent<Image>();
            if (image != null)
            {
                image.color = Color.white; // 기본 색상으로 복원
            }

            // 하이라이트 해제를 위한 이벤트 트리거
            IPointerExitHandler exitHandler = highlightedObject.GetComponent<IPointerExitHandler>();
            if (exitHandler != null)
            {
                PointerEventData pointerExitEventData = new PointerEventData(EventSystem.current);
                exitHandler.OnPointerExit(pointerExitEventData);
            }

            highlightedObject = null;
        }
    }

    // 클릭 효과 처리
    private void TriggerSelect(GameObject obj)
    {
        Debug.Log($"상호작용: {obj.name}");

        // StartButton이 클릭되면 게임 시작 처리
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
        // Card가 클릭되면 CardID를 확인하여 해당 UI를 활성화
        if (obj.CompareTag("Card"))
        {
            if (cardMatchingGame != null)
            {
                cardMatchingGame.OnCardSelected(obj); // 카드 선택 처리
            }
            // 카드 오브젝트에서 Card 컴포넌트를 가져옵니다.
            Card card = obj.GetComponent<Card>();

            if (card != null)
            {
                card.Flip();
                // 해당 카드의 CardID로 UI 제어
                CheckForMatchingCards(card.CardID);
            }
            else
            {
                Debug.LogError("Card component is not attached to the card!");
            }
        }
        // UI 버튼 상호작용 처리
        Button button = obj.GetComponent<Button>();
        if (button != null)
        {
            // 클릭 시 색상 변경
            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                StartCoroutine(FlashClickEffect(image));
            }
            // 버튼의 클릭 이벤트 호출
            button.onClick.Invoke();
        }

    }

    // 클릭 시 깜빡임 효과
    private System.Collections.IEnumerator FlashClickEffect(Image image)
    {
        Color originalColor = image.color; // 클릭 전에 이미지의 색상을 저장
        image.color = clickColor; // 클릭 색상으로 변경

        // 클릭 상태를 잠시 유지
        yield return new WaitForSeconds(0.1f);

        // 원래 색상으로 복원, 현재 하이라이트된 오브젝트인지 확인
        if (highlightedObject == image.gameObject)
        {
            image.color = highlightColor; // 하이라이트 색상으로 복원
        }
        else
        {
            image.color = Color.white; // 기본 색상으로 복원
        }
    }

    // 아웃라인 처리
    private void ShowOutline(GameObject obj)
    {
        Outline outline = obj.GetComponent<Outline>();

        if (outline != null)
        {
            if (currentOutline != outline)
            {
                ClearOutline();
                currentOutline = outline;
                currentOutline.enabled = true; // 현재 오브젝트의 아웃라인 활성화
            }
        }
        else
        {
            ClearOutline();
        }
    }

    // 아웃라인 해제
    private void ClearOutline()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
    }
    // 동일한 CardID를 가진 카드가 2장 이상 있는지 체크
    private void CheckForMatchingCards(int cardID)
    {
        Debug.Log($"Checking for matching CardID: {cardID}");

        // 씬 내 모든 Card 오브젝트 찾기
        Card[] allCards = FindObjectsOfType<Card>();
        int matchingCardsCount = 0;
        Card firstCard = null; // 첫 번째 일치하는 카드를 저장

        foreach (Card card in allCards)
        {
            if (card.CardID == cardID)
            {
                matchingCardsCount++; // 동일한 CardID를 가진 카드 수 증가
                if (firstCard == null)
                    firstCard = card; // 첫 번째 일치하는 카드를 기록
            }
        }

        // 동일한 카드가 2장 이상 있는 경우
        if (matchingCardsCount >= 2)
        {
            O_UI.SetActive(true);  // O_UI 활성화
            X_UI.SetActive(false); // X_UI 비활성화
            StartCoroutine(DisableUIAfterDelay(O_UI)); // 2초 후 O_UI 비활성화
        }
        else
        {
            X_UI.SetActive(true);  // X_UI 활성화
            O_UI.SetActive(false); // O_UI 비활성화
            StartCoroutine(DisableUIAfterDelay(X_UI)); // 2초 후 X_UI 비활성화

            // 틀린 카드를 다시 뒤집기
            if (firstCard != null && firstCard.isFlipped)
            {
                StartCoroutine(FlipCardBack(firstCard)); // 2초 후 다시 뒤집기
            }
        }
    }


    private System.Collections.IEnumerator FlipCardBack(Card card)
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        card.Flip(); // 카드 뒤집기
        isFlipping = false; // 뒤집기 완료 후 입력 재개
    }

    // UI 비활성화 후 입력 가능하게 만드는 메서드
    private System.Collections.IEnumerator DisableUIAfterDelay(GameObject ui)
    {
        yield return new WaitForSeconds(2f); // 2초 대기
        ui.SetActive(false); // UI 비활성화
    }
}
