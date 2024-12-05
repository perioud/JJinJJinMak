using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
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
            if (xrinput.IsTriggerPressed() && (hit.collider.CompareTag("RayInteractable") || hit.collider.CompareTag("InteractionObject") || hit.collider.CompareTag("UI")))
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
}
