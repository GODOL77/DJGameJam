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
    public float attackCoolTime = 2.0f;
    private bool canAttack = false;
    private float currentAttackCoolTime = 0.0f;
    public GameObject creamEnemyPrefab; // 크림빵 분열 시 스폰할 크림 적 프리팹
    private bool creamDied = false;
    private bool canDamage = false;
    private Animator animator; // Animator 컴포넌트
    private CircleCollider2D col; // CircleCollider2D 컴포넌트

    [Header("Red Bean Bomb Bread Settings")]
    public float explosionRadius = 3.5f; // 폭발 반경
    public int explosionDamage = 20; // 폭발 피해량
    public GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹
    public float explosionDelay = 0.5f; // 폭발 지연 시간

    private Transform playerTarget; // 플레이어의 Transform
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private SpriteRenderer spriteRenderer; // SpriteRenderer 컴포넌트
    private GameManager gameManager; // GameManager 컴포넌트


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

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Animator component not foundd on Enemy!");
        }

        col = GetComponent<CircleCollider2D>();
        if (col == null)
        {
            Debug.Log("CircleCollider2D component not found on Enenmy!");
        }

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.Log("GameManager component not found on Enemy!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        canDamage = true;
    }

    // FixedUpdate is called once per physics step
    void FixedUpdate()
    {
        if (playerTarget != null)
        {
            // 플렝이어 기준으로 왼쪽에 있는지 오른쪽에 있는지 확인하여 이미지 flip
            SpriteFlip();

            // 플레이어를 향하는 방향 벡터
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            // Rigidbody2D를 사용하여 이동 (물리 충돌 처리)
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // 단팥폭탄빵일 경우 플레이어와의 거리를 체크하여 폭발 트리거
            if (myBread == BreadType.RedBeanBombBread)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
                if (distanceToPlayer <= explosionRadius && canDamage)
                {
                    StartCoroutine(ExplodeAfterDelay());
                    canDamage = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Die();
        }

        if (canAttack)
        {
            if (currentAttackCoolTime < attackCoolTime)
            {
                currentAttackCoolTime += Time.deltaTime * 1;
            }
            else
            {
                currentAttackCoolTime = 0.0f;
            }
        }
        if (!gameManager.isRunning && gameManager.isEnemyDead)
        {
            Die();
        }
    }

    // 다른 Collider2D와 충돌이 지속되는 동안 호출됩니다.
    void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌한 오브젝트가 "Player" 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player") && canDamage)
        {
            // PlayerManager의 TakeDamage 메서드 호출
            if (PlayerManager.Instance != null)
            {
                canAttack = true;
                if (currentAttackCoolTime == 0.0f)
                {
                    Debug.LogError("Attack");
                    PlayerManager.Instance.TakeDamage(attackDamage); // 10의 피해를 줍니다.
                }
            }
            else
            {
                Debug.LogWarning("PlayerManager Instance not found.");
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canDamage)
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
        spriteRenderer.color = new Color(1,0,0,1);
        Invoke("ResetColor", 0.3f);

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
            if (myBread != BreadType.RedBeanBombBread)
            {
                Die();
            }
            else
            {
                Explode();
            }
        }
    }

    void ResetColor()
    {
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void Die()
    {
        Debug.Log("Enemy Died!");

        // 단팥폭탄빵일 경우 즉시 폭발
        if (myBread == BreadType.RedBeanBombBread)
        {
            Explode();
            return; // 폭발 후에는 일반적인 파괴 로직을 건너뜁니다.
        }

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
            if (creamDied == false)
            {
                creamDied = true;
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
        }

        canDamage = false;
        moveSpeed = 0.0f;
        col.isTrigger = true;
        animator.SetBool("isDead?", true);
    }

    private void Dead()
    {
        Destroy(gameObject); // 현재 적 오브젝트 파괴
    }

    // 폭발 메서드
    void Explode()
    {
        Debug.Log("Red Bean Bomb Bread Exploded!");

        // 폭발 이펙트 생성
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 플레이어에게 피해 주기
        if (playerTarget != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
            if (distanceToPlayer <= explosionRadius)
            {
                if (PlayerManager.Instance != null)
                {
                    PlayerManager.Instance.TakeDamage(explosionDamage);
                    Debug.Log($"Player took {explosionDamage} damage from explosion.");
                }
            }
        }
        
        // 경험치 오브 드롭 (폭발 시에도 드롭)
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

        canDamage = false;
        moveSpeed = 0.0f;
        col.isTrigger = true;
        animator.SetBool("isDead?", true);
    }

    // 지연 후 폭발 코루틴
    IEnumerator ExplodeAfterDelay()
    {
        // 폭발 지연 시간 동안 적의 움직임을 멈추거나 다른 상태로 전환할 수 있습니다.
        // 예: moveSpeed = 0;
        // 예: 애니메이션 변경

        yield return new WaitForSeconds(explosionDelay);

        Explode();
    }
}
