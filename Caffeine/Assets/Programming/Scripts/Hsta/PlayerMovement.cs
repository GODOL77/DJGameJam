using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public float moveSpeed = 5f; // 플레이어 이동 속도
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    public float speed = 8f;
    private Animator animator; // Animator 컴포넌트
    private SpriteRenderer spriteRenderer; // SpriteRenderer 컴포넌트
    public Vector2 moveDirection = new Vector2(0, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on Player! Please add a Rigidbody2D and Collider2D to the player GameObject and set Gravity Scale to 0.");
            enabled = false; // 스크립트 비활성화
            return;

        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.Log("Animator component not found on Player!");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("SpriteRenderer component not found on Player!");
        }
    }

    void FixedUpdate()
    {
        // WASD 또는 화살표 키 입력 받기
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D 또는 왼쪽/오른쪽 화살표
        float moveY = Input.GetAxisRaw("Vertical");   // W/S 또는 위/아래 화살표

        // 이동 방향 벡터 계산
        moveDirection = new Vector2(moveX, moveY).normalized; // 대각선 이동 시 속도 일정하게 유지

        if (moveDirection != Vector2.zero)
        {
            animator.SetBool("isRun?", true);
        }
        else
        {
            animator.SetBool("isRun?", false);
        }

        if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Rigidbody2D를 사용하여 이동 (물리 충돌 처리)
            rb.MovePosition(rb.position + moveDirection * (PlayerManager.Instance.moveSpeed * speed) * Time.fixedDeltaTime);
    }
}
