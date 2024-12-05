using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public Transform[] waypoints; // ������ �̵��� ���
    public float speed = 5.0f; // �̵� �ӵ�
    private int currentWaypointIndex = 0; // ���� ��ǥ ����

    private bool isMoving = false; // �̵� ������ ����

    public void StartMoving()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("���� �̵� ��ΰ� �������� �ʾҽ��ϴ�!");
            return;
        }

        isMoving = true; // �̵� ����
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0)
        {
            return; // �̵� ���� �ƴϸ� ����
        }

        // ���� ��ǥ �������� �̵�
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // ��ǥ ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // ���� Waypoint�� �̵�
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Debug.Log($"������ �̵� ���Դϴ�. ��ǥ ����: {targetWaypoint.name}");
        }
    }
}
