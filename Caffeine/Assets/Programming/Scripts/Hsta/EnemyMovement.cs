using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 이동 속도
    private Transform playerTarget; // 플레이어의 Transform
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트


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
                PlayerManager.Instance.TakeDamage(10); // 10의 피해를 줍니다.
            }
            else
            {
                Debug.LogWarning("PlayerManager Instance not found.");
            }
        }
    }
}