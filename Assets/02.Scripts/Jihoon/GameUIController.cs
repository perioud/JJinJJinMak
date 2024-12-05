using UnityEngine;
using UnityEngine.SceneManagement; // �� ������ ���� �߰�

public class GameUIController : MonoBehaviour
{
    [Header("Managers")]
    public SlotManager slotManager; // SlotManager�� ����
    public SoundManager soundManager; // SoundManager�� ����

    [Header("UI Panels")]
    public GameObject mainMenuPanel;   // ���� �޴� UI
    public GameObject inGamePanel;    // �ΰ��� UI
    public GameObject gameClearPanel; // ���� Ŭ���� UI

    private void Start()
    {
        // ���� ���� �� �⺻ UI ���� ����
        ShowMainMenu();
    }

    public void StartGame()
    {
        // ��ư Ŭ�� ����
        if (soundManager != null)
        {
            soundManager.PlayButtonClickSound();
        }

        // ���� �޴� ��Ȱ��ȭ �� �ΰ��� �г� Ȱ��ȭ
        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(true);
        gameClearPanel.SetActive(false);

        // SlotManager���� ���� ���� �ʱ�ȭ
        if (slotManager != null)
        {
            slotManager.StartGame();
        }

        Debug.Log("���� ����!");
    }

    public void ShowMainMenu()
    {
        // ��� �г� �ʱ�ȭ �� ���� �޴� Ȱ��ȭ
        mainMenuPanel.SetActive(true);
        inGamePanel.SetActive(false);
        gameClearPanel.SetActive(false);

        Debug.Log("���� �޴��� �̵�");
    }

    public void ShowGameClearUI()
    {
        // ���� Ŭ���� �г� Ȱ��ȭ
        mainMenuPanel.SetActive(false);
        inGamePanel.SetActive(false);
        gameClearPanel.SetActive(true);

        // �̼� �Ϸ� ����
        if (soundManager != null)
        {
            soundManager.PlayMissionCompleteSound();
        }

        Debug.Log("���� Ŭ���� ȭ�� ǥ��");
    }

    public void RestartGame()
    {
        // ��ư Ŭ�� ����
        if (soundManager != null)
        {
            soundManager.PlayButtonClickSound();
        }

        // ���� �� �ٽ� �ε�
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

        Debug.Log("������ ����۵Ǿ����ϴ�!");
    }
}
