using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    public int maxHealth = 100; // 최대 체력
    public int currentHealth; // 현재 체력
    public int attackDamage = 10;
    public int playerXP = 0;
    public float attackSpeed = 1f;
    public float moveSpeed = 5f;
    
    private bool isInvincible = false; // 무적 상태 여부
    public float invincibilityDuration = 1f; // 무적 시간 (초)

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
        currentHealth = maxHealth;
        Debug.Log($"Player Health: {currentHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
