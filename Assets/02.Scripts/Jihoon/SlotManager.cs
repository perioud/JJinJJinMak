using UnityEngine;

[System.Serializable]
public class SlotData
{
    public GameObject slotPrefab;      // ����� ���� ���� ������
    public GameObject[] cityObjects;  // ���� �Ϸ� �� Ȱ��ȭ�� ���� ������Ʈ
    public GameObject[] relatedObjects; // ���� �Ϸ� �� ������ ���� ������Ʈ
}

public class SlotManager : MonoBehaviour
{
    public SlotData[] slots;          // ��� ���� ������ �迭
    public GameObject[] designPrefabs; // ���� ������Ʈ �迭 (UI�� ǥ��)
    public GameUIController gameUIController; // GameUIController ����

    private int currentSlotIndex = 0; // ���� ���� ���� ���� �ε���
    private GameObject activeSlot;    // ���� Ȱ��ȭ�� ����
    private GameObject activeDesign;  // ���� Ȱ��ȭ�� ���� ������Ʈ
    private GridSlot[] activeGridSlots; // ���� Ȱ��ȭ�� ���Ե��� GridSlot �迭

    private bool isMissionComplete = false;

    void Awake()
    {
        // ���� ���� �� ��� ���԰� ���� ��Ȱ��ȭ
        DeactivateAllSlotsAndDesigns();
    }

    public void StartGame()
    {
        // ���� ���� �� ù ��° ���԰� ���� �ʱ�ȭ
        currentSlotIndex = 0;
        isMissionComplete = false;
        InitializeCurrentSlot();
    }

    private void InitializeCurrentSlot()
    {
        // ��� ���� ��Ȱ��ȭ
        foreach (SlotData slotData in slots)
        {
            if (slotData.slotPrefab != null)
                slotData.slotPrefab.SetActive(false);
        }

        // ��� ���� ��Ȱ��ȭ
        foreach (GameObject design in designPrefabs)
        {
            if (design != null)
                design.SetActive(false);
        }

        // ���� ���� Ȱ��ȭ
        if (currentSlotIndex < slots.Length)
        {
            activeSlot = slots[currentSlotIndex].slotPrefab;
            if (activeSlot != null)
            {
                activeSlot.SetActive(true);
                activeGridSlots = activeSlot.GetComponentsInChildren<GridSlot>();
                Debug.Log($"���� ���� {activeSlot.name} Ȱ��ȭ��");
            }

            activeDesign = designPrefabs[currentSlotIndex];
            if (activeDesign != null)
            {
                activeDesign.SetActive(true);
                Debug.Log($"���� ���� {activeDesign.name} Ȱ��ȭ��");
            }
        }
        else
        {
            Debug.LogWarning("���� �ε����� ������ �ʰ��߽��ϴ�!");
        }
    }

    private void DeactivateAllSlotsAndDesigns()
    {
        foreach (SlotData slotData in slots)
        {
            if (slotData.slotPrefab != null)
                slotData.slotPrefab.SetActive(false);
        }

        foreach (GameObject design in designPrefabs)
        {
            if (design != null)
                design.SetActive(false);
        }
    }

    private bool CheckMissionCompletion()
    {
        if (activeGridSlots == null) return false;

        foreach (GridSlot slot in activeGridSlots)
        {
            if (!slot.IsCorrectlyOccupied())
            {
                return false;
            }
        }

        return true;
    }

    private void CompleteMission()
    {
        isMissionComplete = true;

        foreach (GridSlot slot in activeGridSlots)
        {
            slot.ClearSlot();
        }

        foreach (GameObject obj in slots[currentSlotIndex].relatedObjects)
        {
            Destroy(obj);
            Debug.Log($"{obj.name} ������");
        }

        foreach (GameObject obj in slots[currentSlotIndex].cityObjects)
        {
            obj.SetActive(true);
            Debug.Log($"{obj.name} Ȱ��ȭ��!");
        }

        Debug.Log($"�̼� ����: {activeSlot.name}");

        ProceedToNextSlot();
    }

    private void ProceedToNextSlot()
    {
        if (activeSlot != null)
        {
            activeSlot.SetActive(false);
        }

        if (activeDesign != null)
        {
            activeDesign.SetActive(false);
        }

        currentSlotIndex++;
        if (currentSlotIndex < slots.Length)
        {
            isMissionComplete = false;
            InitializeCurrentSlot();
        }
        else
        {
            Debug.Log("��� �̼� �Ϸ�!");
            if (gameUIController != null)
            {
                gameUIController.ShowGameClearUI(); // ���� Ŭ���� UI ǥ��
            }
        }
    }

    void Update()
    {
        if (!isMissionComplete && CheckMissionCompletion())
        {
            Debug.Log("�̼� �Ϸ� ���� ����!");
            CompleteMission();
        }
    }
}
