using UnityEngine;

[System.Serializable]
public class ProblemData
{
    public string questionText;  // 문제 텍스트
    public string[] answers;     // 답변 리스트
    public int correctAnswerIndex;  // 정답 인덱스
}
