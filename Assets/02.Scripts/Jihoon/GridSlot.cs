using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public string requiredTag; // �䱸�Ǵ� �±�
    private GameObject currentObject; // ���� ���Կ� ��ġ�� ��
    public ActiveState activeState; // ���� ���¸� Ȯ��
    public SoundManager soundManager; // SoundManager ����

    private void OnTriggerEnter(Collider other)
    {
        if (currentObject == null && other.CompareTag(requiredTag) && !activeState.isGrabbed)
        {
            // ���Կ� ������Ʈ ��ġ
            currentObject = other.gameObject;
            SnapObjectToSlot(other.transform);
            Debug.Log($"���� {name}�� {other.name} ��ġ �Ϸ�");

            // ���� ���
            if (soundManager != null)
            {
                soundManager.PlaySlotInsertSound();
            }
        }
    }

    private void SnapObjectToSlot(Transform objTransform)
    {
        // ���� ��ġ�� ����
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
            Destroy(currentObject); // ���Կ� ��ġ�� �� ����
            currentObject = null;
        }
    }
}
