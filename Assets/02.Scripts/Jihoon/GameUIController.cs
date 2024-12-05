using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리를 위해 추가

public class GameUIController : MonoBehaviour
{
    [Header("Managers")]
    public SlotManager slotManager; // SlotManager를 연결
    public SoundManager soundManager; // SoundManager를 연결

    [Header("UI Panels")]
    public GameObject mainMenuPanel;   // 메인 메뉴 UI
    public GameObject inGamePanel;    // 인게임 UI
    public GameObject gameClearPanel; // 게임 클리어 UI

    private void Start()
    {
        // 게임 시작 전 기본 UI 상태 설정
        ShowMainMenu();
    }

    public void StartGame()
    {
        // 버튼 클릭 사운드
        if (soundManager != null)
        {
            soundManager.PlayButtonClickSound();
        }

        // 메인 메뉴 비활성화 및 인게임 패널 활성화
        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
        gameClearPanel.SetActive(false);

        // SlotManager에서 게임 시작 초기화
        if (slotManager != null)
        {
            slotManager.StartGame();
        }

        Debug.Log("게임 시작!");
    }

    public void ShowMainMenu()
    {
        // 모든 패널 초기화 후 메인 메뉴 활성화
        mainMenuPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameClearPanel.SetActive(false);

        Debug.Log("메인 메뉴로 이동");
    }

    public void ShowGameClearUI()
    {
        // 게임 클리어 패널 활성화
        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(false);
        gameClearPanel.SetActive(true);

        // 미션 완료 사운드
        if (soundManager != null)
        {
            soundManager.PlayMissionCompleteSound();
        }

        Debug.Log("게임 클리어 화면 표시");
    }

    public void RestartGame()
    {
        // 버튼 클릭 사운드
        if (soundManager != null)
        {
            soundManager.PlayButtonClickSound();
        }

        // 현재 씬 다시 로드
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Debug.Log("게임이 재시작되었습니다!");
    }
}
