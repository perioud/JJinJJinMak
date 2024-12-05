using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitMove : MonoBehaviour
{
    // 이동 속도를 설정할 수 있는 변수
    public float speed = 1f;

    void Update()
    {
        // 오브젝트를 앞으로 이동
        // Vector3.forward는 (0, 0, 1) 방향을 의미하며, Transform의 방향에 따라 변경됩니다.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
