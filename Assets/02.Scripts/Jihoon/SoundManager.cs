using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip buttonClickSound; // ��ư Ŭ�� ����
    public AudioClip missionCompleteSound; // �̼� �Ϸ� ����
    public AudioClip slotInsertSound; // ���� ���� ����

    private AudioSource audioSource;

    void Awake()
    {
        // AudioSource ������Ʈ ��������
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource ������Ʈ�� SoundManager�� �����ϴ�!");
        }
    }

    // ��ư Ŭ�� ���� ���
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("buttonClickSound�� �������� �ʾҽ��ϴ�!");
        }
    }

    // �̼� �Ϸ� ���� ���
    public void PlayMissionCompleteSound()
    {
        if (missionCompleteSound != null)
        {
            audioSource.PlayOneShot(missionCompleteSound);
        }
        else
        {
            Debug.LogWarning("missionCompleteSound�� �������� �ʾҽ��ϴ�!");
        }
    }

    // ���� ���� ���� ���
    public void PlaySlotInsertSound()
    {
        if (slotInsertSound != null)
        {
            audioSource.PlayOneShot(slotInsertSound);
        }
        else
        {
            Debug.LogWarning("slotInsertSound�� �������� �ʾҽ��ϴ�!");
        }
    }
}
