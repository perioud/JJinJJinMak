using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitMove : MonoBehaviour
{
    // �̵� �ӵ��� ������ �� �ִ� ����
    public float speed = 1f;

    void Update()
    {
        // ������Ʈ�� ������ �̵�
        // Vector3.forward�� (0, 0, 1) ������ �ǹ��ϸ�, Transform�� ���⿡ ���� ����˴ϴ�.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
