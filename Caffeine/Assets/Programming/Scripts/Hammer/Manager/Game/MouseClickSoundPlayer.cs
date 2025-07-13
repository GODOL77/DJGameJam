using UnityEngine;

public class MouseClickSoundPlayer : MonoBehaviour
{
    public static MouseClickSoundPlayer Instance { get; private set; } // 싱글톤 인스턴스

    public AudioSource audioSource;
    public AudioClip clickSound;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 현재 GameObject 파괴
        }
        else
        {
            Instance = this; // 현재 인스턴스를 싱글톤으로 설정
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 파괴되지 않도록 설정
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0은 왼쪽 마우스 버튼을 의미합니다.
        {
            if (audioSource != null && clickSound != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}