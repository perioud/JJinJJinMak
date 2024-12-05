using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class XylophoneGame : MonoBehaviour
{
    // 성공 시 재생할 오디오 소스 배열
    [SerializeField] private AudioSource[] successAudios;
    // 성공 시 활성화할 게임 오브젝트 리스트
    [SerializeField] private List<GameObject> objectsToActivate;
    // 실패 시 재생할 오디오 소스
    public AudioSource failureAudio;
    // 각 음계에 해당하는 오디오 소스 리스트
    public List<AudioSource> notes;
    // 색깔별로 시각적 피드백을 주기 위한 원형 오브젝트 리스트
    public List<GameObject> circles;
    // 성공 시 표시할 UI
    public GameObject successUI;
    public GameObject startUI;
    // 실패 시 표시할 UI
    public GameObject failUI;

    //토끼오브젝트
    [SerializeField] private List<GameObject> Rabbit;

    // 성공한 곡 수
    private int successCount = 0;
    // 남은 곡 수
    private int remainingCount = 6;

    // 곡의 정답 순서 (색깔의 배열로 정의)
    private readonly Dictionary<int, List<string>> correctOrders = new()
    {
        { 0, new() { "Yellow", "Orange", "Red", "Orange", "Yellow", "Yellow", "Yellow" } },
        { 1, new() { "Red", "Orange", "Red", "Orange", "Green", "Blue", "Green", "Blue" } },
        { 2, new() { "Blue", "Blue", "Yellow", "Green", "Blue", "Purple", "Purple", "Blue" } },
        { 3, new() { "Blue", "Yellow", "Yellow", "Green", "Orange", "Orange" } },
        { 4, new() { "Red", "Green", "Blue", "Purple", "Blue", "Green", "Blue" } },
        { 5, new() { "Red", "Orange", "Yellow", "Green", "Blue", "Blue", "Purple", "Purple", "Blue" } }
    };

    // 이미 재생된 곡의 인덱스 저장
    private readonly List<int> playedSongIndices = new();
    // 플레이어가 입력한 순서를 저장
    private List<string> playerOrder = new();
    // 현재 게임의 정답 순서
    public List<string> correctOrder;
    // 현재 선택된 곡의 인덱스
    private int selectedSongIndex;
    // 현재 시퀀스 실행 여부
    public bool isSequenceRunning = false;

    // 색상과 음계를 매핑하는 딕셔너리
    private Dictionary<string, AudioSource> noteMap;

    void Start()
    {
        // 초기화: 원형 오브젝트 비활성화
        circles.ForEach(c => c.SetActive(false));

        // 색상과 오디오 소스를 매핑
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

    // 랜덤하게 곡을 선택하는 메서드
    public void SelectRandomSong()
    {
        // 아직 재생되지 않은 곡의 인덱스 리스트를 가져옴
        var availableIndices = correctOrders.Keys.Except(playedSongIndices).ToList();

        // 모든 곡이 재생되었다면 리스트를 초기화
        //if (!availableIndices.Any())
        //{
        //    playedSongIndices.Clear();
        //    availableIndices = correctOrders.Keys.ToList();
        //    Debug.Log("모든 곡이 재생되었습니다. 초기화합니다.");
        //}

        // 랜덤하게 곡을 선택
        selectedSongIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        correctOrder = correctOrders[selectedSongIndex];
        playedSongIndices.Add(selectedSongIndex);

        Debug.Log($"선택된 곡: {selectedSongIndex}");
    }

    // 성공 오디오 재생
    public void PlaySuccessAudio()
    {
        if (selectedSongIndex < successAudios.Length)
        {
            successAudios[selectedSongIndex].Play();
            Debug.Log($"성공 오디오: {successAudios[selectedSongIndex].name}");
        }
        else
        {
            Debug.LogError("잘못된 인덱스: 성공 오디오를 찾을 수 없습니다.");
        }
    }

    // 정답 시퀀스를 실행하는 코루틴
    public IEnumerator StartCircleSequence()
    {
        isSequenceRunning = true;

        // 정답 순서에 따라 원형을 활성화하고 음계 재생
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

    // 플레이어가 색상을 연주할 때 호출
    public void PlayXylophone(string color)
    {
        // 플레이어 입력을 기록
        playerOrder.Add(color);
        PlayNoteByColor(color);

        // 정답과 비교
        if (!playerOrder.SequenceEqual(correctOrder.Take(playerOrder.Count)))
        {
            // 틀렸을 경우
            ShowFailUI();
        }
        else if (playerOrder.Count == correctOrder.Count)
        {
            // 정답을 모두 맞췄을 경우
            ShowSuccessUI();
        }
    }

    // 실패 UI 표시 및 초기화
    private void ShowFailUI()
    {
        failUI.SetActive(true);
        failureAudio.Play();
        Invoke(nameof(HideFailUI), 2f);
        playerOrder.Clear();
        Debug.Log("틀렸습니다!");
    }

    // 성공 UI 표시 및 초기화
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
        Debug.Log("성공!");
    }

    // UI 업데이트 (남은 곡 수, 성공 곡 수)
    private void UpdateUI()
    {
        // 성공 횟수에 따라 게임 오브젝트 활성화
        if (successCount <= objectsToActivate.Count)
        {
            var obj = objectsToActivate[successCount - 1]; // 성공 횟수는 1부터 시작
            if (obj != null && !obj.activeSelf) // 이미 활성화된 오브젝트는 건너뜀
            {
                obj.SetActive(true);
                Debug.Log($"{obj.name} 활성화됨.");
            }
        }
        // 토끼 오브젝트 활성화
        if (successCount <= Rabbit.Count)
        {
            var rabbit = Rabbit[successCount - 1]; // 성공 횟수에 해당하는 토끼
            if (rabbit != null && !rabbit.activeSelf) // 이미 활성화된 토끼는 건너뜀
            {
                rabbit.SetActive(true);
                Debug.Log($"{rabbit.name} 활성화됨.");
            }
        }
    }

    // 실패 UI 숨기기
    private void HideFailUI() => failUI.SetActive(false);

    // 성공 UI 숨기기
    private void HideSuccessUI() => successUI.SetActive(false);

    // 색상에 맞는 음계를 재생
    private void PlayNoteByColor(string color)
    {
        if (noteMap.TryGetValue(color, out var note))
        {
            note.Play();
        }
    }
}
