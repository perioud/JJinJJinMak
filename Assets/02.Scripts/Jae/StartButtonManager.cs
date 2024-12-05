using System.Collections;
using UnityEngine;

public class StartButtonManager : MonoBehaviour
{
    public GameObject startUI;         // ���� UI
    public GameObject nextUI;          // ���� ���� UI
    public CardMatchingGame cardGame;  // ī�� ��Ī ���� ��ũ��Ʈ
    public LayerMask uiLayer;          // UI ���̾� ����ũ
    public InputManager xrinput;       // VR �Է� �ý���
    private RaycastHit hit;
    public FlipCard Flip;
    private bool isGameStarted = false;

    private void Update()
    {
        if (isGameStarted) return;

        // ����ĳ��Ʈ�� ����Ͽ� UI ������Ʈ�� ��ȣ�ۿ�
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, uiLayer))  // UI ���̾� ����ũ�� �߰�
        {
            if (xrinput.IsTriggerPressed() && hit.collider.CompareTag("StartButton"))
            {
                Debug.Log("Start Game");
                StartGame();
            }
            if (xrinput.IsTriggerPressed() && hit.collider.CompareTag("Card"))
            {
                Flip.Flip();
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("Game Started!");
        isGameStarted = true;

        // ī�� ���� ������ ����
        StartCoroutine(FlipAllCards());
        nextUI.SetActive(true);  // ���� UI Ȱ��ȭ

    }

    public IEnumerator FlipAllCards()
    {
        cardGame.FlipAllCards(); // ī�� ������ ȣ��
        //// ���� UI ��Ȱ��ȭ
        startUI.SetActive(false);
        yield return new WaitForSeconds(5f); // �ִϸ��̼� ��� �ð�
        cardGame.FlipAllCards(); // ī�� ������ ȣ��
    }
}
