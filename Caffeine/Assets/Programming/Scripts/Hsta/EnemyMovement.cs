using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsta;

public class EnemyMovement : MonoBehaviour
{
    public BreadType myBread;
    public int maxHealth = 10; // 최대 체력
    public int currentHealth; // 현재 체력
    public int dropXP = 10;
    public GameObject experienceOrbPrefab; // 경험치 오브 프리팹
    [SerializeField] public float moveSpeed = 3f; // 이동 속도
    [SerializeField] public int attackDamage = 10;
    private Transform playerTarget; // 플레이어의 Transform
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트

    public GameObject creamEnemyPrefab; // 크림빵 분열 시 스폰할 크림 적 프리팹
    //public int creamEnemySpawnHealth = 5; // 분열된 크림 적의 체력

    void Awake()
    {
        currentHealth = maxHealth;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on Enemy! Please add a Rigidbody2D and Collider2D to the enemy GameObject and set Gravity Scale to 0.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        // "Player" 태그를 가진 오브젝트를 찾습니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        else
        {
            Debug.LogWarning("Player GameObject with tag 'Player' not found.");
        }
       

    }

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {
        if (playerTarget != null)
        {
            // 플레이어를 향하는 방향 벡터
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            // Rigidbody2D를 사용하여 이동 (물리 충돌 처리)
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Die();
        }
    }

    // 다른 Collider2D와 충돌이 지속되는 동안 호출됩니다.
    void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // PlayerManager의 TakeDamage 메서드 호출
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.TakeDamage(attackDamage); // 10의 피해를 줍니다.
            }
            else
            {
                Debug.LogWarning("PlayerManager Instance not found.");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (myBread == BreadType.Baguette)
        {
            currentHealth -= 1;
        }
        else
        {
            currentHealth -= damage;
        }

        Debug.Log($"Enemy took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Died!");

        // 경험치 오브 드롭
        if (experienceOrbPrefab != null)
        {
            GameObject xpOrb = Instantiate(experienceOrbPrefab, transform.position, Quaternion.identity);
            ExperienceOrb orbScript = xpOrb.GetComponent<ExperienceOrb>();
            if (orbScript != null)
            {
                orbScript.xpAmount = dropXP;
                orbScript.attractionRange = PlayerManager.Instance.attractionRange;
            }
        }
        else
        {
            Debug.LogWarning("Experience Orb Prefab is not assigned in EnemyMovement script.");
        }

        if (myBread == BreadType.CreamBread)
        {
            // 크림 적 2개로 분열
            for (int i = 0; i < 2; i++)
            {
                if (creamEnemyPrefab != null)
                {
                    // 약간의 오프셋을 주어 스폰 위치 조정
                    Vector3 spawnOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                    GameObject newCreamEnemy = Instantiate(creamEnemyPrefab, transform.position + spawnOffset, Quaternion.identity);
                    
                    // 스폰된 크림 적의 체력 설정
                    EnemyMovement newEnemyMovement = newCreamEnemy.GetComponent<EnemyMovement>();
                    
                }
                else
                {
                    Debug.LogWarning("Cream Enemy Prefab is not assigned in EnemyMovement script.");
                }
            }
        }
        Destroy(gameObject); // 현재 적 오브젝트 파괴
    }
}