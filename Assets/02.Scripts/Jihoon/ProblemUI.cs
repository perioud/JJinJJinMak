using UnityEngine;
using TMPro;

public class ProblemUI : MonoBehaviour
{
    public TMP_Text questionText;    // 문제를 표시할 TextMeshPro
    public TMP_Text[] answerButtons; // 답변을 표시할 TextMeshPro 배열
    public TMP_Text feedbackText;    // 피드백 메시지 텍스트

    private ProblemData currentProblem;

    // 문제를 화면에 표시하는 함수
    public void ShowProblem(ProblemData problem)
    {
        currentProblem = problem;
        questionText.text = problem.questionText;
        feedbackText.text = ""; // 피드백 메시지 초기화

        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].text = problem.answers[i];
        }

        gameObject.SetActive(true);
    }

    // 사용자가 답변을 선택했을 때 호출되는 함수
    public void OnAnswerSelected(int index)
    {
        if (index == currentProblem.correctAnswerIndex)
        {
            feedbackText.text = "정답입니다!";
            Debug.Log("정답입니다!");
            // 일정 시간 후 UI 패널을 숨깁니다.
            Invoke("HidePanel", 1.5f);
        }
        else
        {
            feedbackText.text = "오답입니다. 다시 시도해보세요!";
            Debug.Log("오답입니다.");
        }
    }

    // 문제 패널을 숨기는 함수
    private void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
