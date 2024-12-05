using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
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
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject") || hit.collider.CompareTag("UI")))
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
}
