using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text timerText;
    public float startTime = 300.0f;
    public bool isRunning = true;
    public bool isEnemyDead = false;
    private float currentTime;
    private Boss_Spawner bossSpawner;

    void Start()
    {
        currentTime = startTime;

        bossSpawner = GameObject.FindWithTag("Boss_Spawner").GetComponent<Boss_Spawner>();
        if (bossSpawner == null)
        {
            Debug.Log("Boss Spawner component not found on GameManager!");
        }
    }

    void Update()
    {
        InGameTimer();
    }

    private void InGameTimer()
    {
        if (!isRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            isEnemyDead = true;
            bossSpawner.BossSpawn();
        }

        // 분:초 형식으로 변환해서 UI 표시
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
