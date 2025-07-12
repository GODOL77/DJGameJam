using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // For .FirstOrDefault()

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

    public int maxHealth; // Initialized by level design
    public int currentHealth; // Initialized by level design
    public int attackDamage; // Initialized by level design
    public int playerXP = 0;
    public int xpToNextLevel;
    public float attackSpeed; // This stat is not in playerLevelDesign, so keep it as is
    public float moveSpeed; // Initialized by level design

    public int playerLevel = 1; // Current player level

    private bool isInvincible = false; // 무적 상태 여부
    public float invincibilityDuration = 1f; // 무적 시간 (초)
    public float attractionRange = 2f; // Not in playerLevelDesign, keep as is


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
        ApplyLevelDesign(playerLevel);
        currentHealth = maxHealth; // Set current health to max health after applying design
        Debug.Log($"Player Health: {currentHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("q 키가 눌렸습니다!");
            GainXP(50);
        }
    }

    // 경험치 획득 메서드
    public void GainXP(int amount)
    {
        playerXP += amount;
        
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

        Debug.Log($"Player Leveled Up! New Level: {playerLevel}");

        ApplyLevelDesign(playerLevel); // Apply new level's stats
        currentHealth = maxHealth; // Restore health on level up

        // 보너스 카드 선택 UI 표시
        if (BonusCardManager.Instance != null)
        {
            BonusCardManager.Instance.ShowBonusCards();
        }
        else
        {
            Debug.LogWarning("BonusCardManager Instance not found. Cannot show bonus cards.");
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
