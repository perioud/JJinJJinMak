using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class ProblemGenerator : MonoBehaviour
{
    public ProblemUI problemUIPanel;
    private List<BlockColor> spawnedBlocks;

    // �����ʿ��� ������ ��� ����� �����մϴ�.
    public void CollectSpawnedBlocks()
    {
        spawnedBlocks = FindObjectsOfType<BlockColor>().ToList();
    }

    // ������ �ڵ����� �����մϴ�.
    public void GenerateProblem()
    {
        if (spawnedBlocks == null || spawnedBlocks.Count == 0)
        {
            Debug.LogError("������ ����� �����ϴ�.");
            return;
        }

        // �������� 3���� ���� �� �ϳ��� �����Ͽ� �����մϴ�.
        int problemType = UnityEngine.Random.Range(0, 3);
        ProblemData problem = new ProblemData();

        switch (problemType)
        {
            case 0:
                // ������ ���� ���� ����
                int redCount = spawnedBlocks.Count(block => block.blockColor == "Red");
                problem.questionText = "������ ����� �� ���ϱ��?";
                problem.answers = GenerateRandomAnswers(redCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, redCount);
                break;

            case 1:
                // �Ķ��� ���� ���� ����
                int blueCount = spawnedBlocks.Count(block => block.blockColor == "Blue");
                problem.questionText = "�Ķ��� ����� �� ���ϱ��?";
                problem.answers = GenerateRandomAnswers(blueCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, blueCount);
                break;

            case 2:
                // ������ + �Ķ��� ���� ���� ����
                int redBlueCount = spawnedBlocks.Count(block => block.blockColor == "Red") +
                                   spawnedBlocks.Count(block => block.blockColor == "Blue");
                problem.questionText = "�������� �Ķ��� ����� ���� �� ���ϱ��?";
                problem.answers = GenerateRandomAnswers(redBlueCount);
                problem.correctAnswerIndex = GetCorrectAnswerIndex(problem.answers, redBlueCount);
                break;
        }

        problemUIPanel.ShowProblem(problem);
    }

    private string[] GenerateRandomAnswers(int correctAnswer)
    {
        HashSet<int> answers = new HashSet<int> { correctAnswer };

        // ���� ����
        while (answers.Count < 3)
        {
            int randomAnswer = UnityEngine.Random.Range(correctAnswer - 2, correctAnswer + 3);
            if (randomAnswer > 0) // 0 ������ ���� ����
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
