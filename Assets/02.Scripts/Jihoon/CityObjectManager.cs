using UnityEngine;

public class CityObjectManager : MonoBehaviour
{
    public GameObject[] cityObjects; // ���� ������Ʈ��

    public void ActivateCityObjects()
    {
        foreach (GameObject obj in cityObjects)
        {
            obj.SetActive(true);
            Debug.Log($"{obj.name} Ȱ��ȭ��!");
        }
    }
}
