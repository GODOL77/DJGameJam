using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI 요소를 사용하기 위해 추가

public class SettingsManager : MonoBehaviour
{
    // 1. 싱글톤 인스턴스
    public static SettingsManager Instance { get; private set; }

    // 2. 설정 값
    public float masterVolume;
    public int graphicsQuality;

    // 3. UI 요소 참조 (선택 사항, 직접 연결해도 됨)
    public Slider volumeSlider;
    public Dropdown graphicsDropdown;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 설정 불러오기
        LoadSettings();
    }

    private void Start()
    {
        // UI에 불러온 값 적용
        UpdateUI();
    }

    // 4. 설정 변경 메서드 (UI에서 호출)
    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        // TODO: 실제 오디오 리스너 볼륨 변경 로직 추가
        // AudioListener.volume = masterVolume;
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        graphicsQuality = qualityIndex;
        QualitySettings.SetQualityLevel(graphicsQuality);
    }

    // 5. 설정 저장 및 불러오기
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsQuality);
        PlayerPrefs.Save(); // 변경사항을 디스크에 즉시 저장
        Debug.Log("Settings Saved!");
    }

    public void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f); // 기본값 1
        graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 2); // 기본값 'Medium'

        // 불러온 설정 적용
        SetMasterVolume(masterVolume);
        SetGraphicsQuality(graphicsQuality);
    }

    // UI 요소에 현재 설정 값을 반영하는 메서드
    public void UpdateUI()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = masterVolume;
        }
        if (graphicsDropdown != null)
        {
            graphicsDropdown.value = graphicsQuality;
        }
    }
}


