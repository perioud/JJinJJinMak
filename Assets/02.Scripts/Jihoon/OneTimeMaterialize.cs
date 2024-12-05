using System.Collections;
using UnityEngine;

namespace INab.Dissolve
{
    [RequireComponent(typeof(Dissolver))]
    public class OneTimeMaterialize : MonoBehaviour
    {
        public float startDelay = 2f; // Materialize 실행 전 대기 시간
        private bool hasMaterialized = false; // Materialize 실행 여부 추적 변수
        private Dissolver dissolver; // Dissolver 컴포넌트 참조

        private void Start()
        {
            // Dissolver 컴포넌트 가져오기
            dissolver = GetComponent<Dissolver>();

            // 코루틴 실행
            StartCoroutine(StartMaterialize());
        }

        private IEnumerator StartMaterialize()
        {
            // 설정된 startDelay 만큼 대기
            yield return new WaitForSeconds(startDelay);

            // Materialize가 아직 실행되지 않았다면 실행
            if (!hasMaterialized)
            {
                dissolver.Materialize(); // Materialize 실행
                hasMaterialized = true; // 실행 여부 업데이트
            }

            // 이후에는 아무 작업도 하지 않음
        }
    }
}
