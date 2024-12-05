using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BlockCountManager : MonoBehaviour
{
    public BlockSpawner blockSpawner; // BlockSpawner ��ũ��Ʈ ����
    public TextMeshProUGUI problemText; // ������ ������ TMP �ؽ�Ʈ
    public Button[] answerButtons; // ���� ���� ��ư �迭

    private int redBlockCount = 0;
    private int blueBlockCount = 0;
    private int totalBlockCount = 0;

    private string correctAnswer;

    private bool isCooldown = false; // ��ٿ� ���� ����
    public float cooldownTime = 1.0f; // ��ٿ� �ð� ���� (1��)

    public TextMeshProUGUI wrongAnswerText; // ���� �޽��� �ؽ�Ʈ ����
    public float wrongAnswerDisplayTime = 1.5f; // ���� �޽��� ǥ�� �ð�

    void Start()
    {
        // �����̸� �ΰ� ��� ������ ��
        StartCoroutine(DelayedCountBlocks());
    }

    IEnumerator DelayedCountBlocks()
    {
        // ����� ��� ������ ������ ��ٸ� (0.5�� ���)
        yield return new WaitForSeconds(0.5f);

        // ��� ���� ���� �� ���� ����
        CountBlocks();
        GenerateProblem();

        // ��� ��ư�� ��ȣ�ۿ��� �ٽ� Ȱ��ȭ
        SetButtonsInteractable(true);
    }

    void CountBlocks()
    {
        GameObject[] redBlocks = GameObject.FindGameObjectsWithTag("Red");
        GameObject[] blueBlocks = GameObject.FindGameObjectsWithTag("Blue");

        redBlockCount = redBlocks.Length;
        blueBlockCount = blueBlocks.Length;
        totalBlockCount = redBlockCount + blueBlockCount;

        // ����� �޽��� �߰�
        Debug.Log($"���� ��� ����: {redBlockCount}, �Ķ� ��� ����: {blueBlockCount}, �� ��� ����: {totalBlockCount}");
    }

    void GenerateProblem()
    {
        int problemType = Random.Range(0, 3);
        int correctIndex = Random.Range(0, answerButtons.Length);

        switch (problemType)
        {
            case 0:
                correctAnswer = redBlockCount.ToString();
                // "���� ����� ������ �� ���Դϱ�?"�� "����"�� �Ķ������� ǥ��
                problemText.text = "<color=#0000FF>����</color> ����� ������ �� ���Դϱ�?";
                Debug.Log("����: ���� ����� ����. ����: " + correctAnswer);
                break;
            case 1:
                correctAnswer = blueBlockCount.ToString();
                // "�Ķ� ����� ������ �� ���Դϱ�?"�� "�Ķ�"�� ���������� ǥ��
                problemText.text = "<color=#FF0000>�Ķ�</color> ����� ������ �� ���Դϱ�?";
                Debug.Log("����: �Ķ� ����� ����. ����: " + correctAnswer);
                break;
            case 2:
                correctAnswer = totalBlockCount.ToString();
                // "�������� �Ķ��� ����� ���� �� ���Դϱ�?"�� "������"�� �Ķ���, "�Ķ���"�� ���������� ǥ��
                problemText.text = "<color=#0000FF>������</color>�� <color=#FF0000>�Ķ���</color> ����� ���� �� ���Դϱ�?";
                Debug.Log("����: �������� �Ķ��� ����� ��. ����: " + correctAnswer);
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
            answerButtons[i].onClick.RemoveAllListeners(); // ������ �����ʸ� ����
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
        // ��ٿ� ������ ���� �������� ����
        if (isCooldown)
            return;

        if (isCorrect)
        {
            Debug.Log("Correct Answer!");

            // ��ٿ� ����
            StartCoroutine(CooldownCoroutine());

            // ���� ó�� �� ��� �ʱ�ȭ �� ���ο� ���� ����
            ClearBlocks();
            StartCoroutine(DelayedCountBlocks());
        }
        else
        {
            Debug.Log("Wrong Answer...");
            StartCoroutine(ShowWrongAnswerText()); // ���� �ؽ�Ʈ ǥ�� �ڷ�ƾ ȣ��
        }
    }

    IEnumerator ShowWrongAnswerText()
    {
        wrongAnswerText.enabled = true; // ���� �޽��� Ȱ��ȭ
        yield return new WaitForSeconds(wrongAnswerDisplayTime); // ������ �ð� ���� ���
        wrongAnswerText.enabled = false; // ���� �޽��� ��Ȱ��ȭ
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true; // ��ٿ� ���·� ����
        yield return new WaitForSeconds(cooldownTime); // ��ٿ� �ð� ���
        isCooldown = false; // ��ٿ� ����
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
        // ���� �����ϴ� ��ϵ��� ��� ã�� ����
        GameObject[] allBlocks = GameObject.FindGameObjectsWithTag("Red");
        foreach (var block in allBlocks) Destroy(block);

        allBlocks = GameObject.FindGameObjectsWithTag("Blue");
        foreach (var block in allBlocks) Destroy(block);

        allBlocks = GameObject.FindGameObjectsWithTag("White");
        foreach (var block in allBlocks) Destroy(block);

        // ���ο� ��� ����
        blockSpawner.GenerateBlockStructure();
    }
}
