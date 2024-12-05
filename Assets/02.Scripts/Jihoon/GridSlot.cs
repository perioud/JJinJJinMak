using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public string requiredTag; // 요구되는 태그
    private GameObject currentObject; // 현재 슬롯에 배치된 블럭
    public ActiveState activeState; // 블럭의 상태를 확인
    public SoundManager soundManager; // SoundManager 참조

    private void OnTriggerEnter(Collider other)
    {
        if (currentObject == null && other.CompareTag(requiredTag) && !activeState.isGrabbed)
        {
            // 슬롯에 오브젝트 배치
            currentObject = other.gameObject;
            SnapObjectToSlot(other.transform);
            Debug.Log($"슬롯 {name}에 {other.name} 배치 완료");

            // 사운드 재생
            if (soundManager != null)
            {
                soundManager.PlaySlotInsertSound();
            }
        }
    }

    private void SnapObjectToSlot(Transform objTransform)
    {
        // 슬롯 위치로 스냅
        objTransform.position = transform.position;
        objTransform.rotation = transform.rotation;

        Rigidbody rb = objTransform.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    public bool IsCorrectlyOccupied()
    {
        return currentObject != null && currentObject.CompareTag(requiredTag);
    }

    public void ClearSlot()
    {
        if (currentObject != null)
        {
            Destroy(currentObject); // 슬롯에 배치된 블럭 삭제
            currentObject = null;
        }
    }
}
