using System.Collections;
using UnityEngine;

public class BossBaguette : MonoBehaviour
{
    [Header("Basic Stats")]
    public int maxHealth = 400;
    public int currentHealth;
    public float moveSpeed = 1.0f;
    public int attackDamage = 15;
    public float attackCoolTime = 2.0f;
    public int dropXP = 150;
    public GameObject experienceOrbPrefab;

    [Header("Missile Skill")]
    public GameObject missilePrefab; // 미사일 프리팹
    public GameObject toastEnemyPrefab; // 미사일 착탄 시 소환될 식빵 프리팹
    public GameObject warningMarkerPrefab; // 미사일 낙하 경고 표시 프리팹
    public float skillCooldown = 7f; // 스킬 쿨타임
    public float warningDuration = 1.5f; // 경고 표시 지속 시간

    // Private components and states
    private Transform playerTarget;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private CircleCollider2D col;
    private bool canDamage = false;
    private float lastSkillTime;
    private bool isUsingSkill = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        col = GetComponent<CircleCollider2D>();
        canDamage = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }

        lastSkillTime = -skillCooldown; // 게임 시작 시 바로 스킬 사용 가능하도록
    }

    void Update()
    {
        if (playerTarget == null || isUsingSkill)
        {
            return;
        }

        // 스킬 사용 가능 여부 확인
        if (Time.time >= lastSkillTime + skillCooldown)
        {
            StartCoroutine(FireMissileSequence());
        }
    }

    void FixedUpdate()
    {
        if (playerTarget != null && !isUsingSkill)
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

    IEnumerator FireMissileSequence()
    {
        isUsingSkill = true;
        lastSkillTime = Time.time;
        rb.velocity = Vector2.zero; // 스킬 시전 중 움직임 멈춤
        Debug.Log("Baguette Boss starts missile sequence!");

        // 1. 목표 위치 설정 (플레이어의 현재 위치)
        Vector3 targetPosition = playerTarget.position;

        // 2. 경고 표시 생성
        GameObject warningMarker = null;
        if (warningMarkerPrefab != null)
        {
            warningMarker = Instantiate(warningMarkerPrefab, targetPosition, Quaternion.identity);
        }

        // 3. 경고 시간만큼 대기
        yield return new WaitForSeconds(warningDuration);

        // 4. 경고 표시 제거
        if (warningMarker != null)
        {
            Destroy(warningMarker);
        }

        // 5. 미사일 발사
        if (missilePrefab != null)
        {
            // 보스 머리 위에서 발사되는 것처럼 보이게 처리
            Vector3 missileSpawnPos = transform.position + new Vector3(0, 2f, 0);
            GameObject missileObj = Instantiate(missilePrefab, missileSpawnPos, Quaternion.identity);
            Missile missileScript = missileObj.GetComponent<Missile>();

            if (missileScript != null)
            {
                // 미사일 스크립트에 목표 위치와 소환할 적 정보 전달
                missileScript.targetPosition = targetPosition;
                missileScript.toastEnemyPrefab = toastEnemyPrefab;
            }
            else
            {
                Debug.LogError("Missile prefab is missing the 'Missile.cs' script!");
            }
        }

        // 스킬 시전 후 짧은 딜레이
        yield return new WaitForSeconds(1.0f);
        isUsingSkill = false;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage && !isUsingSkill)
        {
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.TakeDamage(attackDamage);
            }
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Baguette Boss Died!");
        if (experienceOrbPrefab != null)
        {
            Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}