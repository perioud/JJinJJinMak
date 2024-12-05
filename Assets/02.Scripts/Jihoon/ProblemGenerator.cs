using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ProblemGenerator : MonoBehaviour
{
    public ProblemUI problemUIPanel;
    private List<BlockColor> spawnedBlocks;

    // 스포너에서 생성된 모든 블록을 수집합니다.
    public void CollectSpawnedBlocks()
    {
        spawnedBlocks = FindObjectsOfType<BlockColor>().ToList();
    }

    // 문제를 자동으로 생성합니다.
    public void GenerateProblem()
    {
        if (spawnedBlocks == null || spawnedBlocks.Count == 0)
        {
            Debug.LogError("생성된 블록이 없습니다.");
            return;
        }

        // 랜덤으로 3가지 문제 중 하나를 선택하여 생성합니다.
        int problemType = UnityEngine.Random.Range(0, 3);
        ProblemData problem = new ProblemData();

        switch (problemType)
        {
            case 0:
                // 빨간색 블럭의 개수 문제
                int redCount = spawnedBlocks.Count(block => block.blockColor == "Red");
                problem.questionText = "빨간색 블록은 몇 개일까요?";
                problem.answers = GenerateRandomAnswers(redCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, redCount);
                break;

            case 1:
                // 파란색 블럭의 개수 문제
                int blueCount = spawnedBlocks.Count(block => block.blockColor == "Blue");
                problem.questionText = "파란색 블록은 몇 개일까요?";
                problem.answers = GenerateRandomAnswers(blueCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, blueCount);
                break;

            case 2:
                // 빨간색 + 파란색 블럭의 개수 문제
                int redBlueCount = spawnedBlocks.Count(block => block.blockColor == "Red") +
                                   spawnedBlocks.Count(block => block.blockColor == "Blue");
                problem.questionText = "빨간색과 파란색 블록의 합은 몇 개일까요?";
                problem.answers = GenerateRandomAnswers(redBlueCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, redBlueCount);
                break;
        }

        problemUIPanel.ShowProblem(problem);
    }

    private string[] GenerateRandomAnswers(int correctAnswer)
    {
        HashSet<int> answers = new HashSet<int> { correctAnswer };

        // 오답 생성
        while (answers.Count < 3)
        {
            int randomAnswer = UnityEngine.Random.Range(correctAnswer - 2, correctAnswer + 3);
            if (randomAnswer > 0) // 0 이하의 값은 피함
            {
                answers.Add(randomAnswer);
            }
        }

        return answers.OrderBy(a => a).Select(a => a.ToString()).ToArray();
    }

    private int GetCorrectAnswerIndex(string[] answers, int correctAnswer)
    {
        return Array.IndexOf(answers, correctAnswer.ToString());
    }
}
