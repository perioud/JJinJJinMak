using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BlockCountManager : MonoBehaviour
{
    public BlockSpawner blockSpawner; // BlockSpawner 스크립트 참조
    public TextMeshProUGUI problemText; // 문제를 보여줄 TMP 텍스트
    public Button[] answerButtons; // 보기 선택 버튼 배열

    private int redBlockCount = 0;
    private int blueBlockCount = 0;
    private int totalBlockCount = 0;

    private string correctAnswer;

    private bool isCooldown = false; // 쿨다운 상태 변수
    public float cooldownTime = 1.0f; // 쿨다운 시간 설정 (1초)

    public TextMeshProUGUI wrongAnswerText; // 오답 메시지 텍스트 참조
    public float wrongAnswerDisplayTime = 1.5f; // 오답 메시지 표시 시간

    void Start()
    {
        // 딜레이를 두고 블록 개수를 셈
        StartCoroutine(DelayedCountBlocks());
    }

    IEnumerator DelayedCountBlocks()
    {
        // 블록이 모두 생성될 때까지 기다림 (0.5초 대기)
        yield return new WaitForSeconds(0.5f);

        // 블록 개수 세기 및 문제 생성
        CountBlocks();
        GenerateProblem();

        // 모든 버튼의 상호작용을 다시 활성화
        SetButtonsInteractable(true);
    }

    void CountBlocks()
    {
        GameObject[] redBlocks = GameObject.FindGameObjectsWithTag("Red");
        GameObject[] blueBlocks = GameObject.FindGameObjectsWithTag("Blue");

        redBlockCount = redBlocks.Length;
        blueBlockCount = blueBlocks.Length;
        totalBlockCount = redBlockCount + blueBlockCount;

        // 디버그 메시지 추가
        Debug.Log($"빨간 블록 개수: {redBlockCount}, 파란 블록 개수: {blueBlockCount}, 총 블록 개수: {totalBlockCount}");
    }

    void GenerateProblem()
    {
        int problemType = Random.Range(0, 3);
        int correctIndex = Random.Range(0, answerButtons.Length);

        switch (problemType)
        {
            case 0:
                correctAnswer = redBlockCount.ToString();
                // "빨간 블록의 개수는 몇 개입니까?"의 "빨간"을 파란색으로 표시
                problemText.text = "<color=#0000FF>빨간</color> 블록의 개수는 몇 개입니까?";
                Debug.Log("문제: 빨간 블록의 개수. 정답: " + correctAnswer);
                break;
            case 1:
                correctAnswer = blueBlockCount.ToString();
                // "파란 블록의 개수는 몇 개입니까?"의 "파란"을 빨간색으로 표시
                problemText.text = "<color=#FF0000>파란</color> 블록의 개수는 몇 개입니까?";
                Debug.Log("문제: 파란 블록의 개수. 정답: " + correctAnswer);
                break;
            case 2:
                correctAnswer = totalBlockCount.ToString();
                // "빨간색과 파란색 블록의 합은 몇 개입니까?"의 "빨간색"을 파란색, "파란색"을 빨간색으로 표시
                problemText.text = "<color=#0000FF>빨간색</color>과 <color=#FF0000>파란색</color> 블록의 합은 몇 개입니까?";
                Debug.Log("문제: 빨간색과 파란색 블록의 합. 정답: " + correctAnswer);
                break;
        }

        SetAnswerButtons(correctIndex, correctAnswer);
    }

    void SetAnswerButtons(int correctIndex, string correctAnswer)
    {
        answerButtons[correctIndex].GetComponentInChildren<TextMeshProUGUI>().text = correctAnswer;

        List<int> wrongAnswers = GenerateWrongAnswers(int.Parse(correctAnswer));
        int wrongAnswerIndex = 0;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i != correctIndex)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = wrongAnswers[wrongAnswerIndex].ToString();
                wrongAnswerIndex++;
            }

            int index = i;
            answerButtons[i].onClick.RemoveAllListeners(); // 기존의 리스너를 제거
            answerButtons[i].onClick.AddListener(() => CheckAnswer(index == correctIndex));
        }
    }

    List<int> GenerateWrongAnswers(int correctAnswer)
    {
        List<int> wrongAnswers = new List<int>();
        int minValue = Mathf.Max(0, correctAnswer - 3);
        int maxValue = correctAnswer + 3;

        while (wrongAnswers.Count < answerButtons.Length - 1)
        {
            int randomAnswer = Random.Range(minValue, maxValue + 1);
            if (randomAnswer != correctAnswer && !wrongAnswers.Contains(randomAnswer))
            {
                wrongAnswers.Add(randomAnswer);
            }
        }

        return wrongAnswers;
    }



    void CheckAnswer(bool isCorrect)
    {
        // 쿨다운 상태일 때는 동작하지 않음
        if (isCooldown)
            return;

        if (isCorrect)
        {
            Debug.Log("Correct Answer!");

            // 쿨다운 시작
            StartCoroutine(CooldownCoroutine());

            // 정답 처리 후 블록 초기화 및 새로운 문제 생성
            ClearBlocks();
            StartCoroutine(DelayedCountBlocks());
        }
        else
        {
            Debug.Log("Wrong Answer...");
            StartCoroutine(ShowWrongAnswerText()); // 오답 텍스트 표시 코루틴 호출
        }
    }

    IEnumerator ShowWrongAnswerText()
    {
        wrongAnswerText.enabled = true; // 오답 메시지 활성화
        yield return new WaitForSeconds(wrongAnswerDisplayTime); // 지정된 시간 동안 대기
        wrongAnswerText.enabled = false; // 오답 메시지 비활성화
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true; // 쿨다운 상태로 설정
        yield return new WaitForSeconds(cooldownTime); // 쿨다운 시간 대기
        isCooldown = false; // 쿨다운 해제
    }

    void SetButtonsInteractable(bool interactable)
    {
        foreach (Button button in answerButtons)
        {
            button.interactable = interactable;
        }
    }

    void ClearBlocks()
    {
        // 현재 존재하는 블록들을 모두 찾고 삭제
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Red");
        foreach (var block in allBlocks) Destroy(block);

        allBlocks = GameObject.FindGameObjectsWithTag("Blue");
        foreach (var block in allBlocks) Destroy(block);

        allBlocks = GameObject.FindGameObjectsWithTag("White");
        foreach (var block in allBlocks) Destroy(block);

        // 새로운 블록 생성
        blockSpawner.GenerateBlockStructure();
    }
}
