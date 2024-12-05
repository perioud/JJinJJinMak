using UnityEngine;
using TMPro;

public class ProblemUI : MonoBehaviour
{
    public TMP_Text questionText;    // ������ ǥ���� TextMeshPro
    public TMP_Text[] answerButtons; // �亯�� ǥ���� TextMeshPro �迭
    public TMP_Text feedbackText;    // �ǵ�� �޽��� �ؽ�Ʈ

    private ProblemData currentProblem;

    // ������ ȭ�鿡 ǥ���ϴ� �Լ�
    public void ShowProblem(ProblemData problem)
    {
        currentProblem = problem;
        questionText.text = problem.questionText;
        feedbackText.text = ""; // �ǵ�� �޽��� �ʱ�ȭ

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].text = problem.answers[i];
        }

        gameObject.SetActive(true);
    }

    // ����ڰ� �亯�� �������� �� ȣ��Ǵ� �Լ�
    public void OnAnswerSelected(int index)
    {
        if (index == currentProblem.correctAnswerIndex)
        {
            feedbackText.text = "�����Դϴ�!";
            Debug.Log("�����Դϴ�!");
            // ���� �ð� �� UI �г��� ����ϴ�.
            Invoke("HidePanel", 1.5f);
        }
        else
        {
            feedbackText.text = "�����Դϴ�. �ٽ� �õ��غ�����!";
            Debug.Log("�����Դϴ�.");
        }
    }

    // ���� �г��� ����� �Լ�
    private void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
