using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip buttonClickSound; // 버튼 클릭 사운드
    public AudioClip missionCompleteSound; // 미션 완료 사운드
    public AudioClip slotInsertSound; // 슬롯 삽입 사운드

    private AudioSource audioSource;

    void Awake()
    {
        // AudioSource 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource 컴포넌트가 SoundManager에 없습니다!");
        }
    }

    // 버튼 클릭 사운드 재생
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
        else
        {
            Debug.LogWarning("buttonClickSound가 설정되지 않았습니다!");
        }
    }

    // 미션 완료 사운드 재생
    public void PlayMissionCompleteSound()
    {
        if (missionCompleteSound != null)
        {
            audioSource.PlayOneShot(missionCompleteSound);
        }
        else
        {
            Debug.LogWarning("missionCompleteSound가 설정되지 않았습니다!");
        }
    }

    // 슬롯 삽입 사운드 재생
    public void PlaySlotInsertSound()
    {
        if (slotInsertSound != null)
        {
            audioSource.PlayOneShot(slotInsertSound);
        }
        else
        {
            Debug.LogWarning("slotInsertSound가 설정되지 않았습니다!");
        }
    }
}
