using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class XylophoneGame : MonoBehaviour
{
    // ���� �� ����� ����� �ҽ� �迭
    [SerializeField] private AudioSource[] successAudios;
    // ���� �� Ȱ��ȭ�� ���� ������Ʈ ����Ʈ
    [SerializeField] private List<GameObject> objectsToActivate;
    // ���� �� ����� ����� �ҽ�
    public AudioSource failureAudio;
    // �� ���迡 �ش��ϴ� ����� �ҽ� ����Ʈ
    public List<AudioSource> notes;
    // ���򺰷� �ð��� �ǵ���� �ֱ� ���� ���� ������Ʈ ����Ʈ
    public List<GameObject> circles;
    // ���� �� ǥ���� UI
    public GameObject successUI;
    public GameObject startUI;
    // ���� �� ǥ���� UI
    public GameObject failUI;

    //�䳢������Ʈ
    [SerializeField] private List<GameObject> Rabbit;

    // ������ �� ��
    private int successCount = 0;
    // ���� �� ��
    private int remainingCount = 6;

    // ���� ���� ���� (������ �迭�� ����)
    private readonly Dictionary<int, List<string>> correctOrders = new()
    {
        { 0, new() { "Yellow", "Orange", "Red", "Orange", "Yellow", "Yellow", "Yellow" } },
        { 1, new() { "Red", "Orange", "Red", "Orange", "Green", "Blue", "Green", "Blue" } },
        { 2, new() { "Blue", "Blue", "Yellow", "Green", "Blue", "Purple", "Purple", "Blue" } },
        { 3, new() { "Blue", "Yellow", "Yellow", "Green", "Orange", "Orange" } },
        { 4, new() { "Red", "Green", "Blue", "Purple", "Blue", "Green", "Blue" } },
        { 5, new() { "Red", "Orange", "Yellow", "Green", "Blue", "Blue", "Purple", "Purple", "Blue" } }
    };

    // �̹� ����� ���� �ε��� ����
    private readonly List<int> playedSongIndices = new();
    // �÷��̾ �Է��� ������ ����
    private List<string> playerOrder = new();
    // ���� ������ ���� ����
    public List<string> correctOrder;
    // ���� ���õ� ���� �ε���
    private int selectedSongIndex;
    // ���� ������ ���� ����
    public bool isSequenceRunning = false;

    // ����� ���踦 �����ϴ� ��ųʸ�
    private Dictionary<string, AudioSource> noteMap;

    void Start()
    {
        // �ʱ�ȭ: ���� ������Ʈ ��Ȱ��ȭ
        circles.ForEach(c => c.SetActive(false));

        // ����� ����� �ҽ��� ����
        noteMap = new()
        {
            { "Red", notes[0] },
            { "Orange", notes[1] },
            { "Yellow", notes[2] },
            { "Green", notes[3] },
            { "Blue", notes[4] },
            { "Purple", notes[5] }
        };
    }

    // �����ϰ� ���� �����ϴ� �޼���
    public void SelectRandomSong()
    {
        // ���� ������� ���� ���� �ε��� ����Ʈ�� ������
        var availableIndices = correctOrders.Keys.Except(playedSongIndices).ToList();

        // ��� ���� ����Ǿ��ٸ� ����Ʈ�� �ʱ�ȭ
        //if (!availableIndices.Any())
        //{
        //    playedSongIndices.Clear();
        //    availableIndices = correctOrders.Keys.ToList();
        //    Debug.Log("��� ���� ����Ǿ����ϴ�. �ʱ�ȭ�մϴ�.");
        //}

        // �����ϰ� ���� ����
        selectedSongIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        correctOrder = correctOrders[selectedSongIndex];
        playedSongIndices.Add(selectedSongIndex);

        Debug.Log($"���õ� ��: {selectedSongIndex}");
    }

    // ���� ����� ���
    public void PlaySuccessAudio()
    {
        if (selectedSongIndex < successAudios.Length)
        {
            successAudios[selectedSongIndex].Play();
            Debug.Log($"���� �����: {successAudios[selectedSongIndex].name}");
        }
        else
        {
            Debug.LogError("�߸��� �ε���: ���� ������� ã�� �� �����ϴ�.");
        }
    }

    // ���� �������� �����ϴ� �ڷ�ƾ
    public IEnumerator StartCircleSequence()
    {
        isSequenceRunning = true;

        // ���� ������ ���� ������ Ȱ��ȭ�ϰ� ���� ���
        foreach (var color in correctOrder)
        {
            var circle = circles.FirstOrDefault(c => c.name == color);
           
            if (circle != null)
            {
                circle.SetActive(true);
                PlayNoteByColor(color);
                yield return new WaitForSeconds(0.3f);
                circle.SetActive(false);
                yield return new WaitForSeconds(0.3f);
            }
        }

        isSequenceRunning = false;
    }

    // �÷��̾ ������ ������ �� ȣ��
    public void PlayXylophone(string color)
    {
        // �÷��̾� �Է��� ���
        playerOrder.Add(color);
        PlayNoteByColor(color);

        // ����� ��
        if (!playerOrder.SequenceEqual(correctOrder.Take(playerOrder.Count)))
        {
            // Ʋ���� ���
            ShowFailUI();
        }
        else if (playerOrder.Count == correctOrder.Count)
        {
            // ������ ��� ������ ���
            ShowSuccessUI();
        }
    }

    // ���� UI ǥ�� �� �ʱ�ȭ
    private void ShowFailUI()
    {
        failUI.SetActive(true);
        failureAudio.Play();
        Invoke(nameof(HideFailUI), 2f);
        playerOrder.Clear();
        Debug.Log("Ʋ�Ƚ��ϴ�!");
    }

    // ���� UI ǥ�� �� �ʱ�ȭ
    private void ShowSuccessUI()
    {
        successUI.SetActive(true);
        Invoke(nameof(HideSuccessUI), 2f);
        Invoke(nameof(PlaySuccessAudio), 1.5f);
        startUI.SetActive(true);
        remainingCount--;
        successCount++;
        UpdateUI();
        playerOrder.Clear();
        Debug.Log("����!");
    }

    // UI ������Ʈ (���� �� ��, ���� �� ��)
    private void UpdateUI()
    {
        // ���� Ƚ���� ���� ���� ������Ʈ Ȱ��ȭ
        if (successCount <= objectsToActivate.Count)
        {
            var obj = objectsToActivate[successCount - 1]; // ���� Ƚ���� 1���� ����
            if (obj != null && !obj.activeSelf) // �̹� Ȱ��ȭ�� ������Ʈ�� �ǳʶ�
            {
                obj.SetActive(true);
                Debug.Log($"{obj.name} Ȱ��ȭ��.");
            }
        }
        // �䳢 ������Ʈ Ȱ��ȭ
        if (successCount <= Rabbit.Count)
        {
            var rabbit = Rabbit[successCount - 1]; // ���� Ƚ���� �ش��ϴ� �䳢
            if (rabbit != null && !rabbit.activeSelf) // �̹� Ȱ��ȭ�� �䳢�� �ǳʶ�
            {
                rabbit.SetActive(true);
                Debug.Log($"{rabbit.name} Ȱ��ȭ��.");
            }
        }
    }

    // ���� UI �����
    private void HideFailUI() => failUI.SetActive(false);

    // ���� UI �����
    private void HideSuccessUI() => successUI.SetActive(false);

    // ���� �´� ���踦 ���
    private void PlayNoteByColor(string color)
    {
        if (noteMap.TryGetValue(color, out var note))
        {
            note.Play();
        }
    }
}
