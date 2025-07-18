using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For .FirstOrDefault()
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct playerLevelDesign
{
    public int L_Level;  // 레벨
    public int L_HP; // 
    public int L_DMG; // 
    public float L_attackSpeed;
    public float L_moveSpeed;
    public int L_xpToNextLevel;
}

public class PlayerManager : MonoBehaviour
{
    public List<playerLevelDesign> playerLevelDesigns = new List<playerLevelDesign>(); // Initialize to prevent NullReferenceException
    public static PlayerManager Instance { get; private set; }

    public int maxHealth= 100; // Initialized by level design
    public int currentHealth; // Initialized by level design
    public int attackDamage=20; // Initialized by level design
    public int playerXP = 0;
    public int xpToNextLevel= 150;
    public int plusXP = 0;
    public float attackSpeed=1; // This stat is not in playerLevelDesign, so keep it as is
    public float moveSpeed=1; // Initialized by level design

    public int playerLevel = 1; // Current player level

    private bool isInvincible = false; // 무적 상태 여부
    public float invincibilityDuration = 1f; // 무적 시간 (초)
    public float attractionRange = 2f; // Not in playerLevelDesign, keep as is

    public Slider hpBar; // hp바 슬라이더
    public TemporaryCardSpawner temporaryCardSpawner; // TemporaryCardSpawner 직접 참조


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize player stats based on level 1 design
        //ApplyLevelDesign(playerLevel);
        //currentHealth = maxHealth; // Set current health to max health after applying design
        Debug.Log($"Player Health: {currentHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        maxHealth = 100;
        attackDamage = 20;
        moveSpeed = 1f;
        attackSpeed = 1f;
        xpToNextLevel = 150;
            HPBar();
    }

    // Update is called once per frame
    void Update()
    {
        HPBar();

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     Debug.Log("q 키가 눌렸습니다!");
        //     GainXP(50);
        // }
    }

    // 플레이어 hp바 메서드
    public void HPBar()
    {
        // float hpValue = currentHealth / maxHealth;
        if (hpBar != null)
        {
            hpBar.value = (float)currentHealth / (float)maxHealth;
            hpBar.transform.localScale = new Vector3(maxHealth / 100, 1.0f, 1.0f);
        }
        else
        {
            Debug.LogWarning("HP Bar Slider is not assigned in PlayerManager.");
        }
    }

    // 경험치 획득 메서드
    public void GainXP(int amount)
    {
        playerXP += (amount+plusXP);

        // Get the xpToNextLevel for the current level
        playerLevelDesign? currentLevelData = playerLevelDesigns.FirstOrDefault(d => d.L_Level == playerLevel);

        if (currentLevelData == null)
        {
            Debug.LogWarning($"PlayerLevelDesign for level {playerLevel} not found. Cannot determine xpToNextLevel.");
            return;
        }

        //int xpNeededForNextLevel = currentLevelData.Value.L_xpToNextLevel;

        //Debug.Log($"Gained {amount} XP. Current XP: {playerXP}/{xpNeededForNextLevel}");
        Debug.Log(playerXP);
        Debug.Log(xpToNextLevel);

        if (playerXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    // 레벨업 메서드
    private void LevelUp()
    {
        
        // Subtract XP for the level just completed
        playerLevelDesign? currentLevelData = playerLevelDesigns.FirstOrDefault(d => d.L_Level == playerLevel);
        if (currentLevelData != null)
        {
            playerXP -= currentLevelData.Value.L_xpToNextLevel; 
        }
        else
        {
            Debug.LogWarning($"PlayerLevelDesign for level {playerLevel} not found when leveling up. XP subtraction might be incorrect.");
        }
        
        playerLevel++;
        playerXP = 0;

        Debug.Log($"Player Leveled Up! New Level: {playerLevel}");

        ApplyLevelDesign(); // Apply new level's stats
        currentHealth = maxHealth; // Restore health on level up

        // TemporaryCardSpawner 활성화
        if (temporaryCardSpawner != null)
        {
            temporaryCardSpawner.gameObject.SetActive(true);
            Debug.Log("TemporaryCardSpawner activated on LevelUp.");
        }
        else
        {
            Debug.LogWarning("TemporaryCardSpawner reference is not set in PlayerManager. Cannot activate.");
        }
    }

    // 레벨 디자인 적용 메서드
    private void ApplyLevelDesign(int level)
    {
        if (playerLevelDesigns == null || playerLevelDesigns.Count == 0)
        {
            Debug.LogWarning("PlayerLevelDesigns list is empty or null. Please populate it in the Inspector.");
            return;
        }

        playerLevelDesign? foundDesign = playerLevelDesigns.FirstOrDefault(d => d.L_Level == level);

        if (foundDesign != null)
        {
            maxHealth = foundDesign.Value.L_HP;
            attackDamage = foundDesign.Value.L_DMG;
            moveSpeed = foundDesign.Value.L_moveSpeed;
            attackSpeed = foundDesign.Value.L_attackSpeed;
            xpToNextLevel += foundDesign.Value.L_xpToNextLevel;
            // xpToNextLevel is now read directly in GainXP, no need to store it as a separate member variable
            Debug.Log($"Applied Level {level} Design: HP={maxHealth}, DMG={attackDamage}, MoveSpeed={moveSpeed}, Attackpeed={attackSpeed}");
        }
        else
        {
            Debug.LogWarning($"PlayerLevelDesign for level {level} not found. Stats not updated.");
        }
    }

    private void ApplyLevelDesign()
    {
        if (playerLevelDesigns == null || playerLevelDesigns.Count == 0)
        {
            Debug.LogWarning("PlayerLevelDesigns list is empty or null. Please populate it in the Inspector.");
            return;
        }

        

        
        {
            maxHealth += 20;
            attackDamage += 5;
            moveSpeed += 0.1f;
            attackSpeed += 0.2f;
            xpToNextLevel = (int)(xpToNextLevel*1.5f);
            // xpToNextLevel is now read directly in GainXP, no need to store it as a separate member variable
        }  
        {
        }
    }

    // 플레이어가 피해를 입는 메서드
    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0)
        {

            return;
        }


        if (isInvincible) return; // 무적 상태면 피해를 입지 않음

        currentHealth -= damage;
        Debug.Log($"Player took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("Player Died!");
            if (SceneManager.GetActiveScene().name == "Scene_tuto")
            {
                if (PlayerManager.Instance != null)
                {
                    Destroy(PlayerManager.Instance.gameObject);
                }
                SceneManager.LoadScene("Scene_InGame");
            }
            if (SceneManager.GetActiveScene().name == "Scene_InGame")
            {
                //this.gameObject.SetActive(false);
                SceneManager.LoadScene("Scene_End");
            }
            // TODO: 게임 오버 처리 (예: 게임 재시작, UI 표시 등)
            // Destroy(gameObject); // PlayerManager는 DontDestroyOnLoad이므로 파괴하지 않음
        }
        else
        {
            // 무적 코루틴 시작
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    // 무적 코루틴
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        Debug.Log("Player is now invincible.");
        // TODO: 플레이어에게 무적 상태 시각적 피드백 추가 (예: 깜빡임, 색상 변경)

        yield return new WaitForSeconds(invincibilityDuration);

        isInvincible = false;
        Debug.Log("Player is no longer invincible.");
        // TODO: 플레이어에게 무적 상태 종료 시각적 피드백 제거
    }
}