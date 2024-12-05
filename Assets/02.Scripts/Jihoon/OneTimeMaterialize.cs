using System.Collections;
using UnityEngine;

namespace INab.Dissolve
{
    [RequireComponent(typeof(Dissolver))]
    public class OneTimeMaterialize : MonoBehaviour
    {
        public float startDelay = 2f; // Materialize ���� �� ��� �ð�
        private bool hasMaterialized = false; // Materialize ���� ���� ���� ����
        private Dissolver dissolver; // Dissolver ������Ʈ ����

        private void Start()
        {
            // Dissolver ������Ʈ ��������
            dissolver = GetComponent<Dissolver>();

            // �ڷ�ƾ ����
            StartCoroutine(StartMaterialize());
        }

        private IEnumerator StartMaterialize()
        {
            // ������ startDelay ��ŭ ���
            yield return new WaitForSeconds(startDelay);

            // Materialize�� ���� ������� �ʾҴٸ� ����
            if (!hasMaterialized)
            {
                dissolver.Materialize(); // Materialize ����
                hasMaterialized = true; // ���� ���� ������Ʈ
            }

            // ���Ŀ��� �ƹ� �۾��� ���� ����
        }
    }
}
