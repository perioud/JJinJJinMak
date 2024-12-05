using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityRotation : MonoBehaviour
{
    public InputManager inputManager; // InputManager ����
    public Transform cityTransform; // �̴� ������ Transform
    public float rotationSpeed = 50.0f; // ȸ�� �ӵ� ����

    void Update()
    {
        // Ʈ���� ��ư�� ���� �ִ� ���ȿ��� ȸ�� ���
        if (inputManager.IsTriggerPressed())
        {
            // ��Ʈ�ѷ��� AngularVelocity�� ������
            Vector3 angularVelocity = inputManager.AngularVelocityInput();

            // Y�� ȸ���� ������� ���ø� �¿�� ȸ��
            float rotationAmount = angularVelocity.y * rotationSpeed * Time.deltaTime;
            cityTransform.Rotate(Vector3.up, rotationAmount);
        }
    }
}