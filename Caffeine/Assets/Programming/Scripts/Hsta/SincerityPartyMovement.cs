using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SincerityPartyMovement : MonoBehaviour
{
    [Header("Basic Stats")]
    public int maxHealth = 500; // 최대 체력
    public int currentHealth; // 현재 체력
    public int dropXP = 100;
    public GameObject experienceOrbPrefab; // 경험치 오브 프리팹
    [SerializeField] public float moveSpeed = 1.5f; // 이동 속도
    [SerializeField] public int attackDamage = 10; // 기본 공격력
    public float attackCoolTime = 2.0f;

    [Header("Charge Skill")]
    public float chargeSpeed = 15f;
    public float chargeDuration = 0.5f;
    public int chargeDamage = 30;
    public float chargeCooldown = 8f;
    private bool isCharging = false;
    private float lastChargeTime;

    [Header("Spawn Skill")]
    public GameObject[] enemyPrefabs; // 소환할 적 프리팹 목록
    public Transform[] spawnPoints; // 몬스터를 소환할 위치
    public float spawnCooldown = 12f;
    private float lastSpawnTime;

    // Private components and states
    private Transform playerTarget;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CircleCollider2D col;
    private bool canAttack = false;
    private float currentAttackCoolTime = 0.0f;
    private bool canDamage = false;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
            enabled = false;
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }

        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canDamage = true;

        // 쿨타임 초기화 (게임 시작 시 바로 스킬을 사용하지 않도록)
        lastChargeTime = Time.time;
        lastSpawnTime = Time.time;
    }

    void Update()
    {
        if (playerTarget == null || isCharging)
        {
            return;
        }

        // AI 행동 결정
        // 1. 소환 스킬
        if (Time.time >= lastSpawnTime + spawnCooldown)
        {
            StartCoroutine(SpawnEnemies());
        }
        // 2. 돌진 스킬
        else if (Time.time >= lastChargeTime + chargeCooldown)
        {
            StartCoroutine(Charge());
        }

        // 기본 공격 쿨타임 관리
        if (canAttack)
        {
            if (currentAttackCoolTime < attackCoolTime)
            {
                currentAttackCoolTime += Time.deltaTime;
            }
            else
            {
                currentAttackCoolTime = 0.0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && !isCharging)
        {
            SpriteFlip();
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTarget.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    IEnumerator Charge()
    {
        isCharging = true;
        lastChargeTime = Time.time;
        Debug.Log("Boss is charging!");

        // Optional: Add charging animation/effect
        // animator.SetTrigger("Charge");

        Vector2 chargeDirection = (playerTarget.position - transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;

        yield return new WaitForSeconds(chargeDuration);

        rb.velocity = Vector2.zero;
        isCharging = false;
        Debug.Log("Charge finished.");
    }

    IEnumerator SpawnEnemies()
    {
        lastSpawnTime = Time.time;
        Debug.Log("Boss is spawning enemies!");
        // Optional: Add spawning animation/effect
        // animator.SetTrigger("Spawn");

        // 소환 시 잠시 멈춤
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1.0f); // 1초 대기

        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Spawn points or enemy prefabs are not set.");
            yield break; // 코루틴 종료
        }

        foreach (Transform spawnPoint in spawnPoints)
        {
            int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[randomEnemyIndex], spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(0.3f); // 소환 간격
        }

        Debug.Log("Spawning finished.");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 돌진 중 플레이어와 충돌했는지 확인
        if (isCharging && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Charge hit player!");
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.TakeDamage(chargeDamage);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // 기본 근접 공격
        if (collision.gameObject.CompareTag("Player") && canDamage && !isCharging)
        {
            if (PlayerManager.Instance != null)
            {
                canAttack = true;
                if (currentAttackCoolTime == 0.0f)
                {
                    PlayerManager.Instance.TakeDamage(attackDamage);
                    currentAttackCoolTime += Time.deltaTime; // 즉시 다시 공격하는 것을 방지
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canAttack = false;
            currentAttackCoolTime = 0.0f;
        }
    }

    void SpriteFlip()
    {
        if (playerTarget.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Boss took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Boss Died!");

        if (experienceOrbPrefab != null)
        {
            GameObject xpOrb = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
            ExperienceOrb orbScript = xpOrb.GetComponent<ExperienceOrb>();
            if (orbScript != null)
            {
                orbScript.xpAmount = dropXP;
                if (PlayerManager.Instance != null)
                {
                    orbScript.attractionRange = PlayerManager.Instance.attractionRange;
                }
            }
        }

        canDamage = false;
        moveSpeed = 0.0f;
        if(col != null) col.isTrigger = true;
        if(animator != null) animator.SetBool("isDead?", true);

        // Stop all coroutines to prevent further actions
        StopAllCoroutines();
    }

    // This method is likely called by an animation event
    private void Dead()
    {
        Destroy(gameObject);
    }
}