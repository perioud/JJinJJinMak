using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRotation : MonoBehaviour
{
    public InputManager inputManager; // InputManager 참조
    public Transform cityTransform; // 미니 도시의 Transform
    public float rotationSpeed = 50.0f; // 회전 속도 조정

    void Update()
    {
        // 트리거 버튼이 눌려 있는 동안에만 회전 허용
        if (inputManager.IsTriggerPressed())
        {
            // 컨트롤러의 AngularVelocity를 가져옴
            Vector3 angularVelocity = inputManager.AngularVelocityInput();

            // Y축 회전을 기반으로 도시를 좌우로 회전
            float rotationAmount = angularVelocity.y * rotationSpeed * Time.deltaTime;
            cityTransform.Rotate(Vector3.up, rotationAmount);
        }
    }
}