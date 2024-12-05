using UnityEngine;

[System.Serializable]
public class SlotData
{
    public GameObject slotPrefab;      // 블록을 맞출 슬롯 프리팹
    public GameObject[] cityObjects;  // 슬롯 완료 시 활성화할 도시 오브젝트
    public GameObject[] relatedObjects; // 슬롯 완료 시 삭제할 관련 오브젝트
}

public class SlotManager : MonoBehaviour
{
    public SlotData[] slots;          // 모든 슬롯 데이터 배열
    public GameObject[] designPrefabs; // 도안 오브젝트 배열 (UI에 표시)
    public GameUIController gameUIController; // GameUIController 참조

    private int currentSlotIndex = 0; // 현재 진행 중인 슬롯 인덱스
    private GameObject activeSlot;    // 현재 활성화된 슬롯
    private GameObject activeDesign;  // 현재 활성화된 도안 오브젝트
    private GridSlot[] activeGridSlots; // 현재 활성화된 슬롯들의 GridSlot 배열

    private bool isMissionComplete = false;

    void Awake()
    {
        // 게임 시작 전 모든 슬롯과 도안 비활성화
        DeactivateAllSlotsAndDesigns();
    }

    public void StartGame()
    {
        // 게임 시작 시 첫 번째 슬롯과 도안 초기화
        currentSlotIndex = 0;
        isMissionComplete = false;
        InitializeCurrentSlot();
    }

    private void InitializeCurrentSlot()
    {
        // 모든 슬롯 비활성화
        foreach (SlotData slotData in slots)
        {
            if (slotData.slotPrefab != null)
                slotData.slotPrefab.SetActive(false);
        }

        // 모든 도안 비활성화
        foreach (GameObject design in designPrefabs)
        {
            if (design != null)
                design.SetActive(false);
        }

        // 현재 슬롯 활성화
        if (currentSlotIndex < slots.Length)
        {
            activeSlot = slots[currentSlotIndex].slotPrefab;
            if (activeSlot != null)
            {
                activeSlot.SetActive(true);
                activeGridSlots = activeSlot.GetComponentsInChildren<GridSlot>();
                Debug.Log($"현재 슬롯 {activeSlot.name} 활성화됨");
            }

            activeDesign = designPrefabs[currentSlotIndex];
            if (activeDesign != null)
            {
                activeDesign.SetActive(true);
                Debug.Log($"현재 도안 {activeDesign.name} 활성화됨");
            }
        }
        else
        {
            Debug.LogWarning("슬롯 인덱스가 범위를 초과했습니다!");
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
            Debug.Log($"{obj.name} 삭제됨");
        }

        foreach (GameObject obj in slots[currentSlotIndex].cityObjects)
        {
            obj.SetActive(true);
            Debug.Log($"{obj.name} 활성화됨!");
        }

        Debug.Log($"미션 성공: {activeSlot.name}");

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
            Debug.Log("모든 미션 완료!");
            if (gameUIController != null)
            {
                gameUIController.ShowGameClearUI(); // 게임 클리어 UI 표시
            }
        }
    }

    void Update()
    {
        if (!isMissionComplete && CheckMissionCompletion())
        {
            Debug.Log("미션 완료 조건 충족!");
            CompleteMission();
        }
    }
}
