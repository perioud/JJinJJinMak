using UnityEngine;

public class TrainMovement : MonoBehaviour
{
    public Transform[] waypoints; // 기차가 이동할 경로
    public float speed = 5.0f; // 이동 속도
    private int currentWaypointIndex = 0; // 현재 목표 지점

    private bool isMoving = false; // 이동 중인지 여부

    public void StartMoving()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("기차 이동 경로가 설정되지 않았습니다!");
            return;
        }

        isMoving = true; // 이동 시작
        MoveToNextWaypoint();
    }

    void Update()
    {
        if (!isMoving || waypoints.Length == 0)
        {
            return; // 이동 중이 아니면 리턴
        }

        // 현재 목표 지점으로 이동
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // 목표 지점에 도착했는지 확인
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // 다음 Waypoint로 이동
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length > 0)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            Debug.Log($"기차가 이동 중입니다. 목표 지점: {targetWaypoint.name}");
        }
    }
}
