using UnityEngine;

public class CityObjectManager : MonoBehaviour
{
    public GameObject[] cityObjects; // 도시 오브젝트들

    public void ActivateCityObjects()
    {
        foreach (GameObject obj in cityObjects)
        {
            obj.SetActive(true);
            Debug.Log($"{obj.name} 활성화됨!");
        }
    }
}
